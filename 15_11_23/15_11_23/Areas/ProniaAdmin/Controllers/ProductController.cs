using _15_11_23.Areas.ProniaAdmin.ViewModels;
using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Extendions;
using _15_11_23.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        private void CreatePopulateDropdowns(CreateProductVM productVM)
        {
            productVM.Tags = _context.Tags.ToList();
            productVM.Categories = _context.Categories.ToList();
            productVM.Colors = _context.Colors.ToList();
            productVM.Sizes = _context.Sizes.ToList();
        }
        private void UpdatePopulateDropdowns(UpdateProductVM productVM)
        {
            productVM.Tags = _context.Tags.ToList();
            productVM.Categories = _context.Categories.ToList();
            productVM.Colors = _context.Colors.ToList();
            productVM.Sizes = _context.Sizes.ToList();
        }

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
                .ToListAsync();
            return View(product);
        }
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
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                CreatePopulateDropdowns(productVM);
                return View(productVM);
            }
            bool result = await _context.Products.AnyAsync(c => c.Name.ToLower().Trim() == productVM.Name.ToLower().Trim());
            if (result)
            {
                CreatePopulateDropdowns(productVM);
                ModelState.AddModelError("Name", "A Name is available");
                return View(productVM);
            };
            bool resultOrder = await _context.Products.AnyAsync(c => c.CountId == productVM.CountId);
            if (resultOrder)
            {
                CreatePopulateDropdowns(productVM);
                ModelState.AddModelError("Order", "A Order is available");
                return View(productVM);
            }

            bool resultCategory = await _context.Categories.AnyAsync(c => c.Id == productVM.CategoryId);
            if (!resultCategory)
            {
                CreatePopulateDropdowns(productVM);
                ModelState.AddModelError("CategoryId", "This id has no category");
                return View(productVM);
            }
            if (productVM.Price <= 0)
            {
                CreatePopulateDropdowns(productVM);
                ModelState.AddModelError("Price", "Price cannot be 0");
                return View(productVM);
            };

            foreach (int tagId in productVM.TagIds)
            {
                bool tagResult = await _context.Tags.AnyAsync(t => t.Id == tagId);
                if (!tagResult)
                {
                    CreatePopulateDropdowns(productVM);
                    return View(productVM);
                }
            }
            foreach (int sizeId in productVM.SizeIds)
            {
                bool sizeResult = await _context.Sizes.AnyAsync(t => t.Id == sizeId);
                if (!sizeResult)
                {
                    CreatePopulateDropdowns(productVM);
                    return View(productVM);
                }
            }
            foreach (int colorId in productVM.ColorIds)
            {
                bool colorResult = await _context.Colors.AnyAsync(t => t.Id == colorId);
                if (!colorResult)
                {
                    CreatePopulateDropdowns(productVM);
                    return View(productVM);
                }
            }
            if (!productVM.MainPhoto.ValidateType())
            {
                CreatePopulateDropdowns(productVM);
                ModelState.AddModelError("Photo", "File Not supported");
                return View(productVM);
            }

            if (!productVM.MainPhoto.ValidataSize(10))
            {
                CreatePopulateDropdowns(productVM);
                ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                return View(productVM);
            }

            if (!productVM.HoverPhoto.ValidateType())
            {
                CreatePopulateDropdowns(productVM);
                ModelState.AddModelError("Photo", "File Not supported");
                return View(productVM);
            }

            if (!productVM.HoverPhoto.ValidataSize(10))
            {
                CreatePopulateDropdowns(productVM);
                ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                return View(productVM);
            }


            ProductImage mainImage = new ProductImage { 
            IsPrimary = true,
            Alternative = productVM.Name,
            Url = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images")
            };

            ProductImage hoverImage = new ProductImage
            {
                IsPrimary = false,
                Alternative = productVM.Name,
                Url = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images")
            };

            Product product = new Product
            {
                Name = productVM.Name,
                Price = productVM.Price,
                Description = productVM.Description,
                SKU = productVM.SKU,
                CategoryId = (int)productVM.CategoryId,
                CountId = productVM.CountId,
                ProductTags = productVM.TagIds.Select(tagId => new ProductTag { TagId = tagId }).ToList(),
                ProductSizes = productVM.SizeIds.Select(sizeId => new ProductSize { SizeId = sizeId }).ToList(),
                ProductColors = productVM.ColorIds.Select(colorId => new ProductColor { ColorId = colorId }).ToList(),
                ProductImages = new List<ProductImage> { mainImage , hoverImage}
            };

            TempData["Message"] = "";

            foreach(IFormFile photo in productVM.Photos)
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
                    Alternative = productVM.Name,
                    Url = await photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images")
                });
            }


            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Product existed = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductColors)
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existed == null) return NotFound();

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

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) { return BadRequest(); }
            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductColors)
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
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
        public async Task<IActionResult> Update(int id, UpdateProductVM productVM)
        {
            Product existed = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductSizes)
                .Include(p => p.ProductColors)
                .FirstOrDefaultAsync(c => c.Id == id);

            productVM.ProductImages = existed.ProductImages;
            if (!ModelState.IsValid)
            {
                UpdatePopulateDropdowns(productVM);
                return View(productVM);
            }
           
            if (existed == null) return NotFound();
            bool resultCategory = await _context.Categories.AnyAsync(c => c.Id == productVM.CategoryId);
            if (!resultCategory)
            {
                UpdatePopulateDropdowns(productVM);
                ModelState.AddModelError("CategoryId", "This id has no category");
                return View(productVM);
            }

            bool resultTag = await _context.Tags.AnyAsync(pt => productVM.TagIds.Contains(pt.Id));
            if (!resultTag)
            {
                UpdatePopulateDropdowns(productVM);
                ModelState.AddModelError("TagId", "This id has no tag");
                return View(productVM);
            }
            List<ProductTag> tagsToRemove = existed.ProductTags
                .Where(pt => !productVM.TagIds.Contains(pt.TagId))
                .ToList();
            _context.ProductTags.RemoveRange(tagsToRemove);

            List<ProductTag> tagsToAdd = productVM.TagIds
                .Except(existed.ProductTags.Select(pt => pt.TagId))
                .Select(tagId => new ProductTag { TagId = tagId })
                .ToList();
            existed.ProductTags.AddRange(tagsToAdd);

            bool resultColor = await _context.Colors.AnyAsync(pc => productVM.ColorIds.Contains(pc.Id));
            if (!resultColor)
            {
                UpdatePopulateDropdowns(productVM);
                ModelState.AddModelError("ColorId", "This id has no color");
                return View(productVM);
            }

            List<ProductColor> colorToRemove = existed.ProductColors
                .Where(pc => !productVM.ColorIds.Contains(pc.ColorId))
                .ToList();
            _context.ProductColors.RemoveRange(colorToRemove);

            List<ProductColor> colorToAdd = productVM.ColorIds
                .Except(existed.ProductColors.Select(pc => pc.ColorId))
                .Select(colorId => new ProductColor { ColorId = colorId })
                .ToList();
            existed.ProductColors.AddRange(colorToAdd);

            bool resultSize = await _context.Sizes.AnyAsync(ps => productVM.SizeIds.Contains(ps.Id));
            if (!resultSize)
            {
                UpdatePopulateDropdowns(productVM);
                ModelState.AddModelError("SizeId", "This id has no size");
                return View(productVM);
            }

            List<ProductSize> sizeToRemove = existed.ProductSizes
                .Where(ps => !productVM.SizeIds.Contains(ps.SizeId))
                .ToList();
            _context.ProductSizes.RemoveRange(sizeToRemove);

            List<ProductSize> sizeToAdd = productVM.SizeIds
                .Except(existed.ProductSizes.Select(ps => ps.SizeId))
                .Select(sizeId => new ProductSize { SizeId = sizeId })
                .ToList();
            existed.ProductSizes.AddRange(sizeToAdd);


            if(productVM.MainPhoto is not null)
            {
                if (!productVM.MainPhoto.ValidateType())
                {
                    UpdatePopulateDropdowns(productVM);
                    ModelState.AddModelError("Photo", "File Not supported");
                    return View(productVM);
                }

                if (!productVM.MainPhoto.ValidataSize(10))
                {
                    UpdatePopulateDropdowns(productVM);
                    ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                    return View(productVM);
                }
            }

            if(productVM.HoverPhoto is not null)
            {
                if (!productVM.HoverPhoto.ValidateType())
                {
                    UpdatePopulateDropdowns(productVM);
                    ModelState.AddModelError("Photo", "File Not supported");
                    return View(productVM);
                }

                if (!productVM.HoverPhoto.ValidataSize(10))
                {
                    UpdatePopulateDropdowns(productVM);
                    ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                    return View(productVM);
                }
            }

            if(productVM.MainPhoto is not null)
            {
                string fileName = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                ProductImage prMain = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == true);
                prMain.Url.DeleteFileAsync(fileName);
                _context.ProductImages.Remove(prMain);

                existed.ProductImages.Add(new ProductImage
                {
                    Alternative = productVM.Name,
                    IsPrimary = true,
                    Url = fileName
                });
            }
            if (productVM.HoverPhoto is not null)
            {
                string fileName = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                ProductImage prHover = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == false);
                prHover.Url.DeleteFileAsync(fileName);
                _context.ProductImages.Remove(prHover);

                existed.ProductImages.Add(new ProductImage
                {
                    Alternative = productVM.Name,
                    IsPrimary = false,
                    Url = fileName
                });
            }

            if (existed.ProductImages is null) existed.ProductImages = new List<ProductImage>();

            if (productVM.ImageIds is null) productVM.ImageIds = new List<int>();


            List<ProductImage> remove = existed.ProductImages.Where(pi => pi.IsPrimary == null && !productVM.ImageIds.Exists(imgId => imgId == pi.id)).ToList();
            foreach (ProductImage image in remove)
            {
                image.Url.DeleteFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.ProductImages.Remove(image);
            }

            if (productVM.ImageIds is null) productVM.ImageIds = new List<int>();
            
            TempData["Message"] = "";

            if(productVM.Photos is not null)
            {
                foreach (IFormFile photo in productVM.Photos)
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
                        Alternative = productVM.Name,
                        Url = await photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images")
                    });
                }
            }

            existed.Name = productVM.Name;
            existed.Price = productVM.Price;
            existed.Description = productVM.Description;
            existed.CategoryId = (int)productVM.CategoryId;
            existed.SKU = productVM.SKU;
            existed.CountId = productVM.CountId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> More(int id)
        {
            if (id <= 0) return BadRequest();
            Product product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductTags).ThenInclude(p => p.Tag)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

    }
}
