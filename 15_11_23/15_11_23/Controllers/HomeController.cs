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
            
            List<Slide> slides = _context.Slides.OrderBy(s => s.Id).Take(3).ToList();
            List<Client> clients =  _context.Clients.ToList();
            List<Blog> blogs =  _context.Blogs.ToList();

            HomeVM vm = new HomeVM { Slides = slides, Products = products, Clients = clients, Blogs = blogs };

            return View(vm);
        }

        public async Task<IActionResult> About()
        {
            return View();
        }
    }
}