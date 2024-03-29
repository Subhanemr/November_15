﻿using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Exceptions;
using _15_11_23.Utilities.Extendions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            List<Blog> blog = await _context.Blogs.ToListAsync();
            return View(blog);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM create)
        {
            if (!ModelState.IsValid) return View(create);
            if (create.Photo == null)
            {
                ModelState.AddModelError("Photo", "The image must be uploaded");
                return View(create);
            }
            if (!create.Photo.ValidateType())
            {
                ModelState.AddModelError("Photo", "File Not supported");
                return View(create);
            }
            if (!create.Photo.ValidataSize(10))
            {
                ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                return View(create);
            }
            string fileName = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");

            Blog blog = new Blog
            {
                Title = create.Title,
                Description = create.Description,
                DateTime = DateTime.Now,
                ByWho = create.ByWho,
                ImgUrl = fileName
            };

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) { throw new WrongRequestException("The request sent does not exist"); }
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) { throw new NotFoundException("Your request was not found"); }

            UpdateBlogVM blogVM = new UpdateBlogVM
            {
                Title = blog.Title,
                Description = blog.Description,
                DateTime = blog.DateTime,
                ByWho = blog.ByWho,
                ImgUrl = blog.ImgUrl
            };

            return View(blogVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateBlogVM update)
        {
            if (!ModelState.IsValid) return View(update);
            Blog existed = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) { throw new NotFoundException("Your request was not found"); };

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

                string fileName = await update.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.ImgUrl.DeleteFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.ImgUrl = fileName;
            }

            existed.Title = update.Title;
            existed.Description = update.Description;
            existed.DateTime = update.DateTime;
            existed.ByWho = update.ByWho;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);
            if (blog == null) { throw new NotFoundException("Your request was not found"); };

            blog.ImgUrl.DeleteFileAsync(_env.WebRootPath, "assets", "images", "website-images");
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) { throw new NotFoundException("Your request was not found"); };
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);
            if (blog == null) { throw new NotFoundException("Your request was not found"); };

            return View(blog);
        }
    }
}
