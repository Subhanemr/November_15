using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
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
        public readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            List<Settings> settings = await _context.Settings.ToListAsync();
            return View(settings);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateSettingsVM settingsVM)
        {
            if(!ModelState.IsValid) return View(settingsVM);

            bool result = await _context.Settings.AnyAsync(c => c.Key.ToLower().Trim() == settingsVM.Key.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Key", "A Key with this name already exists");
                return View(settingsVM);
            }

            Settings settings = new Settings { Key = settingsVM.Key, Value = settingsVM.Value };

            await _context.Settings.AddAsync(settings);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) { return BadRequest(); }

            Settings settings = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (settings == null) { return NotFound(); }
            CreateUpdateSettingsVM settingsVM = new CreateUpdateSettingsVM { Key = settings.Key, Value = settings.Value };

            return View(settingsVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateSettingsVM settingsVM)
        {
            if (!ModelState.IsValid) return View(settingsVM);

            Settings settings = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (settings == null) { return NotFound();  }

            bool result = await _context.Settings.AnyAsync(c => c.Key.ToLower().Trim() == settingsVM.Key.ToLower().Trim()&& c.Id !=id);

            if (result)
            {
                ModelState.AddModelError("Key", "A Key with this name already exists");
                return View(settingsVM);
            }

            settings.Key = settingsVM.Key;
            settings.Value = settingsVM.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Settings settings = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (settings == null) return NotFound();
            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
