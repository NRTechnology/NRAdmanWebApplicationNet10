using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRAdmanWebApplicationNet10.Services;
using NRAdmanWebApplicationNet10.Models;
using NRAdmanWebApplicationNet10.ViewModels;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class SettingsNasController(ApplicationDbContext applicationDbContext,
        IWebHostEnvironment environment, ILogger<SettingsNasController> logger,
        IConfiguration configuration, ISSHService sshService) : Controller
    {
        public IActionResult Index()
        {
            return View("SettingsNasList");
        }

        [HttpGet]
        public IActionResult IsNasNameUnique(string nasName)
        {
            if (string.IsNullOrWhiteSpace(nasName))
            {
                return Json(new { isUnique = false });
            }

            var exists = applicationDbContext.Nas.Any(n => n.NasName == nasName);
            return Json(new { isUnique = !exists });
        }

        [HttpGet]
        public IActionResult GetJsonResult()
        {
            var data = applicationDbContext.Nas.Select(n => new {
                id = n.Id,
                nasName = n.NasName,
                shortName = n.ShortName,
                type = n.Type,
                ports = n.Ports,
                server = n.Server,
                community = n.Community,
                description = n.Description,                
            }).ToList();

            return Json(data);
        }

        [HttpGet]
        public IActionResult CreateModal()
        {
            var viewModel = new NasViewModel();
            return PartialView("../Shared/_Modals/_ModalCreateNas", viewModel);
        }

        [HttpGet]
        public IActionResult EditModal(int id)
        {
            var nas = applicationDbContext.Nas.FirstOrDefault(n => n.Id == id);
            if (nas == null)
            {
                return NotFound();
            }

            var viewModel = new NasViewModel
            {
                Id = nas.Id,
                NasName = nas.NasName,
                ShortName = nas.ShortName,
                Type = nas.Type,
                Ports = nas.Ports,
                Secret = nas.Secret,
                Server = nas.Server,
                Community = nas.Community,
                Description = nas.Description,                
            };

            return PartialView("../Shared/_Modals/_ModalEditNas", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NasViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var errorList = errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal.", errors = errorList });
            }

            // server-side unique check for NasName
            if (applicationDbContext.Nas.Any(n => n.NasName == model.NasName))
            {
                return Json(new { success = false, message = "NAS Name sudah ada." });
            }

            try
            {
                var entity = new Nas
                {
                    NasName = model.NasName,
                    ShortName = model.ShortName,
                    Type = model.Type,
                    Ports = model.Ports,
                    Secret = model.Secret,
                    Server = model.Server,
                    Community = model.Community,
                    Description = model.Description,                
                };

                applicationDbContext.Nas.Add(entity);
                applicationDbContext.SaveChanges();

                logger.LogInformation("NAS {NasName} berhasil dibuat oleh {AdminUser}", model.NasName, User.Identity?.Name);
                return Json(new { success = true, message = "NAS berhasil ditambahkan." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal menyimpan NAS {NasName}", model.NasName);
                return Json(new { success = false, message = "Gagal menyimpan data NAS. Silakan coba lagi." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, NasViewModel model)
        {
            if (id != model.Id)
            {
                return Json(new { success = false, message = "ID tidak sesuai." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var errorList = errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal.", errors = errorList });
            }

            // server-side unique check for NasName (exclude current record)
            if (applicationDbContext.Nas.Any(n => n.NasName == model.NasName && n.Id != id))
            {
                return Json(new { success = false, message = "NAS Name sudah ada." });
            }

            try
            {
                var entity = applicationDbContext.Nas.FirstOrDefault(n => n.Id == id);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Data NAS tidak ditemukan." });
                }

                entity.NasName = model.NasName;
                entity.ShortName = model.ShortName;
                entity.Type = model.Type;
                entity.Ports = model.Ports;
                entity.Secret = model.Secret;
                entity.Server = model.Server;
                entity.Community = model.Community;
                entity.Description = model.Description;                

                applicationDbContext.Nas.Update(entity);
                applicationDbContext.SaveChanges();

                logger.LogInformation("NAS {NasName} berhasil diperbarui oleh {AdminUser}", model.NasName, User.Identity?.Name);
                return Json(new { success = true, message = "NAS berhasil diperbarui." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal memperbarui NAS {NasId}", id);
                return Json(new { success = false, message = "Gagal memperbarui data NAS. Silakan coba lagi." });
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var entity = applicationDbContext.Nas.FirstOrDefault(n => n.Id == id);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Data NAS tidak ditemukan." });
                }

                applicationDbContext.Nas.Remove(entity);
                applicationDbContext.SaveChanges();

                return Json(new { success = true, message = "NAS berhasil dihapus." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal menghapus NAS");
                return Json(new { success = false, message = "Gagal menghapus data NAS. Silakan coba lagi." });
            }
        }

        [HttpGet]
        public IActionResult ConnectSSHModal(int id, string server, string username)
        {
            var nas = applicationDbContext.Nas.FirstOrDefault(n => n.Id == id);
            if (nas == null)
            {
                return NotFound();
            }

            var viewModel = new SSHConnectionViewModel
            {
                Id = nas.Id,
                NasName = nas.NasName,
                Server = server,
                Username = username,                
            };

            return PartialView("../Shared/_Modals/_ModalConnectSSH", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestSSHConnection(SSHConnectionRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Server) || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return Json(new { success = false, message = "Server, username, dan password tidak boleh kosong." });
                }

                var result = await sshService.TestConnectionAsync(request.Server, request.Port, request.Username, request.Password);

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
        public async Task<IActionResult> ExecuteSSHCommand(SSHCommandRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Server) || string.IsNullOrWhiteSpace(request.Username) || 
                    string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Command))
                {
                    return Json(new { success = false, message = "Semua parameter wajib diisi." });
                }

                var result = await sshService.ExecuteCommandAsync(request.Server, request.Port, request.Username, request.Password, request.Command);

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
