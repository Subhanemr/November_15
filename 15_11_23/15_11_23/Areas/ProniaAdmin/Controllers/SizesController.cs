using Microsoft.AspNetCore.Mvc;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    public class SizesController : Controller
    {
        [Area("ProniaAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
