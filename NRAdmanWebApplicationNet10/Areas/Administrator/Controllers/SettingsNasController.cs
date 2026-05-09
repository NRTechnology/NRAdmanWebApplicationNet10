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
        public IActionResult Create()
        {
            // return empty ViewModel to the view
            return View(new NRAdmanWebApplicationNet10.ViewModels.Nas());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NRAdmanWebApplicationNet10.ViewModels.Nas model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // server-side unique check for NasName
            if (applicationDbContext.Nas.Any(n => n.NasName == model.NasName))
            {
                ModelState.AddModelError(nameof(model.NasName), "NAS Name sudah ada.");
                return View(model);
            }

            try
            {
                // map ViewModel to Model
                var entity = new NRAdmanWebApplicationNet10.Models.Nas
                {
                    NasName = model.NasName,
                    ShortName = model.ShortName,
                    Type = model.Type,
                    Ports = model.Ports,
                    Secret = model.Secret,
                    Server = model.Server,
                    Community = model.Community,
                    Description = model.Description,
                    RouterType = model.RouterType,
                    Username = model.Username,
                    Password = model.Password
                };

                applicationDbContext.Nas.Add(entity);
                applicationDbContext.SaveChanges();
                TempData["SuccessMessage"] = "NAS berhasil ditambahkan.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // log and show error
                logger.LogError(ex, "Gagal menyimpan NAS");
                ModelState.AddModelError(string.Empty, "Gagal menyimpan data NAS. Periksa kembali input dan coba lagi.");
                return View(model);
            }
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var nas = applicationDbContext.Nas.FirstOrDefault(n => n.Id == id);
            if (nas == null)
            {
                return NotFound();
            }

            // map Model to ViewModel
            var viewModel = new NRAdmanWebApplicationNet10.ViewModels.Nas
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
                RouterType = nas.RouterType,
                Username = nas.Username,
                Password = nas.Password
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, NRAdmanWebApplicationNet10.ViewModels.Nas model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // server-side unique check for NasName (exclude current record)
            if (applicationDbContext.Nas.Any(n => n.NasName == model.NasName && n.Id != id))
            {
                ModelState.AddModelError(nameof(model.NasName), "NAS Name sudah ada.");
                return View(model);
            }

            try
            {
                var entity = applicationDbContext.Nas.FirstOrDefault(n => n.Id == id);
                if (entity == null)
                {
                    return NotFound();
                }

                // map ViewModel to Model
                entity.NasName = model.NasName;
                entity.ShortName = model.ShortName;
                entity.Type = model.Type;
                entity.Ports = model.Ports;
                entity.Secret = model.Secret;
                entity.Server = model.Server;
                entity.Community = model.Community;
                entity.Description = model.Description;
                entity.RouterType = model.RouterType;
                entity.Username = model.Username;
                entity.Password = model.Password;

                applicationDbContext.Nas.Update(entity);
                applicationDbContext.SaveChanges();
                TempData["SuccessMessage"] = "NAS berhasil diperbarui.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal memperbarui NAS");
                ModelState.AddModelError(string.Empty, "Gagal memperbarui data NAS. Periksa kembali input dan coba lagi.");
                return View(model);
            }
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
    }
}
