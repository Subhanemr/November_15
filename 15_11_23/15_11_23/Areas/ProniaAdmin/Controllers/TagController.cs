using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(int page)
        {
            if(page < 0) throw new WrongRequestException("The request sent does not exist");
            double count = await _context.Tags.CountAsync();
            List<Tag> tags = await _context.Tags.Skip(page * 3).Take(3)
                .Include(c => c.ProductTags).ToListAsync();
            PaginationVM<Tag> paginationVM = new PaginationVM<Tag>
            {
                TotalPage = Math.Ceiling(count / 3),
                CurrentPage = page + 1,
                Item = tags
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
        public async Task<IActionResult> Create(CreateUpdateTagVM tagVM)
        {
            if (!ModelState.IsValid)
            {
                return View(tagVM);
            }

            bool result = await _context.Tags.AnyAsync(c => c.Name.ToLower().Trim() == tagVM.Name.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Color.Name", "A Color with this name already exists");
                return View(tagVM);
            }
            Tag tag = new Tag { Name = tagVM.Name };

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Tag tag = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);

            if (tag == null) throw new NotFoundException("Your request was not found");
            CreateUpdateTagVM tagVM = new CreateUpdateTagVM { Name = tag.Name };


            return View(tagVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateTagVM tagVM)
        {
            if (!ModelState.IsValid) return View(tagVM);

            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException("Your request was not found");

            bool result = await _context.Tags.AnyAsync(c => c.Name.ToLower().Trim() == tagVM.Name.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Name", "A Tag is available");
                return View(tagVM);
            }

            existed.Name = tagVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException("Your request was not found");
            _context.Tags.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Tag tag = await _context.Tags
                .Include(p => p.ProductTags)
                .ThenInclude(p => p.Product).ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (tag == null) throw new NotFoundException("Your request was not found");

            return View(tag);
        }
    }
}
