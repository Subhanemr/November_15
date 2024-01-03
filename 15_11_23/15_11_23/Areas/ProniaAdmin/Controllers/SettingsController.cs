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
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(int page)
        {
            if (page < 0) throw new WrongRequestException("The request sent does not exist");
            double count = await _context.Settings.CountAsync();
            List<Settings> settings = await _context.Settings.Skip(page *4).Take(4).ToListAsync();

            PaginationVM<Settings> paginationVM = new PaginationVM<Settings>
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 4),
                Items = settings
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
        public async Task<IActionResult> Create(CreateUpdateSettingsVM create)
        {
            if(!ModelState.IsValid) return View(create);

            bool result = await _context.Settings.AnyAsync(c => c.Key.ToLower().Trim() == create.Key.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Key", "A Key with this name already exists");
                return View(create);
            }

            Settings settings = new Settings { Key = create.Key, Value = create.Value };

            await _context.Settings.AddAsync(settings);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) { throw new WrongRequestException("The request sent does not exist"); }

            Settings settings = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (settings == null) { throw new NotFoundException("Your request was not found"); }
            CreateUpdateSettingsVM settingsVM = new CreateUpdateSettingsVM { Key = settings.Key, Value = settings.Value };

            return View(settingsVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateSettingsVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Settings settings = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (settings == null) { throw new NotFoundException("Your request was not found");  }

            bool result = await _context.Settings.AnyAsync(c => c.Key.ToLower().Trim() == update.Key.ToLower().Trim()&& c.Id !=id);

            if (result)
            {
                ModelState.AddModelError("Key", "A Key with this name already exists");
                return View(update);
            }

            settings.Key = update.Key;
            settings.Value = update.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Settings settings = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (settings == null) throw new NotFoundException("Your request was not found");
            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
