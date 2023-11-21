using _15_11_23.DAL;
using _15_11_23.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.Include(c => c.Products).ToListAsync();

            return View(categories);
        }
    }
}
