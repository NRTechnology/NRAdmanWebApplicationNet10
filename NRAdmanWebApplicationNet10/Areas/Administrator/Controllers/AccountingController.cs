using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator,Accounting")]
    public class AccountingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
