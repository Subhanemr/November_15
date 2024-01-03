using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Exceptions;
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
        public async Task<IActionResult> Index(int page)
        {
            if (page < 0) throw new WrongRequestException("The request sent does not exist");
            double count = await _context.Categories.CountAsync();
            List<Category> categories = await _context.Categories.Skip(page * 2).Take(2)
                .Include(c => c.Products).ToListAsync();

            PaginationVM<Category> paginationVM = new PaginationVM<Category>
            {
                TotalPage = Math.Ceiling(count / 2),
                CurrentPage = page + 1,
                Items = categories
            };
            if (paginationVM.TotalPage < page) throw new NotFoundException("Your request was not found");
            return View(paginationVM);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateCategoryVM create)
        {
            if(!ModelState.IsValid) return View(create);

            bool result = await _context.Categories.AnyAsync(c => c.Name.ToLower().Trim() == create.Name.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Name", "A Category is available");
                return View(create);
            }
            Category category = new Category { Name = create.Name };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) throw new NotFoundException("Your request was not found");
            CreateUpdateCategoryVM categoryVM = new CreateUpdateCategoryVM { Name = category.Name };

            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateCategoryVM update)
        {
            if (!ModelState.IsValid) return View(update);

           Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
           if(existed == null) throw new NotFoundException("Your request was not found");

           bool result = await _context.Categories.AnyAsync(c=> c.Name.ToLower().Trim() == update.Name.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Name", "A Category is available");
                return View(update);
            }

            existed.Name = update.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException("Your request was not found");
            _context.Categories.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Category category = await _context.Categories
                .Include(p => p.Products)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (category == null) throw new NotFoundException("Your request was not found");

            return View(category);
        }
         
    }
}
