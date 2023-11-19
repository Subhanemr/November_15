using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return BadRequest();
            Product product = _context.Products.Include(p => p.Category).Include(p => p.ProductImages).FirstOrDefault(p => p.Id == id);
            List<Product> products = _context.Products.Include(p => p.Category).Include(p => p.ProductImages).Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id).ToList();

            if (product == null) return NotFound();

            ProductVM vm = new ProductVM { Product = product, Products = products };

            return View(vm);
        }
    }
}