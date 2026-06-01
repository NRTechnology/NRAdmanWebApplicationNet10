using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NRAdmanWebApplicationNet10.Models;
using NRAdmanWebApplicationNet10.Services;
using System.Text.Json;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class MikrotikRadiusProfileController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<MikrotikRadiusProfileController> _logger;

        public MikrotikRadiusProfileController(
            ApplicationDbContext db,
            ILogger<MikrotikRadiusProfileController> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Halaman manajemen RADIUS policies
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get data policies untuk DataTable
        /// </summary>
        [HttpGet]
        public IActionResult GetJsonResult()
        {
            try
            {
                var data = _db.MikrotikRadiusPolicies
                    .OrderByDescending(p => p.CreatedDate)
                    .Select(p => new
                    {
                        id = p.Id,
                        policyName = p.PolicyName,
                        description = p.Description ?? "-",
                        downloadLimit = p.DownloadLimit.HasValue ? $"{p.DownloadLimit}M" : "Unlimited",
                        uploadLimit = p.UploadLimit.HasValue ? $"{p.UploadLimit}M" : "Unlimited",
                        burstLimitDown = p.BurstLimitDown.HasValue ? $"{p.BurstLimitDown}M" : "-",
                        burstLimitUp = p.BurstLimitUp.HasValue ? $"{p.BurstLimitUp}M" : "-",
                        priority = p.Priority,
                        isActive = p.IsActive ? "Active" : "Inactive",
                        createdDate = p.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                        modifiedDate = p.ModifiedDate.HasValue ? p.ModifiedDate.Value.ToString("yyyy-MM-dd HH:mm") : "-"
                    }).ToList();

                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting policy data");
                return Json(new { error = "Error loading data" });
            }
        }

        /// <summary>
        /// Form create policy
        /// </summary>
        public IActionResult Create()
        {
            return View(new MikrotikRadiusPolicy());
        }

        /// <summary>
        /// Create policy
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(MikrotikRadiusPolicy model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Validasi gagal" });
                }

                // Check duplicate policy name
                var existing = await _db.MikrotikRadiusPolicies
                    .FirstOrDefaultAsync(p => p.PolicyName == model.PolicyName);
                if (existing != null)
                {
                    return Json(new { success = false, message = "Policy name sudah digunakan" });
                }

                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = User.Identity?.Name ?? "System";

                _db.MikrotikRadiusPolicies.Add(model);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Policy '{model.PolicyName}' dibuat oleh {User.Identity?.Name}");

                return Json(new { success = true, message = "Policy berhasil dibuat", id = model.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error create policy");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Form edit policy
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var policy = await _db.MikrotikRadiusPolicies.FindAsync(id);
                if (policy == null)
                    return NotFound();

                return View(policy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update policy
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MikrotikRadiusPolicy model)
        {
            try
            {
                if (id != model.Id)
                    return BadRequest();

                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Validasi gagal" });

                var existing = await _db.MikrotikRadiusPolicies.FindAsync(id);
                if (existing == null)
                    return NotFound();

                // Update properties
                existing.PolicyName = model.PolicyName;
                existing.Description = model.Description;
                existing.DownloadLimit = model.DownloadLimit;
                existing.UploadLimit = model.UploadLimit;
                existing.BurstLimitDown = model.BurstLimitDown;
                existing.BurstLimitUp = model.BurstLimitUp;
                existing.BurstThresholdDown = model.BurstThresholdDown;
                existing.BurstThresholdUp = model.BurstThresholdUp;
                existing.BurstTime = model.BurstTime;
                existing.Priority = model.Priority;
                existing.IsActive = model.IsActive;
                existing.ModifiedDate = DateTime.UtcNow;
                existing.ModifiedBy = User.Identity?.Name ?? "System";

                _db.MikrotikRadiusPolicies.Update(existing);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Policy '{existing.PolicyName}' diupdate oleh {User.Identity?.Name}");

                return Json(new { success = true, message = "Policy berhasil diupdate" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error update policy");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get policy detail (AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var policy = await _db.MikrotikRadiusPolicies.FindAsync(id);
                if (policy == null)
                    return NotFound();

                return Json(new
                {
                    id = policy.Id,
                    policyName = policy.PolicyName,
                    description = policy.Description,
                    downloadLimit = policy.DownloadLimit,
                    uploadLimit = policy.UploadLimit,
                    burstLimitDown = policy.BurstLimitDown,
                    burstLimitUp = policy.BurstLimitUp,
                    burstThresholdDown = policy.BurstThresholdDown,
                    burstThresholdUp = policy.BurstThresholdUp,
                    burstTime = policy.BurstTime,
                    priority = policy.Priority,
                    isActive = policy.IsActive,
                    createdDate = policy.CreatedDate,
                    modifiedDate = policy.ModifiedDate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting policy detail");
                return Json(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Delete policy
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var policy = await _db.MikrotikRadiusPolicies.FindAsync(id);
                if (policy == null)
                    return Json(new { success = false, message = "Policy tidak ditemukan" });

                // Check if policy is deployed
                var deployedCount = await _db.MikrotikQueueConfigs
                    .CountAsync(q => q.PolicyId == id && q.DeploymentStatus == EnumDeploymentStatus.Deployed);

                if (deployedCount > 0)
                    return Json(new { success = false, message = "Policy masih di-deploy. Rollback terlebih dahulu sebelum menghapus." });

                _db.MikrotikRadiusPolicies.Remove(policy);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Policy '{policy.PolicyName}' dihapus oleh {User.Identity?.Name}");

                return Json(new { success = true, message = "Policy berhasil dihapus" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error delete policy");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get statistics
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var totalPolicies = await _db.MikrotikRadiusPolicies.CountAsync();
                var activePolicies = await _db.MikrotikRadiusPolicies.CountAsync(p => p.IsActive);
                var deployedPolicies = await _db.MikrotikQueueConfigs
                    .Where(q => q.DeploymentStatus == EnumDeploymentStatus.Deployed)
                    .Select(q => q.PolicyId)
                    .Distinct()
                    .CountAsync();

                return Json(new
                {
                    totalPolicies,
                    activePolicies,
                    inactivePolicies = totalPolicies - activePolicies,
                    deployedPolicies
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting statistics");
                return Json(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Activate/Deactivate policy
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            try
            {
                var policy = await _db.MikrotikRadiusPolicies.FindAsync(id);
                if (policy == null)
                    return Json(new { success = false, message = "Policy tidak ditemukan" });

                policy.IsActive = !policy.IsActive;
                policy.ModifiedDate = DateTime.UtcNow;
                policy.ModifiedBy = User.Identity?.Name ?? "System";

                _db.MikrotikRadiusPolicies.Update(policy);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Policy '{policy.PolicyName}' status diubah menjadi {(policy.IsActive ? "Active" : "Inactive")}");

                return Json(new { success = true, message = $"Policy berhasil diubah menjadi {(policy.IsActive ? "Active" : "Inactive")}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling policy status");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
