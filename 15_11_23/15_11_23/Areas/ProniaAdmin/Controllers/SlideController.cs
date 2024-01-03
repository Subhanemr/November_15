using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Exceptions;
using _15_11_23.Utilities.Extendions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            List<Slide> slide = await _context.Slides.ToListAsync();
            return View(slide);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM create)
        {
            if (!ModelState.IsValid) return View(create);

            if (create.Photo is null)
            {
                ModelState.AddModelError("Photo", "The image must be uploaded");
                return View(create);
            }

            if (!create.Photo.ValidateType())
            {
                ModelState.AddModelError("Photo", "File Not supported");
                return View(create);
            }

            if(!create.Photo.ValidataSize(10))
            {
                ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                return View(create);
            }

            string fileName = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets","images", "website-images");

            Slide slide = new Slide
            {
                Title = create.Title,
                SubTitle = create.SubTitle,
                Description = create.Description,
                ImgUrl = fileName
            };


            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Slide slide = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);

            if (slide == null) throw new NotFoundException("Your request was not found");

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
        public async Task<IActionResult> Update(int id, UpdateSlideVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Slide existed = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException("Your request was not found");
            if (update.Photo is not null) 
            {

                if (!update.Photo.ValidateType())
                {
                    ModelState.AddModelError("Photo", "File Not supported");
                    return View(existed);
                }

                if (!update.Photo.ValidataSize(10))
                {
                    ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                    return View(existed);
                }
                string newImage = await update.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.ImgUrl.DeleteFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.ImgUrl = newImage;
            }

            existed.Title = update.Title;
            existed.SubTitle = update.SubTitle;
            existed.Description = update.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");

            Slide existed = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);

            if (existed == null) throw new NotFoundException("Your request was not found");

            existed.ImgUrl.DeleteFileAsync(_env.WebRootPath, "assets","images", "website-images");

            _context.Slides.Remove(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Slide slide = await _context.Slides.FirstOrDefaultAsync(c => c.Id == id);
            if (slide == null) throw new NotFoundException("Your request was not found");

            return View(slide);
        }
    }
}
