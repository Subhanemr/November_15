using Microsoft.AspNetCore.Mvc;

namespace _15_11_23.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
