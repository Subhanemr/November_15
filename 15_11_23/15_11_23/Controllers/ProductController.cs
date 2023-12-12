using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Exceptions;
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

        public async Task<IActionResult> Detail(int id)
        {
            if (id == 0) throw new WrongRequestException("The request sent does not exist");
            Product product = await _context.Products
                .Include(p => p.Category)
                .Include(pi => pi.ProductImages)
                .Include(pt => pt.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(ps => ps.ProductSizes).ThenInclude(ps => ps.Size)
                .Include(pc => pc.ProductColors).ThenInclude(pc => pc.Color)
                .FirstOrDefaultAsync(p => p.Id == id);  

            if (product == null) throw new NotFoundException("Your request was not found");

            List<Product> products = await _context.Products
                .Include(pi => pi.ProductImages.Where(pi => pi.IsPrimary != null))
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .ToListAsync();



            ProductVM vm = new ProductVM { Product = product, Products = products };

            return View(vm);
        }
    }
}