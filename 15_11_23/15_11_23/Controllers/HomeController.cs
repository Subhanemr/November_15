using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.ModelsVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //List<Product> productsList = new List<Product> {
            //new Product
            //{
            //    Name = "American Marigold",
            //    Price = 300.00m,
            //    CountId = 3,
            //    SKU = "Ch-256xl",
            //    Description = "Des1",
            //    CategoryId = 2
            //},
            //new Product
            //{
            //    Name = "American Marigold",
            //    Price = 23.45m,
            //    CountId = 1,
            //    SKU = "Ch-26xl",
            //    Description = "Des2",
            //    CategoryId = 1
            //},
            //new Product
            //{
            //    Name = "American Marigold",
            //    Price = 45.23m,
            //    CountId = 2,
            //    SKU = "Ch-56b",
            //    Description = "Des3",
            //    CategoryId = 3
            //},
            //new Product
            //{
            //    Name = "African Black Flowers",
            //    Price = 999.99m,
            //    CountId = 4,
            //    SKU = "Ch-999xxxxxxl",
            //    Description = "Des4",
            //    CategoryId = 4
            //},
            //new Product
            //{
            //    Name = "African Black Flowers",
            //    Price = 1000.00m,
            //    CountId = 6,
            //    SKU = "Ch-666xxxxxxl",
            //    Description = "Des5",
            //    CategoryId = 4
            //},
            //new Product
            //{
            //    Name = "African Black Flowers",
            //    Price = 0.10m,
            //    CountId = 12,
            //    SKU = "Ch-13xxxxxxxl",
            //    Description = "Des12",
            //    CategoryId = 4
            //},
            //new Product
            //{
            //    Name = "American Marigold",
            //    Price = 12.23m,
            //    CountId = 5,
            //    SKU = "Ch-25b",
            //    Description = "Des6",
            //    CategoryId = 3
            //},
            //new Product
            //{
            //    Name = "American Marigold",
            //    Price = 56.67m,
            //    CountId = 7,
            //    SKU = "Ch-6xl",
            //    Description = "Des7",
            //    CategoryId = 1
            //},
            //new Product
            //{
            //    Name = "American Marigold",
            //    Price = 76.23m,
            //    CountId = 9,
            //    SKU = "Ch-46m",
            //    Description = "Des8",
            //    CategoryId = 3
            //},
            //new Product
            //{
            //    Name = "American Marigold",
            //    Price = 34.12m,
            //    CountId = 8,
            //    SKU = "Ch-12s",
            //    Description = "Des9",
            //    CategoryId = 2
            //},
            // new Product
            //{
            //    Name = "American Marigold",
            //    Price = 76.24m,
            //    CountId = 10,
            //    SKU = "Ch-26s",
            //    Description = "Des10",
            //    CategoryId = 1
            //},
            //  new Product
            //{
            //    Name = "American Marigold",
            //    Price = 45.78m,
            //    CountId = 11,
            //    SKU = "Ch-12d",
            //    Description = "Des11",
            //    CategoryId = 2
            //},
            //};
            //_context.Products.AddRange(productsList);
            //_context.SaveChanges();

            //List<Category> categories = new List<Category>
            //{
            //    new Category
            //    {
            //        Name = "Office"
            //    },
            //    new Category
            //    {
            //        Name = "Home"
            //    },
            //    new Category
            //    {
            //        Name = "Otel"
            //    },
            //    new Category
            //    {
            //        Name = "Africa"
            //    }
            //};
            //_context.Categories.AddRange(categories);
            //_context.SaveChanges();

            //List<Client> clientsList = new List<Client>
            //{
            //    new Client
            //    {
            //        Name = "Phoenix Baker",
            //        Description = "Des1",
            //        IsClient = true,
            //        ImgUrl = "1--1rnu4p.png"
            //    },
            //    new Client
            //    {
            //        Name = "Phoenix Baker",
            //        Description = "Des2",
            //        IsClient = false,
            //        ImgUrl = "2.png"
            //    },
            //    new Client
            //    {
            //        Name = "Phoenix Baker",
            //        Description = "Des3",
            //        IsClient = true,
            //        ImgUrl = "3.png"
            //    },
            //};
            //_context.Clients.AddRange(clientsList);
            //_context.SaveChanges();

            //List<Blog> blogsList = new List<Blog>
            //{
            //    new Blog
            //    {
            //        Title = "Aenean Vulputate Lorem",
            //        Description = "Des1",
            //        DateTime = DateTime.Now,
            //        ByWho = "Ryan Gosling",
            //        ImgUrl = "1-2-310x220.jpg"
            //    },
            //    new Blog
            //    {
            //        Title = "There Many Variations",
            //        Description = "Des2",
            //        DateTime = DateTime.Now,
            //        ByWho = "John Wick",
            //        ImgUrl = "1-3-310x220.jpg"
            //    },
            //    new Blog
            //    {
            //        Title = "Maecenas Laoreet Massa",
            //        Description = "Des3",
            //        DateTime = DateTime.Now,
            //        ByWho = "Jason Statham",
            //        ImgUrl = "1-1-310x220.jpg"
            //    }
            //};
            //_context.Blogs.AddRange(blogsList);
            //_context.SaveChanges();

            //List<Slide> slidesList = new List<Slide>
            //{
            //    new Slide
            //    {
            //        Title = "Title1",
            //        SubTitle = "SubTitle1",
            //        Description = "Des1",
            //        ImgUrl = "1-2-524x617.png"
            //    },
            //    new Slide
            //    {
            //        Title = "Title2",
            //        SubTitle = "SubTitle2",
            //        Description = "Des2",
            //        ImgUrl = "1-1-524x617.png"
            //    },
            //    new Slide
            //    {
            //        Title = "Title3",
            //        SubTitle = "SubTitle3",
            //        Description = "Des3",
            //        ImgUrl = "1-1-524x617.png"
            //    }
            //};
            //_context.Blogs.AddRange(_context.Blogs);
            //_context.SaveChanges();

            //List<ProductImage> productImages = new List<ProductImage>
            //{
            //    new ProductImage
            //    {
            //        Url = "1-1-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 1
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-2-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 1
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-1-570x633.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 1
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-4-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 2
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-3-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 2
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-2-570x633.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 2
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-8-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 3
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-7-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 3
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-6-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 3
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-1-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 4
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-2-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 4
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-1-570x633.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 4
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-8-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 5
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-1-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 5
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-6-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 5
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-8-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 6
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-7-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 6
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-6-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 6
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-8-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 7
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-6-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 7
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-7-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 7
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-3-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 8
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-7-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 8
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-6-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 8
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-1-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 9
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-2-570x633.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 9
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-6-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 9
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-1-570x633.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 10
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-7-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 10
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-3-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 10
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-1-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 11
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-4-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 11
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-2-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 11
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-2-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = true,
            //        ProductId = 12
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-4-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = false,
            //        ProductId = 12
            //    },
            //    new ProductImage
            //    {
            //        Url = "1-1-270x300.jpg",
            //        Alternative = "Product Images",
            //        IsPrimary = null,
            //        ProductId = 12
            //    }
            //};
            //_context.ProductImages.AddRange(productImages);
            //_context.SaveChanges();

            //List<Color> colors = new List<Color>
            //{
            //    new Color
            //    {
            //        Name = "Red"
            //    },
            //    new Color
            //    {
            //        Name = "Green"
            //    },
            //    new Color
            //    {
            //        Name = "Black"
            //    },
            //    new Color
            //    {
            //        Name = "Blue"
            //    },
            //    new Color
            //    {
            //        Name = "White"
            //    }
            //};
            //_context.Colors.AddRange(colors);
            //_context.SaveChanges();

            //List<ProductColor> productColors = new List<ProductColor>
            //{
            //    new ProductColor
            //    {
            //        ProductId = 1,
            //        ColorId = 1,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 1,
            //        ColorId = 2,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 2,
            //        ColorId = 4,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 2,
            //        ColorId = 5,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 3,
            //        ColorId = 1,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 3,
            //        ColorId = 4,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 4,
            //        ColorId = 3,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 5,
            //        ColorId = 3,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 6,
            //        ColorId = 1,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 6,
            //        ColorId = 5,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 7,
            //        ColorId = 1,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 7,
            //        ColorId = 4,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 8,
            //        ColorId = 1,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 9,
            //        ColorId = 1,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 9,
            //        ColorId = 2,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 10,
            //        ColorId = 1,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 10,
            //        ColorId = 2,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 11,
            //        ColorId = 2,
            //    },
            //    new ProductColor
            //    {
            //        ProductId = 12,
            //        ColorId = 3,
            //    }
            //};
            //_context.ProductColors.AddRange(productColors);
            //_context.SaveChanges();

            //List<Size> sizes = new List<Size> 
            //{
            //    new Size
            //    {
            //        Name = "Small"
            //    },
            //    new Size
            //    {
            //        Name = "Medium"
            //    },
            //    new Size
            //    {
            //        Name = "Large"
            //    },
            //    new Size
            //    {
            //        Name = "Big"
            //    },
            //    new Size
            //    {
            //        Name = "XXXXL"
            //    }
            //};
            //_context.Sizes.AddRange(sizes);
            //_context.SaveChanges();

            //List<Tag> tags = new List<Tag> 
            //{
            //    new Tag
            //    {
            //        Name = "Garden"
            //    },
            //    new Tag
            //    {
            //        Name = "Furniture"
            //    },
            //    new Tag
            //    {
            //        Name = "Flower"
            //    },
            //    new Tag
            //    {
            //        Name = "Air"
            //    },
            //    new Tag
            //    {
            //        Name = "AfrikaHelper"
            //    },
            //    new Tag
            //    {
            //        Name = "Water"
            //    }
            //};
            //_context.Tags.AddRange(tags);
            //_context.SaveChanges();

            //List<ProductSize> productSizes = new List<ProductSize> 
            //{
            //    new ProductSize
            //    {
            //        ProductId = 1,
            //        SizeId = 2
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 1,
            //        SizeId = 3
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 2,
            //        SizeId = 2
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 2,
            //        SizeId = 3
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 3,
            //        SizeId = 1
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 3,
            //        SizeId = 3
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 4,
            //        SizeId = 4
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 4,
            //        SizeId = 5
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 5,
            //        SizeId = 4
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 5,
            //        SizeId = 5
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 6,
            //        SizeId = 4
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 6,
            //        SizeId = 5
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 7,
            //        SizeId = 2
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 7,
            //        SizeId = 3
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 8,
            //        SizeId = 1
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 8,
            //        SizeId = 2
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 8,
            //        SizeId = 3
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 9,
            //        SizeId = 1
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 9,
            //        SizeId = 2
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 9,
            //        SizeId = 3
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 10,
            //        SizeId = 2
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 11,
            //        SizeId = 2
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 12,
            //        SizeId = 4
            //    },
            //    new ProductSize
            //    {
            //        ProductId = 12,
            //        SizeId = 4
            //    }
            //};
            //_context.ProductSizes.AddRange(productSizes);
            //_context.SaveChanges();

            //List<ProductTag> productTags = new List<ProductTag> 
            //{
            //    new ProductTag
            //    {
            //        ProductId = 1,
            //        TagId = 2
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 1,
            //        TagId = 3
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 2,
            //        TagId = 1
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 2,
            //        TagId = 1
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 3,
            //        TagId = 1
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 3,
            //        TagId = 4
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 4,
            //        TagId = 5
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 4,
            //        TagId = 6
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 5,
            //        TagId = 5
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 5,
            //        TagId = 6
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 6,
            //        TagId = 1
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 6,
            //        TagId = 4
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 7,
            //        TagId = 1
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 7,
            //        TagId = 3
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 8,
            //        TagId = 1
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 8,
            //        TagId = 3
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 9,
            //        TagId = 3
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 10,
            //        TagId = 2
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 11,
            //        TagId = 2
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 12,
            //        TagId = 5
            //    },
            //    new ProductTag
            //    {
            //        ProductId = 12,
            //        TagId = 6
            //    }
            //};
            //_context.ProductTags.AddRange(productTags);
            //_context.SaveChanges();

            List<Product> products = _context.Products
            .Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).OrderByDescending(s => s.CountId).Take(8)
            .ToList();
            
            List<Slide> slides = _context.Slides.OrderBy(s => s.Id).Take(3).ToList();
            List<Client> clients = _context.Clients.ToList();
            List<Blog> blogs = _context.Blogs.ToList();

            HomeVM vm = new HomeVM { Slides = slides, Products = products, Clients = clients, Blogs = blogs };

            return View(vm);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}