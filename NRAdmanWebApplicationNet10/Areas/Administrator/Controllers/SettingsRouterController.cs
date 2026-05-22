using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRAdmanWebApplicationNet10.Models;
using NRAdmanWebApplicationNet10.ViewModels;
using NRAdmanWebApplicationNet10.Services;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class SettingsRouterController(
        ApplicationDbContext applicationDbContext,
        ILogger<SettingsRouterController> logger) : Controller
    {
        public IActionResult Index()
        {
            return View("SettingsRouterList");
        }

        [HttpGet]
        public IActionResult GetJsonResult()
        {
            try
            {
                var data = applicationDbContext.Routers.Select(r => new
                {
                    id = r.Id.ToString(),
                    routerType = r.RouterType.ToString(),
                    ipAddress = r.IpAddress,
                    username = r.Username,
                    ports = r.Ports,
                    description = r.Description,
                    createdDate = r.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToList();

                return Json(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving routers list");
                return Json(new { success = false, message = "Gagal mengambil data router." });
            }
        }

        [HttpGet]
        public IActionResult IsIpAddressUnique(string ipAddress, string excludeId = "")
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return Json(new { isUnique = false });
            }

            var router = applicationDbContext.Routers.FirstOrDefault(r => r.IpAddress == ipAddress);
            if (router != null && !string.IsNullOrEmpty(excludeId) && router.Id.ToString() == excludeId)
            {
                return Json(new { isUnique = true });
            }

            return Json(new { isUnique = router == null });
        }

        [HttpGet]
        public IActionResult CreateModal()
        {
            var viewModel = new RouterViewModel();
            return PartialView("../Shared/_Modals/_ModalCreateRouter", viewModel);
        }

        [HttpGet]
        public IActionResult EditModal(string id)
        {
            if (!Guid.TryParse(id, out var routerId))
            {
                return NotFound();
            }

            var router = applicationDbContext.Routers.FirstOrDefault(r => r.Id == routerId);
            if (router == null)
            {
                return NotFound();
            }

            var viewModel = new RouterViewModel
            {
                Id = router.Id,
                RouterType = router.RouterType,
                IpAddress = router.IpAddress,
                Username = router.Username,
                Password = router.Password,
                Ports = router.Ports,
                Description = router.Description
            };

            return PartialView("../Shared/_Modals/_ModalEditRouter", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RouterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Validasi gagal.", errors = errors });
            }

            // Server-side unique check for IP Address
            var existingRouter = applicationDbContext.Routers.FirstOrDefault(r => r.IpAddress == model.IpAddress);
            if (existingRouter != null)
            {
                return Json(new { success = false, message = "IP Address sudah digunakan." });
            }

            try
            {
                var router = new Router
                {
                    RouterType = model.RouterType,
                    IpAddress = model.IpAddress,
                    Username = model.Username,
                    Password = model.Password,
                    Ports = model.Ports,
                    Description = model.Description,
                    CreatedDate = DateTime.UtcNow
                };

                applicationDbContext.Routers.Add(router);
                applicationDbContext.SaveChanges();

                logger.LogInformation("Router {IpAddress} berhasil ditambahkan oleh {AdminUser}", model.IpAddress, User.Identity?.Name);
                return Json(new { success = true, message = "Router berhasil ditambahkan." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal membuat router {IpAddress}", model.IpAddress);
                return Json(new { success = false, message = "Gagal menyimpan data router. Silakan coba lagi." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Guid id, RouterViewModel model)
        {
            // Remove password validation if password is empty (optional on edit)
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.Remove("Password");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Validasi gagal.", errors = errors });
            }

            try
            {
                var router = applicationDbContext.Routers.FirstOrDefault(r => r.Id == id);
                if (router == null)
                {
                    return Json(new { success = false, message = "Router tidak ditemukan." });
                }

                // Check if IP Address is unique (exclude current router)
                var existingRouter = applicationDbContext.Routers.FirstOrDefault(r => r.IpAddress == model.IpAddress && r.Id != id);
                if (existingRouter != null)
                {
                    return Json(new { success = false, message = "IP Address sudah digunakan oleh router lain." });
                }

                router.RouterType = model.RouterType;
                router.IpAddress = model.IpAddress;
                router.Username = model.Username;
                router.Ports = model.Ports;
                router.Description = model.Description;
                router.LastModifiedDate = DateTime.UtcNow;

                // Update password if provided
                if (!string.IsNullOrEmpty(model.Password))
                {
                    router.Password = model.Password;
                }

                applicationDbContext.SaveChanges();

                logger.LogInformation("Router {IpAddress} berhasil diperbarui oleh {AdminUser}", model.IpAddress, User.Identity?.Name);
                return Json(new { success = true, message = "Router berhasil diperbarui." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal mengupdate router {Id}", id);
                return Json(new { success = false, message = "Gagal memperbarui data router. Silakan coba lagi." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { success = false, message = "ID router tidak valid." });
            }

            try
            {
                var router = applicationDbContext.Routers.FirstOrDefault(r => r.Id == id);
                if (router == null)
                {
                    return Json(new { success = false, message = "Router tidak ditemukan." });
                }

                applicationDbContext.Routers.Remove(router);
                applicationDbContext.SaveChanges();

                logger.LogInformation("Router {IpAddress} berhasil dihapus oleh {AdminUser}", router.IpAddress, User.Identity?.Name);
                return Json(new { success = true, message = "Router berhasil dihapus." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal menghapus router {Id}", id);
                return Json(new { success = false, message = "Gagal menghapus data router. Silakan coba lagi." });
            }
        }
    }
}
