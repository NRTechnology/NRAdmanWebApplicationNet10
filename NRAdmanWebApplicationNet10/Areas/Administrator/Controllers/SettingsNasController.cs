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
        public IActionResult GetNasData()
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
    }
}
