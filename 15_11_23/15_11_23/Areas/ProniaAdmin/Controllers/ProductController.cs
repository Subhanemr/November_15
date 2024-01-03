using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Exceptions;
using _15_11_23.Utilities.Extendions;
using _15_11_23.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        private void CreatePopulateDropdowns(CreateProductVM create)
        {
            create.Tags = _context.Tags.ToList();
            create.Categories = _context.Categories.ToList();
            create.Colors = _context.Colors.ToList();
            create.Sizes = _context.Sizes.ToList();
        }
        private void UpdatePopulateDropdowns(UpdateProductVM update)
        {
            update.Tags = _context.Tags.ToList();
            update.Categories = _context.Categories.ToList();
            update.Colors = _context.Colors.ToList();
            update.Sizes = _context.Sizes.ToList();
        }

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(int page)
        {
            if (page < 0) throw new WrongRequestException("The request sent does not exist");
            double count = await _context.Products.CountAsync();
            List<Product> product = await _context.Products.Skip(page*3).Take(3)
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
                .ToListAsync();

            PaginationVM<Product> paginationVM = new PaginationVM<Product>
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 3),
                Items = product
            };
            if (paginationVM.TotalPage < page) throw new NotFoundException("Your request was not found");

            return View(paginationVM);
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create()
        {
            CreateProductVM productVM = new CreateProductVM { 
            Categories = await _context.Categories.ToListAsync(),
            Tags = await _context.Tags.ToListAsync(),
            Sizes = await _context.Sizes.ToListAsync(),
            Colors = await _context.Colors.ToListAsync()
            };
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM create)
        {
            if (!ModelState.IsValid)
            {
                CreatePopulateDropdowns(create);
                return View(create);
            }
            bool result = await _context.Products.AnyAsync(c => c.Name.ToLower().Trim() == create.Name.ToLower().Trim());
            if (result)
            {
                CreatePopulateDropdowns(create);
                ModelState.AddModelError("Name", "A Name is available");
                return View(create);
            };
            bool resultOrder = await _context.Products.AnyAsync(c => c.CountId == create.CountId);
            if (resultOrder)
            {
                CreatePopulateDropdowns(create);
                ModelState.AddModelError("Order", "A Order is available");
                return View(create);
            }

            bool resultCategory = await _context.Categories.AnyAsync(c => c.Id == create.CategoryId);
            if (!resultCategory)
            {
                CreatePopulateDropdowns(create);
                ModelState.AddModelError("CategoryId", "This id has no category");
                return View(create);
            }
            if (create.Price <= 0)
            {
                CreatePopulateDropdowns(create);
                ModelState.AddModelError("Price", "Price cannot be 0");
                return View(create);
            };

            foreach (int tagId in create.TagIds)
            {
                bool tagResult = await _context.Tags.AnyAsync(t => t.Id == tagId);
                if (!tagResult)
                {
                    CreatePopulateDropdowns(create);
                    return View(create);
                }
            }
            foreach (int sizeId in create.SizeIds)
            {
                bool sizeResult = await _context.Sizes.AnyAsync(t => t.Id == sizeId);
                if (!sizeResult)
                {
                    CreatePopulateDropdowns(create);
                    return View(create);
                }
            }
            foreach (int colorId in create.ColorIds)
            {
                bool colorResult = await _context.Colors.AnyAsync(t => t.Id == colorId);
                if (!colorResult)
                {
                    CreatePopulateDropdowns(create);
                    return View(create);
                }
            }
            if (!create.MainPhoto.ValidateType())
            {
                CreatePopulateDropdowns(create);
                ModelState.AddModelError("Photo", "File Not supported");
                return View(create);
            }

            if (!create.MainPhoto.ValidataSize(10))
            {
                CreatePopulateDropdowns(create);
                ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                return View(create);
            }

            if (!create.HoverPhoto.ValidateType())
            {
                CreatePopulateDropdowns(create);
                ModelState.AddModelError("Photo", "File Not supported");
                return View(create);
            }

            if (!create.HoverPhoto.ValidataSize(10))
            {
                CreatePopulateDropdowns(create);
                ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                return View(create);
            }


            ProductImage mainImage = new ProductImage { 
            IsPrimary = true,
            Alternative = create.Name,
            Url = await create.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images")
            };

            ProductImage hoverImage = new ProductImage
            {
                IsPrimary = false,
                Alternative = create.Name,
                Url = await create.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images")
            };

            Product product = new Product
            {
                Name = create.Name,
                Price = create.Price,
                Description = create.Description,
                SKU = create.SKU,
                CategoryId = (int)create.CategoryId,
                CountId = create.CountId,
                ProductTags = create.TagIds.Select(tagId => new ProductTag { TagId = tagId }).ToList(),
                ProductSizes = create.SizeIds.Select(sizeId => new ProductSize { SizeId = sizeId }).ToList(),
                ProductColors = create.ColorIds.Select(colorId => new ProductColor { ColorId = colorId }).ToList(),
                ProductImages = new List<ProductImage> { mainImage , hoverImage}
            };

            TempData["Message"] = "";

            foreach(IFormFile photo in create.Photos)
            {
                if (!photo.ValidateType())
                {
                    TempData["Message"] += $"<p class=\"text-danger\">{photo.Name} type is not suitable</p>";
                    continue;
                }

                if (!photo.ValidataSize(10))
                {
                    TempData["Message"] += $"<p class=\"text-danger\">{photo.Name} the size is not suitable</p>";
                    continue;
                }

                product.ProductImages.Add(new ProductImage
                {
                    IsPrimary = null,
                    Alternative = create.Name,
                    Url = await photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images")
                });
            }


            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");

            Product existed = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductColors)
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existed == null) throw new NotFoundException("Your request was not found");

            List<ProductColor> productColors = await _context.ProductColors.Where(pc => pc.ProductId == id).ToListAsync();
            _context.ProductColors.RemoveRange(productColors);

            List<ProductSize> productSizes = await _context.ProductSizes.Where(pc => pc.ProductId == id).ToListAsync();
            _context.ProductSizes.RemoveRange(productSizes);

            List<ProductTag> productTags = await _context.ProductTags.Where(pc => pc.ProductId == id).ToListAsync();
            _context.ProductTags.RemoveRange(productTags);

            foreach(ProductImage image in existed.ProductImages)
            {
                image.Url.DeleteFileAsync(_env.WebRootPath ,"assets", "images", "website-images");
            }
            List<ProductImage> productImages = await _context.ProductImages.Where(pc => pc.ProductId == id).ToListAsync();
            _context.ProductImages.RemoveRange(productImages);

            _context.Products.Remove(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) { throw new WrongRequestException("The request sent does not exist"); }
            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductColors)
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) throw new NotFoundException("Your request was not found");
            UpdateProductVM productVM = new UpdateProductVM 
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CountId = product.CountId,
                ProductImages = product.ProductImages,
                CategoryId = (int)product.CategoryId,
                SKU = product.SKU,
                Categories = await _context.Categories.ToListAsync(),
                TagIds = product.ProductTags.Select(p => p.TagId).ToList(),
                ColorIds = product.ProductColors.Select(p => p.ColorId).ToList(),
                SizeIds = product.ProductSizes.Select(p => p.SizeId).ToList(),
                Tags = await _context.Tags.ToListAsync(),
                Sizes = await _context.Sizes.ToListAsync(),
                Colors = await _context.Colors.ToListAsync()
            };
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProductVM update)
        {
            Product existed = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductSizes)
                .Include(p => p.ProductColors)
                .FirstOrDefaultAsync(c => c.Id == id);

            update.ProductImages = existed.ProductImages;
            if (!ModelState.IsValid)
            {
                UpdatePopulateDropdowns(update);
                return View(update);
            }
           
            if (existed == null) throw new NotFoundException("Your request was not found");
            bool resultCategory = await _context.Categories.AnyAsync(c => c.Id == update.CategoryId);
            if (!resultCategory)
            {
                UpdatePopulateDropdowns(update);
                ModelState.AddModelError("CategoryId", "This id has no category");
                return View(update);
            }

            bool resultTag = await _context.Tags.AnyAsync(pt => update.TagIds.Contains(pt.Id));
            if (!resultTag)
            {
                UpdatePopulateDropdowns(update);
                ModelState.AddModelError("TagId", "This id has no tag");
                return View(update);
            }
            List<ProductTag> tagsToRemove = existed.ProductTags
                .Where(pt => !update.TagIds.Contains(pt.TagId))
                .ToList();
            _context.ProductTags.RemoveRange(tagsToRemove);

            List<ProductTag> tagsToAdd = update.TagIds
                .Except(existed.ProductTags.Select(pt => pt.TagId))
                .Select(tagId => new ProductTag { TagId = tagId })
                .ToList();
            existed.ProductTags.AddRange(tagsToAdd);

            bool resultColor = await _context.Colors.AnyAsync(pc => update.ColorIds.Contains(pc.Id));
            if (!resultColor)
            {
                UpdatePopulateDropdowns(update);
                ModelState.AddModelError("ColorId", "This id has no color");
                return View(update);
            }

            List<ProductColor> colorToRemove = existed.ProductColors
                .Where(pc => !update.ColorIds.Contains(pc.ColorId))
                .ToList();
            _context.ProductColors.RemoveRange(colorToRemove);

            List<ProductColor> colorToAdd = update.ColorIds
                .Except(existed.ProductColors.Select(pc => pc.ColorId))
                .Select(colorId => new ProductColor { ColorId = colorId })
                .ToList();
            existed.ProductColors.AddRange(colorToAdd);

            bool resultSize = await _context.Sizes.AnyAsync(ps => update.SizeIds.Contains(ps.Id));
            if (!resultSize)
            {
                UpdatePopulateDropdowns(update);
                ModelState.AddModelError("SizeId", "This id has no size");
                return View(update);
            }

            List<ProductSize> sizeToRemove = existed.ProductSizes
                .Where(ps => !update.SizeIds.Contains(ps.SizeId))
                .ToList();
            _context.ProductSizes.RemoveRange(sizeToRemove);

            List<ProductSize> sizeToAdd = update.SizeIds
                .Except(existed.ProductSizes.Select(ps => ps.SizeId))
                .Select(sizeId => new ProductSize { SizeId = sizeId })
                .ToList();
            existed.ProductSizes.AddRange(sizeToAdd);


            if(update.MainPhoto is not null)
            {
                if (!update.MainPhoto.ValidateType())
                {
                    UpdatePopulateDropdowns(update);
                    ModelState.AddModelError("Photo", "File Not supported");
                    return View(update);
                }

                if (!update.MainPhoto.ValidataSize(10))
                {
                    UpdatePopulateDropdowns(update);
                    ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                    return View(update);
                }
            }

            if(update.HoverPhoto is not null)
            {
                if (!update.HoverPhoto.ValidateType())
                {
                    UpdatePopulateDropdowns(update);
                    ModelState.AddModelError("Photo", "File Not supported");
                    return View(update);
                }

                if (!update.HoverPhoto.ValidataSize(10))
                {
                    UpdatePopulateDropdowns(update);
                    ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                    return View(update);
                }
            }

            if(update.MainPhoto is not null)
            {
                string fileName = await update.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                ProductImage prMain = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == true);
                prMain.Url.DeleteFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                _context.ProductImages.Remove(prMain);

                existed.ProductImages.Add(new ProductImage
                {
                    Alternative = update.Name,
                    IsPrimary = true,
                    Url = fileName
                });
            }
            if (update.HoverPhoto is not null)
            {
                string fileName = await update.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                ProductImage prHover = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == false);
                prHover.Url.DeleteFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                _context.ProductImages.Remove(prHover);

                existed.ProductImages.Add(new ProductImage
                {
                    Alternative = update.Name,
                    IsPrimary = false,
                    Url = fileName
                });
            }

            if (existed.ProductImages is null) existed.ProductImages = new List<ProductImage>();

            if (update.ImageIds is null) update.ImageIds = new List<int>();


            List<ProductImage> remove = existed.ProductImages.Where(pi => pi.IsPrimary == null && !update.ImageIds.Exists(imgId => imgId == pi.id)).ToList();
            foreach (ProductImage image in remove)
            {
                image.Url.DeleteFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.ProductImages.Remove(image);
            }

            if (update.ImageIds is null) update.ImageIds = new List<int>();
            
            TempData["Message"] = "";

            if(update.Photos is not null)
            {
                foreach (IFormFile photo in update.Photos)
                {
                    if (!photo.ValidateType())
                    {
                        TempData["Message"] += $"<p class=\"text-danger\">{photo.Name} type is not suitable</p>";
                        continue;
                    }

                    if (!photo.ValidataSize(10))
                    {
                        TempData["Message"] += $"<p class=\"text-danger\">{photo.Name} the size is not suitable</p>";
                        continue;
                    }

                    existed.ProductImages.Add(new ProductImage
                    {
                        IsPrimary = null,
                        Alternative = update.Name,
                        Url = await photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images")
                    });
                }
            }

            existed.Name = update.Name;
            existed.Price = update.Price;
            existed.Description = update.Description;
            existed.CategoryId = (int)update.CategoryId;
            existed.SKU = update.SKU;
            existed.CountId = update.CountId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Product product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductTags).ThenInclude(p => p.Tag)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (product == null) throw new NotFoundException("Your request was not found");

            return View(product);
        }

    }
}
