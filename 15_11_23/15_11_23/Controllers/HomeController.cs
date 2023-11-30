using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.ModelsVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
            .Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).OrderByDescending(s => s.CountId).Take(8)
            .ToListAsync();

            List<Settings> settings = await _context.Settings.ToListAsync();
            List<Slide> slides = await _context.Slides.OrderBy(s => s.Id).Take(3).ToListAsync();
            List<Client> clients = await _context.Clients.ToListAsync();
            List<Blog> blogs = await _context.Blogs.ToListAsync();

            HomeVM vm = new HomeVM { Slides = slides, Products = products, Clients = clients, Blogs = blogs, Settings= settings };

            return View(vm);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}