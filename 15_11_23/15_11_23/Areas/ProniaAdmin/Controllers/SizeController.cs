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
    public class SizeController : Controller
    {
        private readonly AppDbContext _context;

        public SizeController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(int page)
        {
            if (page < 0) throw new WrongRequestException("The request sent does not exist");
            double count = await _context.Sizes.CountAsync();
            List<Size> sizes = await _context.Sizes.Skip(page * 3).Take(3)
                .Include(c => c.ProductSizes).ToListAsync();

            PaginationVM<Size> paginationVM = new PaginationVM<Size>
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 3),
                Item = sizes
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
        public async Task<IActionResult> Create(CreateUpdateSizeVM sizeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(sizeVM);
            }

            bool result = await _context.Sizes.AnyAsync(c => c.Name.ToLower().Trim() == sizeVM.Name.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Color.Name", "A Color with this name already exists");
                return View(sizeVM);
            }
            Size size = new Size { Name = sizeVM.Name };

            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Size size = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);

            if (size == null) throw new NotFoundException("Your request was not found");
            CreateUpdateSizeVM sizeVM = new CreateUpdateSizeVM { Name = size.Name };


            return View(sizeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateUpdateSizeVM sizeVM)
        {
            if (!ModelState.IsValid) return View();

            Size existed = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException("Your request was not found");

            bool result = await _context.Sizes.AnyAsync(c => c.Name.ToLower().Trim() == sizeVM.Name.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Name", "A Size is available");
                return View();
            }

            existed.Name = sizeVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Size existed = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException("Your request was not found");
            _context.Sizes.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Size size = await _context.Sizes
                .Include(p => p.ProductSizes)
                .ThenInclude(p => p.Product).ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (size == null) throw new NotFoundException("Your request was not found");

            return View(size);
        }
    }
}
