using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace _15_11_23.Servicers
{
    public class LayoutServices
    {
        private readonly IHttpContextAccessor _http;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public LayoutServices(IHttpContextAccessor http, AppDbContext context, UserManager<AppUser> userManager)
        {
            _http = http;
            _context = context;
            _userManager = userManager;
        }
        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            Dictionary<string, string> keyValuePairs = await _context.Settings.ToDictionaryAsync(p=>p.Key,p=>p.Value);
            return keyValuePairs;
        }

        public async Task<List<CartItemVM>> GetItemAsync()
        {
            List<CartItemVM> cartVM = new List<CartItemVM>();
            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(b => b.BasketItems.Where(p => p.OrderId == null)).ThenInclude(p => p.Product)
                    .ThenInclude(pi => pi.ProductImages.Where(pi => pi.IsPrimary == true))
                    .FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                foreach (BasketItem item in appUser.BasketItems)
                {
                    cartVM.Add(new CartItemVM
                    {
                        Id = item.ProductId,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Count = item.Count,
                        SubTotal = item.Count * item.Product.Price,
                        Image = item.Product.ProductImages.FirstOrDefault()?.Url
                    });
                }
            }
            else
            {
                if (_http.HttpContext.Request.Cookies["Basket"] is not null)
                {
                    List<CartCookieItemVM> cart = JsonConvert.DeserializeObject<List<CartCookieItemVM>>(_http.HttpContext.Request.Cookies["Basket"]);
                    foreach (CartCookieItemVM cartCookieItemVM in cart)
                    {
                        Product product = await _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
                            .FirstOrDefaultAsync(p => p.Id == cartCookieItemVM.Id);
                        if (product is not null)
                        {
                            CartItemVM cartItemVM = new CartItemVM
                            {
                                Id = cartCookieItemVM.Id,
                                Name = product.Name,
                                Price = product.Price,
                                Image = product.ProductImages.FirstOrDefault().Url,
                                Count = cartCookieItemVM.Count,
                                SubTotal = (decimal)cartCookieItemVM.Count * product.Price

                            };
                            cartVM.Add(cartItemVM);
                        }
                    }
                }
            }

            return cartVM;
        }
        public async Task<List<CartItemVM>> GetCookieItemAsync(List<CartCookieItemVM> cartCookieItems)
        {
            List<CartItemVM> cartVM = new List<CartItemVM>();
            if (_http.HttpContext.Request.Cookies["Basket"] is not null)
            {
                foreach (CartCookieItemVM cartCookieItemVM in cartCookieItems)
                {
                    Product product = await _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
                        .FirstOrDefaultAsync(p => p.Id == cartCookieItemVM.Id);
                    if (product is not null)
                    {
                        CartItemVM cartItemVM = new CartItemVM
                        {
                            Id = cartCookieItemVM.Id,
                            Name = product.Name,
                            Price = product.Price,
                            Image = product.ProductImages.FirstOrDefault().Url,
                            Count = cartCookieItemVM.Count,
                            SubTotal = (decimal)cartCookieItemVM.Count * product.Price

                        };
                        cartVM.Add(cartItemVM);
                    }
                }
            }
            return cartVM;
        }

        public async Task<List<CartItemVM>> GetDbItemAsync(AppUser appUser)
        {
            List<CartItemVM> cartVM = new List<CartItemVM>();
            AppUser appUsers = await _userManager.Users.Include(b => b.BasketItems.Where(p => p.OrderId == null)).ThenInclude(p => p.Product)
                .ThenInclude(pi => pi.ProductImages.Where(pi => pi.IsPrimary == true))
                .FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            foreach (BasketItem item in appUsers.BasketItems)
            {
                cartVM.Add(new CartItemVM
                {
                    Id = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    Count = item.Count,
                    SubTotal = item.Count * item.Product.Price,
                    Image = item.Product.ProductImages.FirstOrDefault()?.Url
                });
            }
            return cartVM;
        }

        public async Task<List<CartCookieItemVM>> GetCartCookie()
        {
            List<CartCookieItemVM> cartCookieItems;
            if (_http.HttpContext.Request.Cookies["Basket"] is not null)
            {
                cartCookieItems = JsonConvert.DeserializeObject<List<CartCookieItemVM>>(_http.HttpContext.Request.Cookies["Basket"]);
                return cartCookieItems;
            }
            return null;
        }

        public async Task<AppUser> GetCartUser()
        {
            AppUser appUser = await _userManager.Users.Include(b => b.BasketItems.Where(p => p.OrderId == null)).ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return appUser;
        }
    }
}
