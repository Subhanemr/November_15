using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(int page)
        {
            if (page < 0) throw new WrongRequestException("The request sent does not exist");
            double count = await _context.Colors.CountAsync();
            List<Color> colors = await _context.Colors.Skip(page * 3).Take(3)
                .Include(c => c.ProductColors).ToListAsync();

            PaginationVM<Color> paginationVM = new PaginationVM<Color>
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 3),
                Items = colors
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
            if (!ModelState.IsValid)
            {
                return View(create);
            }

            bool result = await _context.Colors.AnyAsync(c => c.Name.ToLower().Trim() == create.Name.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Name", "A Color with this name already exists");
                return View(create);
            }
            Color color = new Color { Name = create.Name };

            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);

            if (color == null) throw new NotFoundException("Your request was not found");
            CreateUpdateColorVM colorVM = new CreateUpdateColorVM { Name = color.Name };


            return View(colorVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateColorVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Color existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException("Your request was not found");

            bool result = await _context.Colors.AnyAsync(c => c.Name.ToLower().Trim() == update.Name.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Name", "A Color is available");
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
            Color existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException("Your request was not found");
            _context.Colors.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Color color = await _context.Colors
                .Include(p => p.ProductColors)
                .ThenInclude(p => p.Product).ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (color == null) throw new NotFoundException("Your request was not found");

            return View(color);
        }
    }
}
