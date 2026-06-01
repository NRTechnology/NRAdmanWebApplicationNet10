using Microsoft.EntityFrameworkCore;
using NRAdmanWebApplicationNet10.Models;
using NRAdmanWebApplicationNet10.Services;
using System.Text.Json;

namespace NRAdmanWebApplicationNet10.Services.Mikrotik
{
    /// <summary>
    /// DTO untuk sync operation result
    /// </summary>
    public class MikrotikSyncResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int QueuesSynced { get; set; }
        public int QueuesDeployed { get; set; }
        public int FailedOperations { get; set; }
        public List<string>? Errors { get; set; }
        public Dictionary<string, object>? SyncStats { get; set; }
        public DateTime SyncDateTime { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// DTO untuk deployment operation result
    /// </summary>
    public class DeploymentResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? MikrotikQueueId { get; set; }
        public int ConfigId { get; set; }
        public DateTime DeployedAt { get; set; } = DateTime.UtcNow;
        public string? ErrorDetails { get; set; }
    }

    /// <summary>
    /// Service untuk orchestrasi sinkronisasi data RADIUS pada Mikrotik
    /// Menghandle deployment policy, sinkronisasi status, dan pull accounting data
    /// </summary>
    public class MikrotikSyncService
    {
        private readonly ApplicationDbContext _db;
        private readonly MikrotikApiService _apiService;
        private readonly MikrotikPolicyApplicationService _policyService;
        private readonly ILogger<MikrotikSyncService> _logger;

        public MikrotikSyncService(
            ApplicationDbContext db,
            MikrotikApiService apiService,
            MikrotikPolicyApplicationService policyService,
            ILogger<MikrotikSyncService> logger)
        {
            _db = db;
            _apiService = apiService;
            _policyService = policyService;
            _logger = logger;
        }

