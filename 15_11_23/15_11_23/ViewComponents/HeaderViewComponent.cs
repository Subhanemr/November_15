using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace _15_11_23.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(AppDbContext context, IHttpContextAccessor http, UserManager<AppUser> userManager)
        {
            _context = context;
            _http = http;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, string> keyValuePairs = await _context.Settings.ToDictionaryAsync(p => p.Key, p => p.Value);


            List<CartItemVM> cartVM = new List<CartItemVM>();
            if (_http.HttpContext.Request.Cookies["Basket"] is not null)
            {
                List<CartCookieItemVM> cart = JsonConvert.DeserializeObject<List<CartCookieItemVM>>(_http.HttpContext.Request.Cookies["Basket"]);
                foreach (CartCookieItemVM cartCookieItemVM in cart)
                {
                    Product product = await _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true)).FirstOrDefaultAsync(p => p.Id == cartCookieItemVM.Id);
                    if (product is not null)
                    {
                        CartItemVM cartItemVM = new CartItemVM
                        {
                            Id = cartCookieItemVM.Id,
                            Name = product.Name,
                            Price = product.Price,
                            Image = product.ProductImages.FirstOrDefault().Url,
                            Count = cartCookieItemVM.Count,
                            SubTotal = Convert.ToDecimal(cartCookieItemVM.Count) * product.Price

                        };
                        cartVM.Add(cartItemVM);
                    }
                }
            }
            AppUser appUser = new AppUser();

            if (User.Identity.IsAuthenticated)
            {
                appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            HeaderVM headerVM = new HeaderVM { Settings = keyValuePairs, Items = cartVM, User = appUser };

            return View(headerVM);
        }
    }
}
