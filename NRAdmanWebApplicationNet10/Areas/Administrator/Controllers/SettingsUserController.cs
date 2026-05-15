using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRAdmanWebApplicationNet10.Services;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class SettingsUserController(ApplicationDbContext applicationDbContext,
        IWebHostEnvironment environment, ILogger<SettingsNasController> logger,
        IConfiguration configuration) : Controller
    {
        public IActionResult Index()
        {
            return View("SettingsUserList");
        }
    }
}
