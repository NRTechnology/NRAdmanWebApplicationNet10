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
        ILogger<SettingsRouterController> logger, ISSHService sshService) : Controller
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
                var data = applicationDbContext.NetworkRouters.Select(router => new
                {
                    id = router.Id.ToString(),
                    routerType = router.RouterType.ToString(),
                    name = router.Name,
                    ipAddress = router.IpAddress,
                    username = router.Username,
                    sshport = router.SShPort,
                    apiport = router.ApiPort,
                    description = router.Description,
                    createdDate = router.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
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

            var router = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.IpAddress == ipAddress);
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

            var router = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.Id == routerId);
            if (router == null)
            {
                return NotFound();
            }

            var viewModel = new RouterViewModel
            {
                Id = router.Id,
                RouterType = router.RouterType,
                Name = router.Name,
                IpAddress = router.IpAddress,
                Username = router.Username,
                Password = router.Password,
                SshPorts = router.SShPort,
                ApiPorts = router.ApiPort,
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
            var existingRouter = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.IpAddress == model.IpAddress);
            if (existingRouter != null)
            {
                return Json(new { success = false, message = "IP Address sudah digunakan." });
            }

            try
            {
                var router = new NetworkRouter
                {
                    RouterType = model.RouterType,
                    Name = model.Name,
                    IpAddress = model.IpAddress,
                    Username = model.Username,
                    Password = model.Password,
                    SShPort = model.SshPorts,
                    ApiPort = model.ApiPorts,
                    Description = model.Description,
                    CreatedDate = DateTime.UtcNow
                };

                applicationDbContext.NetworkRouters.Add(router);
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
                var router = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.Id == id);
                if (router == null)
                {
                    return Json(new { success = false, message = "Router tidak ditemukan." });
                }

                // Check if IP Address is unique (exclude current router)
                var existingRouter = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.IpAddress == model.IpAddress && r.Id != id);
                if (existingRouter != null)
                {
                    return Json(new { success = false, message = "IP Address sudah digunakan oleh router lain." });
                }

                router.RouterType = model.RouterType;
                router.Name = model.Name;
                router.IpAddress = model.IpAddress;
                router.Username = model.Username;
                router.SShPort = model.SshPorts;
                router.ApiPort = model.ApiPorts;
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
                var router = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.Id == id);
                if (router == null)
                {
                    return Json(new { success = false, message = "Router tidak ditemukan." });
                }

                applicationDbContext.NetworkRouters.Remove(router);
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


        [HttpGet]
        public IActionResult ConnectSshModal(Guid id)
        {
            var router = applicationDbContext.NetworkRouters.FirstOrDefault(n => n.Id == id);
            if (router == null)
            {
                return NotFound();
            }

            var viewModel = new SshConnectionViewModel
            {
                Id = router.Id,
                RouterName = router.Name,
                IpAddress = router.IpAddress,
                Username = router.Username,
                Port = router.SShPort
            };

            return PartialView("../Shared/_Modals/_ModalConnectSSH", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestSshConnection(Guid id, SshConnectionViewModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.IpAddress) || string.IsNullOrWhiteSpace(request.Username))
                {
                    return Json(new { success = false, message = "Server, username, dan password tidak boleh kosong." });
                }

                var router = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.Id == request.Id);
                if (router == null)
                {
                    return Json(new { success = false, message = "Router tidak ditemukan." });
                }

                var result = await sshService.TestConnectionAsync(router.IpAddress, router.SShPort, request.Username, router.Password);

                return Json(new
                {
                    success = result.IsSuccessful,
                    message = result.Message,
                    connectedAt = result.ConnectedAt
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error testing SSH connection");
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExecuteSshCommand(SshConnectionViewModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.IpAddress) || string.IsNullOrWhiteSpace(request.Username))
                {
                    return Json(new { success = false, message = "Semua parameter wajib diisi." });
                }

                var router = applicationDbContext.NetworkRouters.FirstOrDefault(r => r.Id == request.Id);
                if (router == null)
                {
                    return Json(new { success = false, message = "Router tidak ditemukan." });
                }

                var result = await sshService.ExecuteCommandAsync(router.IpAddress, router.SShPort, request.Username, router.Password, "ls");

                return Json(new
                {
                    success = result.IsSuccessful,
                    message = result.Message,
                    output = result.Output,
                    errorOutput = result.ErrorOutput,
                    exitStatus = result.ExitStatus
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing SSH command");
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}
