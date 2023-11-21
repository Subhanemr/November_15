using Microsoft.AspNetCore.Mvc;

namespace _15_11_23.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("ProniaAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
