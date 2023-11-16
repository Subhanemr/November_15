using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.ModelsVM;
using Microsoft.AspNetCore.Mvc;

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
            //    List<Product> produts1 = new List<Product> {
            //    new Product
            //    {
            //        Name = "American Marigold",
            //        Price = 23.45m,
            //        CountId = 1,
            //        ImgUrl= "1-7-270x300.jpg",
            //        ImgUrl2 = "1-8-270x300.jpg"
            //    },
            //    new Product
            //    {
            //        Name = "Black Eyed Susan",
            //        Price = 23.45m,
            //        CountId = 2,
            //        ImgUrl = "1-8-270x300.jpg",
            //        ImgUrl2 = "1-1-270x300.jpg"
            //    },
            //     new Product
            //    {
            //        Name = "American Marigold",
            //        Price = 23.45m,
            //        CountId = 6,
            //        ImgUrl = "1-2-270x300.jpg",
            //        ImgUrl2 = "1-3-270x300.jpg"
            //    },
            //     new Product
            //    {
            //        Name = "Doublefile Viburnum",
            //        Price = 67.45m,
            //        CountId = 3,
            //        ImgUrl = "1-7-270x300.jpg",
            //        ImgUrl2 = "1-6-270x300.jpg"
            //    },
            //     new Product
            //    {
            //        Name = "Butterfly Weed",
            //        Price = 50.45m,
            //        CountId = 4,
            //        ImgUrl = "1-6-270x300.jpg",
            //        ImgUrl2 = "1-5-270x300.jpg"
            //    },
            //     new Product
            //    {
            //        Name = "Bleeding Heart",
            //        Price = 45.00m,
            //        CountId = 5,
            //        ImgUrl = "1-3-270x300.jpg",
            //        ImgUrl2 = "1-4-270x300.jpg"
            //    },
            //     new Product
            //    {
            //        Name = "American Marigold",
            //        Price = 23.45m,
            //        CountId = 7,
            //        ImgUrl = "1-7-270x300.jpg",
            //        ImgUrl2 = "1-8-270x300.jpg"
            //    },
            //     new Product
            //    {
            //        Name = "American Marigold",
            //        Price = 23.45m,
            //        CountId = 9,
            //        ImgUrl = "1-7-270x300.jpg",
            //        ImgUrl2 = "1-8-270x300.jpg"
            //    },
            //     new Product
            //    {
            //        Name = "American Marigold",
            //        Price = 23.45m,
            //        CountId = 8,
            //        ImgUrl = "1-7-270x300.jpg",
            //        ImgUrl2 = "1-8-270x300.jpg"
            //    },
            //     new Product
            //    {
            //        Name = "American Marigold",
            //        Price = 23.45m,
            //        CountId = 10,
            //        ImgUrl = "1-7-270x300.jpg",
            //        ImgUrl2 = "1-8-270x300.jpg"
            //    }
            //};

            //    _context.Products.AddRange(produts);
            //    _context.SaveChanges();


            //List <Slide> slides1 = new List<Slide> {
            //new Slide
            //{
            //    Title = "Title1",
            //    SubTitle = "SubTitle1",
            //    Description = "Des1",
            //    ImgUrl= "1-2-524x617.png"
            //},
            //new Slide
            //{
            //    Title = "Title2",
            //    SubTitle = "SubTitle2",
            //    Description = "Des2",
            //    ImgUrl= "1-2-524x617.png"
            //},
            //new Slide
            //{
            //    Title = "Title3",
            //    SubTitle = "SubTitle3",
            //    Description = "Des3",
            //    ImgUrl= "1-2-524x617.png"
            //}
            //};
            //_context.Slides.AddRange(slides1);
            //_context.SaveChanges();

            List<Product> products = _context.Products.OrderByDescending(s=>s.CountId).Take(8).ToList();
            List<Slide> slides = _context.Slides.OrderBy(s => s.Id).Take(3).ToList();

            HomeVM vm = new HomeVM(products, slides);

            return View(vm);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
