using _15_11_23.Areas.ProniaAdmin.ViewModels;
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
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if(slideVM.Photo is null)
            {
                ModelState.AddModelError("Photo", "The image must be uploaded");
                return View(slideVM);
            }

            if (!slideVM.Photo.ValidateType())
            {
                ModelState.AddModelError("Photo", "File Not supported");
                return View(slideVM);
            }

            if(!slideVM.Photo.ValidataSize(10))
            {
                ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                return View(slideVM);
            }

            

            string fileName = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets","images", "website-images");

            Slide slide = new Slide
            {
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                Description = slideVM.Description,
                ImgUrl = fileName
            };


            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);

            if (slide == null) return NotFound();

            UpdateSlideVM slideVM = new UpdateSlideVM 
            {
                Title= slide.Title,
                SubTitle = slide.SubTitle,
                Description = slide.Description,
                ImgUrl = slide.ImgUrl,

            };

            return View(slideVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSlideVM slideVM)
        {
            if (!ModelState.IsValid) return View(slideVM);

            Slide existed = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (slideVM.Photo is not null) 
            {

                if (!slideVM.Photo.ValidateType())
                {
                    ModelState.AddModelError("Photo", "File Not supported");
                    return View(existed);
                }

                if (!slideVM.Photo.ValidataSize(10))
                {
                    ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                    return View(existed);
                }
                string newImage = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.ImgUrl.DeleteFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.ImgUrl = newImage;
            }

            existed.Title = slideVM.Title;
            existed.SubTitle = slideVM.SubTitle;
            existed.Description = slideVM.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slide existed = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);

            if (existed == null) return NotFound();

            existed.ImgUrl.DeleteFileAsync(_env.WebRootPath, "assets","images", "website-images");

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
