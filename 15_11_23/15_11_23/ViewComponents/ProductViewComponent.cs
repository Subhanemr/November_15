using _15_11_23.DAL;
using _15_11_23.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int key = 1)
        {
            List<Product> products;
            

            switch (key)
            {
                case 1:
                    products = await _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).Take(8).ToListAsync();
                    break;
                case 2:
                    products = await _context.Products.OrderByDescending(p => p.Price).Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).Take(8).ToListAsync();
                    break;
                case 3:
                    products = await _context.Products.OrderByDescending(p => p.CountId).Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).Take(8).ToListAsync();
                    break;
                default:
                    products = await _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).Take(8).ToListAsync();
                    break;
            }
            return View(products);
        }
    }
}
