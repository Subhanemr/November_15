using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Extendions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

            if (!slide.Photo.ValidateType())
            {
                ModelState.AddModelError("Photo", "File Not supported");
                return View();
            }

            if(!slide.Photo.ValidataSize(10))
            {
                ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                return View();
            }



            slide.ImgUrl = await slide.Photo.CreateFile(_env.WebRootPath, "assets","images", "website-images");


            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);

            if (slide == null) return NotFound();

            return View(slide);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Slide slide)
        {
            if (!ModelState.IsValid) return View();

            Slide existed = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (slide.Photo is null)
            {
                ModelState.AddModelError("Photo", "The image must be uploaded");
                return View(existed);
            }
            if(existed.ImgUrl is not null) 
            {

                if (!slide.Photo.ValidateType())
                {
                    ModelState.AddModelError("Photo", "File Not supported");
                    return View(existed);
                }

                if (!slide.Photo.ValidataSize(10))
                {
                    ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                    return View(existed);
                }
                string newImage = await slide.Photo.CreateFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.ImgUrl.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.ImgUrl = newImage;
            }

            existed.Title = slide.Title;
            existed.SubTitle = slide.SubTitle;
            existed.Description = slide.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slide existed = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);

            if (existed == null) return NotFound();

            existed.ImgUrl.DeleteFile(_env.WebRootPath, "assets","images", "website-images");

            _context.Slides.Remove(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);
            if (slide == null) return NotFound();

            return View(slide);
        }
    }
}