        /// <summary>
        /// Deploy policy ke satu router dengan target address tertentu
        /// </summary>
        public async Task<DeploymentResult> DeployPolicyToRouterAsync(
            int policyId,
            Guid routerId,
            string targetAddress,
            string? customQueueName = null)
        {
            try
            {
                // Get policy
                var policy = _db.MikrotikRadiusPolicies.FirstOrDefault(p => p.Id == policyId);
                if (policy == null)
                {
                    return new DeploymentResult
                    {
                        Success = false,
                        Message = "Policy tidak ditemukan",
                        ErrorDetails = $"Policy ID {policyId} tidak ditemukan di database"
                    };
                }

                // Get router
                var router = _db.Routers.FirstOrDefault(r => r.Id == routerId);
                if (router == null)
                {
                    return new DeploymentResult
                    {
                        Success = false,
                        Message = "Router tidak ditemukan",
                        ErrorDetails = $"Router ID {routerId} tidak ditemukan di database"
                    };
                }

                // Validasi policy
                var (isValid, errors) = _policyService.ValidatePolicyApplication(policy, targetAddress);
                if (!isValid)
                {
                    return new DeploymentResult
                    {
                        Success = false,
                        Message = "Validasi policy gagal",
                        ErrorDetails = string.Join(", ", errors)
                    };
                }

                // Convert policy to queue command
                var queueCommand = _policyService.ConvertPolicyToQueueCommand(
                    policy,
                    targetAddress,
                    customQueueName);

                // Create connection settings
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

                // Test connection
                var connTest = await _apiService.TestConnectionAsync(connSettings);
                if (!connTest.Success)
                {
                    return new DeploymentResult
                    {
                        Success = false,
                        Message = "Koneksi ke router gagal",
                        ErrorDetails = connTest.ErrorMessage
                    };
                }

                // Create queue on Mikrotik
                var queueResult = await _apiService.CreateSimpleQueueAsync(
                    connSettings,
                    queueCommand.QueueName,
                    queueCommand.TargetAddress,
                    queueCommand.MaxLimitDown ?? "unlimited",
                    queueCommand.MaxLimitUp ?? "unlimited",
                    queueCommand.BurstLimitDown,
                    queueCommand.BurstLimitUp,
                    queueCommand.Priority ?? 8,
                    queueCommand.IsDisabled ?? false);

                if (!queueResult.Success)
                {
                    return new DeploymentResult
                    {
                        Success = false,
                        Message = "Pembuatan queue di Mikrotik gagal",
                        ErrorDetails = queueResult.ErrorMessage
                    };
                }

                // Save to database
                var queueConfig = new MikrotikQueueConfig
                {
                    RouterId = routerId,
                    PolicyId = policyId,
                    MikrotikQueueId = queueResult.Data?.Id,
                    QueueName = queueCommand.QueueName,
                    TargetAddress = targetAddress,
                    DeploymentStatus = EnumDeploymentStatus.Deployed,
                    SyncStatus = EnumSyncStatus.InSync,
                    DeployedDate = DateTime.UtcNow,
                    LastSyncDate = DateTime.UtcNow,
                    CreatedBy = "System",
                    ConfigMetadata = JsonSerializer.Serialize(queueCommand.ToMikrotikAttributes())
                };

                _db.MikrotikQueueConfigs.Add(queueConfig);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Policy {policyId} berhasil di-deploy ke router {routerId} dengan queue ID {queueResult.Data?.Id}");

                return new DeploymentResult
                {
                    Success = true,
                    Message = "Policy berhasil di-deploy",
                    MikrotikQueueId = queueResult.Data?.Id,
                    ConfigId = queueConfig.Id,
                    DeployedAt = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deploy policy {policyId} ke router {routerId}");
                return new DeploymentResult
                {
                    Success = false,
                    Message = "Error saat deploy",
                    ErrorDetails = ex.Message
                };
            }
        }

        /// <summary>
        /// Deploy policy ke multiple routers
        /// </summary>
        public async Task<MikrotikSyncResult> DeployPolicyToMultipleRoutersAsync(
            int policyId,
            List<(Guid RouterId, string TargetAddress)> deployments)
        {
            var result = new MikrotikSyncResult
            {
                Success = true,
                Errors = new List<string>(),
                SyncStats = new Dictionary<string, object>()
            };

            foreach (var (routerId, targetAddress) in deployments)
            {
                var deployResult = await DeployPolicyToRouterAsync(policyId, routerId, targetAddress);

                if (deployResult.Success)
                {
                    result.QueuesDeployed++;
                }
                else
                {
                    result.FailedOperations++;
                    result.Errors.Add($"Router {routerId}: {deployResult.Message}");
                }
            }

            result.Success = result.FailedOperations == 0;
            result.Message = $"Deployment selesai. Berhasil: {result.QueuesDeployed}, Gagal: {result.FailedOperations}";
            result.SyncStats["total-deployments"] = deployments.Count;
            result.SyncStats["successful"] = result.QueuesDeployed;
            result.SyncStats["failed"] = result.FailedOperations;

            return result;
        }

        /// <summary>
        /// Sinkronisasi status queue dari semua router
        /// </summary>
        public async Task<MikrotikSyncResult> SyncQueueStatusAsync()
        {
            var result = new MikrotikSyncResult
            {
                Success = true,
                Errors = new List<string>(),
                SyncStats = new Dictionary<string, object>()
            };

            try
            {
                // Get all active queue configs
                var configs = _db.MikrotikQueueConfigs
                    .Where(c => c.DeploymentStatus == EnumDeploymentStatus.Deployed)
                    .Include(c => c.Router)
                    .Include(c => c.Policy)
                    .ToList();

                if (!configs.Any())
                {
                    result.Message = "Tidak ada queue yang di-deploy";
                    return result;
                }

                // Group by router
                var configsByRouter = configs.GroupBy(c => c.Router).ToList();

                foreach (var routerGroup in configsByRouter)
                {
                    try
                    {
                        var router = routerGroup.Key;
                        var connSettings = new MikrotikConnectionSettings
                        {
                            RouterName = router?.Name,
                            ApiHost = router?.IpAddress,
                            ApiPort = 8729,
                            ApiUsername = router?.Username,
                            ApiPassword = router?.Password,
                            UseSSL = true,
                            IgnoreCertificate = true
                        };

                        // Get queues from router
                        var queuesResult = await _apiService.GetSimpleQueuesAsync(connSettings);
                        if (!queuesResult.Success)
                        {
                            result.Errors.Add($"Router {router?.Name}: {queuesResult.ErrorMessage}");
                            result.FailedOperations++;
                            continue;
                        }

                        // Check each config
                        foreach (var config in routerGroup)
                        {
                            var queueExists = queuesResult.Data?.Any(q => q.Id == config.MikrotikQueueId) ?? false;

                            if (queueExists)
                            {
                                config.SyncStatus = EnumSyncStatus.InSync;
                            }
                            else
                            {
                                config.SyncStatus = EnumSyncStatus.OutOfSync;
                                config.LastError = "Queue tidak ditemukan di router";
                            }

                            config.LastSyncDate = DateTime.UtcNow;
                            _db.MikrotikQueueConfigs.Update(config);
                            result.QueuesSynced++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error sinkronisasi router {routerGroup.Key?.Name}");
                        result.FailedOperations++;
                    }
                }

                await _db.SaveChangesAsync();

                result.Success = result.FailedOperations == 0;
                result.Message = $"Sinkronisasi selesai. {result.QueuesSynced} queue disinkronisasi, {result.FailedOperations} gagal";
                result.SyncStats["total-queues"] = configs.Count;
                result.SyncStats["routers-synced"] = configsByRouter.Count;

                _logger.LogInformation($"Sync status berhasil: {result.QueuesSynced} queues, {result.FailedOperations} failures");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sinkronisasi queue status");
                result.Success = false;
                result.Message = "Error saat sinkronisasi";
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        /// <summary>
        /// Pull accounting data dari Mikrotik dan simpan ke database
        /// </summary>
        public async Task<MikrotikSyncResult> PullAccountingDataAsync(Guid? routerId = null)
        {
            var result = new MikrotikSyncResult
            {
                Success = true,
                Errors = new List<string>(),
                SyncStats = new Dictionary<string, object>()
            };

            try
            {
                var routers = routerId.HasValue
                    ? _db.Routers.Where(r => r.Id == routerId).ToList()
                    : _db.Routers.ToList();

                foreach (var router in routers)
                {
                    try
                    {
                        // Create connection settings
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

                        // Test connection
                        var connTest = await _apiService.TestConnectionAsync(connSettings);
                        if (!connTest.Success)
                        {
                            result.Errors.Add($"Router {router.Name}: Koneksi gagal");
                            result.FailedOperations++;
                            continue;
                        }

                        // Simulasi: ambil queue stats sebagai accounting data
                        var queues = _db.MikrotikQueueConfigs
                            .Where(q => q.RouterId == router.Id && q.DeploymentStatus == EnumDeploymentStatus.Deployed)
                            .ToList();

                        foreach (var queue in queues)
                        {
                            if (string.IsNullOrEmpty(queue.MikrotikQueueId))
                                continue;

                            // Get stats
                            var statsResult = await _apiService.GetQueueStatsAsync(connSettings, queue.MikrotikQueueId);
                            if (statsResult.Success && statsResult.Data != null)
                            {
                                // Create accounting record
                                var acctRecord = new MikrotikRadiusAccounting
                                {
                                    Username = $"Queue-{queue.QueueName}",
                                    NasIpAddress = router.IpAddress,
                                    AcctInputOctets = statsResult.Data.ContainsKey("bytes-in") ? (long?)Convert.ToInt64(statsResult.Data["bytes-in"]) : null,
                                    AcctOutputOctets = statsResult.Data.ContainsKey("bytes-out") ? (long?)Convert.ToInt64(statsResult.Data["bytes-out"]) : null,
                                    AcctStatusType = "Interim-Update",
                                    CreatedDate = DateTime.UtcNow
                                };

                                _db.MikrotikRadiusAccounting.Add(acctRecord);
                                result.QueuesSynced++;
                            }

                            queue.LastSyncDate = DateTime.UtcNow;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error pull accounting data dari router {router.Name}");
                        result.FailedOperations++;
                    }
                }

                await _db.SaveChangesAsync();

                result.Message = $"Pull accounting selesai. {result.QueuesSynced} records, {result.FailedOperations} gagal";
                result.SyncStats["records-pulled"] = result.QueuesSynced;
                result.SyncStats["routers-processed"] = routers.Count;

                _logger.LogInformation($"Pull accounting berhasil: {result.QueuesSynced} records");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pull accounting data");
                result.Success = false;
                result.Message = "Error saat pull accounting";
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        /// <summary>
        /// Rollback deployment dari satu atau multiple queues
        /// </summary>
        public async Task<MikrotikSyncResult> RollbackDeploymentAsync(List<int> configIds)
        {
            var result = new MikrotikSyncResult
            {
                Success = true,
                Errors = new List<string>()
            };

            try
            {
                var configs = _db.MikrotikQueueConfigs
                    .Where(c => configIds.Contains(c.Id))
                    .Include(c => c.Router)
                    .ToList();

                foreach (var config in configs)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(config.MikrotikQueueId) || config.Router == null)
                        {
                            result.Errors.Add($"Config {config.Id}: Missing queue ID or router");
                            result.FailedOperations++;
                            continue;
                        }

                        var connSettings = new MikrotikConnectionSettings
                        {
                            RouterName = config.Router.Name,
                            ApiHost = config.Router.IpAddress,
                            ApiPort = 8729,
                            ApiUsername = config.Router.Username,
                            ApiPassword = config.Router.Password,
                            UseSSL = true,
                            IgnoreCertificate = true
                        };

                        // Delete queue from Mikrotik
                        var deleteResult = await _apiService.DeleteSimpleQueueAsync(connSettings, config.MikrotikQueueId);
                        if (deleteResult.Success)
                        {
                            config.DeploymentStatus = EnumDeploymentStatus.RolledBack;
                            config.SyncStatus = EnumSyncStatus.NotSynced;
                            _db.MikrotikQueueConfigs.Update(config);
                            result.QueuesDeployed++;
                        }
                        else
                        {
                            result.Errors.Add($"Config {config.Id}: {deleteResult.ErrorMessage}");
                            result.FailedOperations++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error rollback config {config.Id}");
                        result.Errors.Add($"Config {config.Id}: {ex.Message}");
                        result.FailedOperations++;
                    }
                }

                await _db.SaveChangesAsync();

                result.Message = $"Rollback selesai. Berhasil: {result.QueuesDeployed}, Gagal: {result.FailedOperations}";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rollback deployment");
                result.Success = false;
                result.Message = "Error saat rollback";
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        /// <summary>
        /// Get deployment status untuk semua queue
        /// </summary>
        public async Task<Dictionary<string, object>> GetDeploymentStatusAsync()
        {
            var status = new Dictionary<string, object>();

            try
            {
                var totalConfigs = _db.MikrotikQueueConfigs.Count();
                var deployedCount = _db.MikrotikQueueConfigs.Count(c => c.DeploymentStatus == EnumDeploymentStatus.Deployed);
                var inSyncCount = _db.MikrotikQueueConfigs.Count(c => c.SyncStatus == EnumSyncStatus.InSync);
                var outOfSyncCount = _db.MikrotikQueueConfigs.Count(c => c.SyncStatus == EnumSyncStatus.OutOfSync);

                status["total-configs"] = totalConfigs;
                status["deployed-count"] = deployedCount;
                status["in-sync-count"] = inSyncCount;
                status["out-of-sync-count"] = outOfSyncCount;
                status["deployment-success-rate"] = totalConfigs > 0 ? (double)deployedCount / totalConfigs * 100 : 0;
                status["sync-success-rate"] = totalConfigs > 0 ? (double)inSyncCount / totalConfigs * 100 : 0;

                var lastSync = _db.MikrotikQueueConfigs.OrderByDescending(c => c.LastSyncDate).FirstOrDefault();
                status["last-sync-date"] = lastSync?.LastSyncDate;

                return await Task.FromResult(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting deployment status");
                status["error"] = ex.Message;
                return status;
            }
        }
    }
}
