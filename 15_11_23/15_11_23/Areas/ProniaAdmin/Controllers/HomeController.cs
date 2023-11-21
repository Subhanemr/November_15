using _15_11_23.DAL;
using Microsoft.AspNetCore.Mvc;

namespace _15_11_23.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        [Area("ProniaAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
