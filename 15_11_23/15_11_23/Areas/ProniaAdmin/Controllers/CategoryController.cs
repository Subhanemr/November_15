using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.Include(c => c.Products).ToListAsync();

            return View(categories);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateCategoryVM categoryVM)
        {
            if(!ModelState.IsValid) return View(categoryVM);

            bool result = await _context.Categories.AnyAsync(c => c.Name.ToLower().Trim() == categoryVM.Name.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Name", "A Category is available");
                return View(categoryVM);
            }
            Category category = new Category { Name = categoryVM.Name };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();
            CreateUpdateCategoryVM categoryVM = new CreateUpdateCategoryVM { Name = category.Name };

            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View(categoryVM);

           Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
           if(existed == null) return NotFound();

           bool result = await _context.Categories.AnyAsync(c=> c.Name.ToLower().Trim() == categoryVM.Name.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Name", "A Category is available");
                return View(categoryVM);
            }

            existed.Name = categoryVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Categories.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) return BadRequest();
            Category category = await _context.Categories
                .Include(p => p.Products)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }
         
    }
}
