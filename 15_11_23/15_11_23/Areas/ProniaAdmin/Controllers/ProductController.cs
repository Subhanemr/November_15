using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Extendions;
using _15_11_23.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
                .ToListAsync();
            return View(product);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(productVM);
            }
            bool result = await _context.Products.AnyAsync(c => c.Name.ToLower().Trim() == productVM.Name.ToLower().Trim());
            if (result)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("Name", "A Name is available");
                return View(productVM);
            };
            bool resultOrder = await _context.Products.AnyAsync(c => c.CountId == productVM.CountId);
            if (resultOrder)
            {
                ModelState.AddModelError("Order", "A Order is available");
                return View(productVM);
            }

            bool resultCategory = await _context.Categories.AnyAsync(c => c.Id == productVM.CategoryId);
            if (!resultCategory)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("CategoryId", "This id has no category");
                return View(productVM);
            }
            if (productVM.Price <= 0)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("Price", "Price cannot be 0");
                return View(productVM);
            };


            Product product = new Product
            {
                Name = productVM.Name,
                Price = productVM.Price,
                Description = productVM.Description,
                SKU = productVM.SKU,
                CategoryId = (int)productVM.CategoryId,
                CountId = productVM.CountId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Product existed = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);

            if (existed == null) return NotFound();

            _context.Products.Remove(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            if (id <= 0) { return BadRequest(); }
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            UpdateProductVM productVM = new UpdateProductVM
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                SKU = product.SKU,
                CategoryId = (int)product.CategoryId,
                CountId = product.CountId
            };
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(productVM);
            }
            Product existed = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            existed.Name = productVM.Name;
            existed.Price = productVM.Price;
            existed.Description = productVM.Description;
            existed.CategoryId = (int)productVM.CategoryId;
            existed.SKU = productVM.SKU;
            existed.CountId = productVM.CountId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) return BadRequest();
            Product product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductTags).ThenInclude(p => p.Tag)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

    }
}
