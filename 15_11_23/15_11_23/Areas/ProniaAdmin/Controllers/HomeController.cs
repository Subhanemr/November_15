using _15_11_23.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _15_11_23.Areas.Admin.Controllers
{
    [Area("ProniaAdmin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
