using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRAdmanWebApplicationNet10.Services;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class SettingsNasController(ApplicationDbContext applicationDbContext,
        IWebHostEnvironment environment, ILogger<SettingsNasController> logger,
        IConfiguration configuration) : Controller
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
                routerType = n.RouterType.ToString(),
                username = n.Username
            }).ToList();

            return Json(data);
        }

        [HttpPost]
        public IActionResult CheckNasNameUnique(string nasName, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(nasName))
            {
                return Json(new { isUnique = false });
            }

            // exclude current record if editing
            var exists = id > 0 
                ? applicationDbContext.Nas.Any(n => n.NasName == nasName && n.Id != id)
                : applicationDbContext.Nas.Any(n => n.NasName == nasName);

            return Json(new { isUnique = !exists });
        }

        [HttpPost]
        public IActionResult DeleteNas(int id)
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
    }
}
