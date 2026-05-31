using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRAdmanWebApplicationNet10.Models;
using NRAdmanWebApplicationNet10.Services;
using NRAdmanWebApplicationNet10.ViewModels;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class MikrotikRadiusProfileController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<MikrotikRadiusProfileController> _logger;

        public MikrotikRadiusProfileController(ApplicationDbContext db, ILogger<MikrotikRadiusProfileController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("MikrotikRadiusProfileList");
        }

        [HttpGet]
        public IActionResult GetJsonResult()
        {
            try
            {
                var data = _db.MikrotikRadiusPolicies.Select(p => new
                {
                    id = p.Id,
                    policyName = p.PolicyName,
                    description = p.Description,
                    downloadLimit = p.DownloadLimit.HasValue ? $"{p.DownloadLimit} Mbps" : "-",
                    uploadLimit = p.UploadLimit.HasValue ? $"{p.UploadLimit} Mbps" : "-",
                    queueType = p.QueueType.ToString(),
                    priority = p.Priority,
                    isActive = p.IsActive ? "Active" : "Inactive",
                    createdDate = p.CreatedDate.ToString("yyyy-MM-dd HH:mm")
                }).ToList();

                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Radius policies");
                return Json(new { error = "Error loading data" });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new MikrotikRadiusPolicyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MikrotikRadiusPolicyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check for duplicate policy name
            if (_db.MikrotikRadiusPolicies.Any(p => p.PolicyName == model.PolicyName))
            {
                ModelState.AddModelError(nameof(model.PolicyName), "Policy name sudah ada.");
                return View(model);
            }

            try
            {
                var entity = new MikrotikRadiusPolicy
                {
                    PolicyName = model.PolicyName,
                    Description = model.Description,
                    DownloadLimit = model.DownloadLimit,
                    UploadLimit = model.UploadLimit,
                    QueueType = model.QueueType,
                    Priority = model.Priority,
                    BurstLimitDown = model.BurstLimitDown,
                    BurstLimitUp = model.BurstLimitUp,
                    BurstThresholdDown = model.BurstThresholdDown,
                    BurstThresholdUp = model.BurstThresholdUp,
                    BurstTime = model.BurstTime,
                    MaxLimitDown = model.MaxLimitDown,
                    MaxLimitUp = model.MaxLimitUp,
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = User.Identity?.Name
                };

                _db.MikrotikRadiusPolicies.Add(entity);
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Radius policy berhasil dibuat.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Radius policy");
                ModelState.AddModelError("", "Gagal menyimpan data. Silahkan coba lagi.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var policy = _db.MikrotikRadiusPolicies.FirstOrDefault(p => p.Id == id);
            if (policy == null)
                return NotFound();

            var model = new MikrotikRadiusPolicyViewModel
            {
                Id = policy.Id,
                PolicyName = policy.PolicyName,
                Description = policy.Description,
                DownloadLimit = policy.DownloadLimit,
                UploadLimit = policy.UploadLimit,
                QueueType = policy.QueueType,
                Priority = policy.Priority,
                BurstLimitDown = policy.BurstLimitDown,
                BurstLimitUp = policy.BurstLimitUp,
                BurstThresholdDown = policy.BurstThresholdDown,
                BurstThresholdUp = policy.BurstThresholdUp,
                BurstTime = policy.BurstTime,
                MaxLimitDown = policy.MaxLimitDown,
                MaxLimitUp = policy.MaxLimitUp,
                IsActive = policy.IsActive,
                CreatedDate = policy.CreatedDate,
                CreatedBy = policy.CreatedBy,
                ModifiedDate = policy.ModifiedDate,
                ModifiedBy = policy.ModifiedBy
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, MikrotikRadiusPolicyViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            // Check for duplicate policy name (exclude current)
            if (_db.MikrotikRadiusPolicies.Any(p => p.PolicyName == model.PolicyName && p.Id != id))
            {
                ModelState.AddModelError(nameof(model.PolicyName), "Policy name sudah ada.");
                return View(model);
            }

            try
            {
                var policy = _db.MikrotikRadiusPolicies.FirstOrDefault(p => p.Id == id);
                if (policy == null)
                    return NotFound();

                policy.PolicyName = model.PolicyName;
                policy.Description = model.Description;
                policy.DownloadLimit = model.DownloadLimit;
                policy.UploadLimit = model.UploadLimit;
                policy.QueueType = model.QueueType;
                policy.Priority = model.Priority;
                policy.BurstLimitDown = model.BurstLimitDown;
                policy.BurstLimitUp = model.BurstLimitUp;
                policy.BurstThresholdDown = model.BurstThresholdDown;
                policy.BurstThresholdUp = model.BurstThresholdUp;
                policy.BurstTime = model.BurstTime;
                policy.MaxLimitDown = model.MaxLimitDown;
                policy.MaxLimitUp = model.MaxLimitUp;
                policy.IsActive = model.IsActive;
                policy.ModifiedDate = DateTime.UtcNow;
                policy.ModifiedBy = User.Identity?.Name;

                _db.MikrotikRadiusPolicies.Update(policy);
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Radius policy berhasil diperbarui.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Radius policy");
                ModelState.AddModelError("", "Gagal mengupdate data. Silahkan coba lagi.");
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var policy = _db.MikrotikRadiusPolicies.FirstOrDefault(p => p.Id == id);
                if (policy == null)
                    return Json(new { success = false, message = "Policy tidak ditemukan." });

                _db.MikrotikRadiusPolicies.Remove(policy);
                _db.SaveChanges();

                return Json(new { success = true, message = "Radius policy berhasil dihapus." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Radius policy");
                return Json(new { success = false, message = "Gagal menghapus policy." });
            }
        }

        [HttpPost]
        public IActionResult CheckPolicyNameUnique(string policyName, int id = 0)
        {
            var exists = id > 0
                ? _db.MikrotikRadiusPolicies.Any(p => p.PolicyName == policyName && p.Id != id)
                : _db.MikrotikRadiusPolicies.Any(p => p.PolicyName == policyName);

            return Json(new { isUnique = !exists });
        }
    }
}

