using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NRAdmanWebApplicationNet10.Models;
using NRAdmanWebApplicationNet10.Services;
using NRAdmanWebApplicationNet10.Services.Mikrotik;
using System.Text.Json;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class MikrotikRadiusDeploymentController(
        ApplicationDbContext applicationDbContext,
        MikrotikSyncService syncService,
        ILogger<MikrotikRadiusDeploymentController> logger) : Controller
    {
        /*private readonly ApplicationDbContext _db = db;
        private readonly MikrotikSyncService _syncService = syncService;
        private readonly ILogger<MikrotikRadiusDeploymentController> _logger = logger;*/

        /// <summary>
        /// Halaman manajemen deployment
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get data deployment untuk DataTable
        /// </summary>
        [HttpGet]
        public IActionResult GetJsonResult()
        {
            try
            {
                var data = applicationDbContext.MikrotikQueueConfigs
                    .Include(c => c.Router)
                    .Include(c => c.Policy)
                    .Select(c => new
                    {
                        id = c.Id,
                        routerName = c.Router.Name,
                        policyName = c.Policy != null ? c.Policy.PolicyName : "Unknown",
                        queueName = c.QueueName,
                        targetAddress = c.TargetAddress,
                        deploymentStatus = c.DeploymentStatus.ToString(),
                        syncStatus = c.SyncStatus.ToString(),
                        deployedDate = c.DeployedDate.HasValue ? c.DeployedDate.Value.ToString("yyyy-MM-dd HH:mm") : "-",
                        lastSyncDate = c.LastSyncDate.HasValue ? c.LastSyncDate.Value.ToString("yyyy-MM-dd HH:mm") : "-",
                        error = c.LastError ?? "-"
                    }).ToList();

                return Json(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting deployment data");
                return Json(new { error = "Error loading data" });
            }
        }

        /// <summary>
        /// Halaman deploy policy baru
        /// </summary>
        public IActionResult Deploy()
        {
            var policies = applicationDbContext.MikrotikRadiusPolicies.Where(p => p.IsActive).ToList();
            var routers = applicationDbContext.NetworkRouters.ToList();

            ViewBag.Policies = policies;
            ViewBag.Routers = routers;

            return View();
        }

        /// <summary>
        /// Execute deploy policy
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ExecuteDeploy(Guid policyId, List<string> selectedRouters, List<string> targetAddresses)
        {
            try
            {
                if (selectedRouters.Count == 0)
                {
                    return Json(new { success = false, message = "Pilih minimal satu router" });
                }

                var policy = applicationDbContext.MikrotikRadiusPolicies.FirstOrDefault(p => p.Id == policyId);
                if (policy == null)
                {
                    return Json(new { success = false, message = "Policy tidak ditemukan" });
                }

                var deployments = new List<(Guid, string)>();
                for (int i = 0; i < selectedRouters.Count && i < targetAddresses.Count; i++)
                {
                    if (Guid.TryParse(selectedRouters[i], out var routerId))
                    {
                        deployments.Add((routerId, targetAddresses[i]));
                    }
                }

                var result = await syncService.DeployPolicyToMultipleRoutersAsync(policyId, deployments);

                return Json(new
                {
                    success = result.Success,
                    message = result.Message,
                    deployed = result.QueuesDeployed,
                    failed = result.FailedOperations,
                    errors = result.Errors
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing deploy");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Sync queue status dengan Mikrotik
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SyncStatus()
        {
            try
            {
                var result = await syncService.SyncQueueStatusAsync();

                return Json(new
                {
                    success = result.Success,
                    message = result.Message,
                    synced = result.QueuesSynced,
                    failed = result.FailedOperations,
                    stats = result.SyncStats
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sync status");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Pull accounting data dari Mikrotik
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PullAccounting(Guid? routerId = null)
        {
            try
            {
                var result = await syncService.PullAccountingDataAsync(routerId);

                return Json(new
                {
                    success = result.Success,
                    message = result.Message,
                    records = result.QueuesSynced,
                    failed = result.FailedOperations,
                    stats = result.SyncStats
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error pull accounting");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Rollback deployment
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Rollback(List<Guid> configIds)
        {
            try
            {
                if (!configIds.Any())
                {
                    return Json(new { success = false, message = "Pilih minimal satu queue untuk rollback" });
                }

                var result = await syncService.RollbackDeploymentAsync(configIds);

                return Json(new
                {
                    success = result.Success,
                    message = result.Message,
                    rolledBack = result.QueuesDeployed,
                    failed = result.FailedOperations,
                    errors = result.Errors
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error rollback");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get deployment status
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var status = await syncService.GetDeploymentStatusAsync();
                return Json(status);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting status");
                return Json(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get detail queue config
        /// </summary>
        [HttpGet]
        public IActionResult GetDetail(Guid id)
        {
            try
            {
                var config = applicationDbContext.MikrotikQueueConfigs
                    .Include(c => c.Router)
                    .Include(c => c.Policy)
                    .FirstOrDefault(c => c.Id == id);

                if (config == null)
                    return NotFound();

                var metadata = !string.IsNullOrEmpty(config.ConfigMetadata)
                    ? JsonSerializer.Deserialize<Dictionary<string, string>>(config.ConfigMetadata)
                    : new Dictionary<string, string>();

                return Json(new
                {
                    id = config.Id,
                    routerName = config.Router?.Name,
                    policyName = config.Policy?.PolicyName,
                    queueName = config.QueueName,
                    targetAddress = config.TargetAddress,
                    mikrotikQueueId = config.MikrotikQueueId,
                    deploymentStatus = config.DeploymentStatus.ToString(),
                    syncStatus = config.SyncStatus.ToString(),
                    deployedDate = config.DeployedDate,
                    lastSyncDate = config.LastSyncDate,
                    lastError = config.LastError,
                    configVersion = config.ConfigVersion,
                    metadata = metadata
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting detail");
                return Json(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get router details untuk deployment form
        /// </summary>
        [HttpGet]
        public IActionResult GetRouterDetails(Guid routerId)
        {
            try
            {
                var router = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.Id == routerId);
                if (router == null)
                    return NotFound();

                return Json(new
                {
                    routerId = router.Id,
                    routerName = router.Name,
                    managementIp = router.IpAddress,
                    apiPort = "8729",
                    routerType = router.RouterType.ToString()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting router details");
                return Json(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Test koneksi ke Mikrotik router
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> TestConnection(Guid routerId)
        {
            try
            {
                var router = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.Id == routerId);
                if (router == null)
                    return Json(new { success = false, message = "Router tidak ditemukan" });

                var apiService = HttpContext.RequestServices.GetRequiredService<MikrotikApiService>();
                var connSettings = new MikrotikConnectionSettings
                {
                    RouterName = router.Name,
                    ApiHost = router.IpAddress,
                    ApiPort = 8729,
                    ApiUsername = router.Username,
                    ApiPassword = router.Password,
                    UseSSL = true,
                    IgnoreCertificate = true
                };

                var result = await apiService.TestConnectionAsync(connSettings);

                return Json(new
                {
                    success = result.Success,
                    message = result.Success ? "Koneksi berhasil" : result.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error test connection");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Export deployment script
        /// </summary>
        [HttpGet]
        public IActionResult ExportScript(Guid policyId)
        {
            try
            {
                var policy = applicationDbContext.MikrotikRadiusPolicies.FirstOrDefault(p => p.Id == policyId);
                if (policy == null)
                    return NotFound();

                var policyService = HttpContext.RequestServices.GetRequiredService<MikrotikPolicyApplicationService>();
                var routers = applicationDbContext.NetworkRouters.ToList();

                var deployments = routers.Select(r => (r, r.IpAddress)).ToList();
                var script = policyService.GenerateDeploymentScript(policy, deployments);

                return File(
                    System.Text.Encoding.UTF8.GetBytes(script),
                    "text/plain",
                    $"deployment-{policy.PolicyName}-{DateTime.UtcNow:yyyyMMddHHmmss}.rsc"
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error export script");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get deployment statistics
        /// </summary>
        [HttpGet]
        public IActionResult GetStatistics()
        {
            try
            {
                var totalConfigs = applicationDbContext.MikrotikQueueConfigs.Count();
                var byStatus = applicationDbContext.MikrotikQueueConfigs
                    .GroupBy(c => c.DeploymentStatus)
                    .Select(g => new { status = g.Key.ToString(), count = g.Count() })
                    .ToList();

                var bySyncStatus = applicationDbContext.MikrotikQueueConfigs
                    .GroupBy(c => c.SyncStatus)
                    .Select(g => new { status = g.Key.ToString(), count = g.Count() })
                    .ToList();

                var byRouter = applicationDbContext.MikrotikQueueConfigs
                    .Include(c => c.Router)
                    .GroupBy(c => c.Router.Name)
                    .Select(g => new { router = g.Key, count = g.Count() })
                    .ToList();

                var byPolicy = applicationDbContext.MikrotikQueueConfigs
                    .Include(c => c.Policy)
                    .GroupBy(c => c.Policy.PolicyName ?? "Unknown")
                    .Select(g => new { policy = g.Key, count = g.Count() })
                    .ToList();

                return Json(new
                {
                    totalConfigs,
                    byStatus,
                    bySyncStatus,
                    byRouter,
                    byPolicy
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting statistics");
                return Json(new { error = ex.Message });
            }
        }
    }
}
