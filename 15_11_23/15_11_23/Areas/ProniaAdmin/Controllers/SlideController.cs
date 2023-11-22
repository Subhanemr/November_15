using _15_11_23.DAL;
using _15_11_23.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;

        public SlideController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slide = await _context.Slides.ToListAsync();
            return View(slide);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slide slide)
        {
            if(slide.Photo is null)
            {
                ModelState.AddModelError("Photo", "The image must be uploaded");
                return View();
            }

            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "File Not supported");
                return View();
            }

            if(slide.Photo.Length > 3 * 1024 * 1024)
            {
                ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                return View();
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            using(FileStream fs = new  ($@"{currentDirectory}\wwwroot\assets\images\website-images\{slide.Photo.FileName}", FileMode.Create))
            {
                await slide.Photo.CopyToAsync(fs);
            };

            slide.ImgUrl = slide.Photo.FileName;

            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slide existed = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);

            if (existed == null) return NotFound();

            _context.Slides.Remove(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
