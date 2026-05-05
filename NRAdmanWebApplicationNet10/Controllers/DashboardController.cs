using Microsoft.AspNetCore.Mvc;

namespace NRAdmanWebApplicationNet10.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
