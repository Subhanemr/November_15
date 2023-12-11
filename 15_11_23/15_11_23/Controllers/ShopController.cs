using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ShopController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? search, int? order, int? categoryId)
        {
            IQueryable<Product> queryable = _context.Products.Include(pi => pi.ProductImages.Where(a => a.IsPrimary != null)).AsQueryable();
            switch (order)
            {
                case 1:
                    queryable = queryable.OrderBy(p => p.Name);
                    break;
                case 2:
                    queryable = queryable.OrderBy(p => p.Price);
                    break;
                case 3:
                    queryable = queryable.OrderByDescending(p => p.Id);
                    break;
                case 4:
                    queryable = queryable.OrderByDescending(p => p.Price);
                    break;
            }
            if (!string.IsNullOrEmpty(search))
            {
                queryable = queryable.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }
            if (categoryId != null)
            {
                queryable = queryable.Where(p => p.CategoryId == categoryId);
            }
            ShopVM shopVM = new ShopVM
            {
                Categories = await _context.Categories.Include(c => c.Products).ToListAsync(),
                Products = await queryable.ToListAsync(),
                Order = order,
                Search = search,
                CategoryId = categoryId,
            };
            return View(shopVM);
        }
    }
}
