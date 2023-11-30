using _15_11_23.DAL;
using _15_11_23.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.ViewComponents
{
    public class ProductSwipperViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductSwipperViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> products = await _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).Take(8).ToListAsync();

            return View(products);
        }
    }
}
