using _15_11_23.DAL;
using _15_11_23.Interfaces;
using _15_11_23.Models;
using _15_11_23.Servicers;
using _15_11_23.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;
using System.Security.Claims;

namespace _15_11_23.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly LayoutServices _layoutServices;


        public CartController(AppDbContext context, UserManager<AppUser> userManager, IEmailService emailService, LayoutServices layoutServices)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _layoutServices = layoutServices;
        }

        public async Task<IActionResult> Index()
        {
            List<CartItemVM> cartVM = new List<CartItemVM>();


            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(b => b.BasketItems.Where(bi => bi.OrderId == null))
                    .ThenInclude(p => p.Product)
                    .ThenInclude(pi => pi.ProductImages.Where(pi => pi.IsPrimary == true))
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
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
                if (Request.Cookies["Basket"] is not null)
                {
                    List<CartCookieItemVM> cart = JsonConvert.DeserializeObject<List<CartCookieItemVM>>(Request.Cookies["Basket"]);
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

            }


            return View(cartVM);
        }

        public async Task<IActionResult> AddBasket(int id)
        {
            if (id <= 0) return BadRequest();
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();
            List<CartCookieItemVM> cart;
            List<CartItemVM> cartItems;

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(p => p.BasketItems.Where(bi => bi.OrderId == null)).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (appUser == null) return NotFound();
                BasketItem item = appUser.BasketItems.FirstOrDefault(b => b.ProductId == id);
                if (item == null)
                {
                    item = new BasketItem
                    {
                        AppUserId = appUser.Id,
                        ProductId = product.Id,
                        Price = product.Price,
                        Count = 1
                    };

                    appUser.BasketItems.Add(item);
                }
                else
                {
                    item.Count++;
                }
                await _context.SaveChangesAsync();

                //cartItems = await _layoutServices.GetDbItemAsync(appUser);
            }
            else
            {
                if (Request.Cookies["Basket"] is not null)
                {
                    cart = JsonConvert.DeserializeObject<List<CartCookieItemVM>>(Request.Cookies["Basket"]);

                    CartCookieItemVM item = cart.FirstOrDefault(c => c.Id == id);
                    if (item == null)
                    {
                        CartCookieItemVM cartCookieItem = new CartCookieItemVM
                        {
                            Id = id,
                            Count = 1
                        };
                        cart.Add(cartCookieItem);
                    }
                    else
                    {
                        item.Count++;
                    }
                }
                else
                {
                    cart = new List<CartCookieItemVM>();
                    CartCookieItemVM cartCookieItem = new CartCookieItemVM
                    {
                        Id = id,
                        Count = 1
                    };
                    cart.Add(cartCookieItem);
                }

                string json = JsonConvert.SerializeObject(cart);
                Response.Cookies.Append("Basket", json, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1),
                });

                //cartItems = await _layoutServices.GetCookieItemAsync(cart);

            }

            //return PartialView("CartItem/_CartPartialView", cartItems);
            return RedirectToAction(nameof(Index), "Home");
        }
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (id <= 0) return BadRequest();

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(p => p.BasketItems.Where(bi => bi.OrderId == null))
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (appUser == null) return NotFound();

                BasketItem item = appUser.BasketItems.FirstOrDefault(b => b.ProductId == id);

                if (item == null) return BadRequest();

                _context.BasketItems.Remove(item);

                await _context.SaveChangesAsync();
            }
            else
            {
                List<CartCookieItemVM> cart = JsonConvert.DeserializeObject<List<CartCookieItemVM>>(Request.Cookies["Basket"]);

                CartCookieItemVM item = cart.FirstOrDefault(c => c.Id == id);

                if (item == null) return BadRequest();

                cart.Remove(item);


                string json = JsonConvert.SerializeObject(cart);
                Response.Cookies.Append("Basket", json, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });

            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CountMinus(int id)
        {

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(b => b.BasketItems.Where(bi => bi.OrderId == null))
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (appUser == null) return NotFound();

                BasketItem item = appUser.BasketItems.FirstOrDefault(b => b.ProductId == id);

                if (item == null) return BadRequest();

                item.Count--;
                if (item.Count <= 0)
                {
                    return await DeleteItem(id);
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                List<CartCookieItemVM> cart = JsonConvert.DeserializeObject<List<CartCookieItemVM>>(Request.Cookies["Basket"]);

                CartCookieItemVM item = cart.FirstOrDefault(c => c.Id == id);

                if (item == null) return BadRequest();

                item.Count--;
                if (item.Count <= 0)
                {
                    return await DeleteItem(id);
                }

                string json = JsonConvert.SerializeObject(cart);
                Response.Cookies.Append("Basket", json, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });
            }


            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> CountPlus(int id)
        {

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(b => b.BasketItems.Where(bi => bi.OrderId == null))
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (appUser == null) return NotFound();

                BasketItem item = appUser.BasketItems.FirstOrDefault(b => b.ProductId == id);

                if (item == null) return BadRequest();

                item.Count++;
                if (item.Count <= 0)
                {
                    return await DeleteItem(id);
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                List<CartCookieItemVM> cart = JsonConvert.DeserializeObject<List<CartCookieItemVM>>(Request.Cookies["Basket"]);

                CartCookieItemVM item = cart.FirstOrDefault(c => c.Id == id);

                if (item == null) return BadRequest();

                item.Count++;
                if (item.Count <= 0)
                {
                    return await DeleteItem(id);
                }

                string json = JsonConvert.SerializeObject(cart);
                Response.Cookies.Append("Basket", json, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });

            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Checkout()
        {
            AppUser appUser = await _userManager.Users.Include(b => b.BasketItems.Where(bi => bi.OrderId == null)).ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            OrderVM orderVM = new OrderVM
            {
                BasketItems = appUser.BasketItems
            };
            return View(orderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderVM orderVM)
        {
            AppUser appUser = await _userManager.Users.Include(b => b.BasketItems.Where(bi => bi.OrderId == null)).ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!appUser.BasketItems.Any(b => b.OrderId == null))
            {
                orderVM.BasketItems = appUser.BasketItems;
                return View(orderVM);
            }
            if (!ModelState.IsValid)
            {
                orderVM.BasketItems = appUser.BasketItems;
                return View(orderVM);
            }

            decimal total = 0;
            foreach (BasketItem item in appUser.BasketItems)
            {
                item.Price = item.Product.Price;
                total += item.Price * item.Count;
            }

            Order order = new Order
            {
                Status = null,
                Address = orderVM.Address,
                PruchaseAt = DateTime.Now,
                AppUserId = appUser.Id,
                BasketItems = appUser.BasketItems,
                TotalPrice = total
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            string bodyHtml = await Table(order);


            await _emailService.SendMailAsync(appUser.Email, "Your Order", bodyHtml, true);

            return RedirectToAction("MyOrders", "Account");
        }

        public async Task<string> Table(Order order)
        {
            string bodyHtml = @"
<h2 style=""font-family: arial, sans-serif; margin:15px;"">Your Order</h2>

<table style=""font-family: arial, sans-serif; border-collapse: collapse; width: 100%; border-radius: 10px; background-color: #E2E0E0;"">
                            <thead>
                                <tr>
                                    <th style=""border-bottom: 1px solid #9E9E9E; text-align: left; padding: 20px; font-weight: bold;  font-size: 25px;"">Product</th>
                                    <th style=""border-bottom: 1px solid #9E9E9E; text-align: left; padding: 20px; font-weight: bold;  font-size: 25px;"">Unit Price</th>
                                    <th style=""border-bottom: 1px solid #9E9E9E; text-align: left; padding: 20px; font-weight: bold;  font-size: 25px;"">Quantity</th>
                                    <th style=""border-bottom: 1px solid #9E9E9E; text-align: left; padding: 20px; font-weight: bold;  font-size: 25px;"">Total</th>
                                </tr>
                            </thead>
                            <tbody>";

            decimal total = 0;
            foreach (BasketItem item in order.BasketItems)
            {
                total += item.Price * item.Count;
                bodyHtml += $@"
                                    <tr>

                                        <td style=""border-bottom: 1px solid #9E9E9E; text-align: left; padding: 20px; font-size: 20px;"">
                                            {item.Product.Name}
                                        </td>
                                        <td style=""border-bottom: 1px solid #9E9E9E; text-align: left; padding: 20px; font-size: 20px;"">
                                            ${item.Price}
                                        </td>
                                        <td style=""border-bottom: 1px solid #9E9E9E; text-align: left; padding: 20px; font-size: 20px;"">
                                            {item.Count}
                                        </td>
                                        <td style=""border-bottom: 1px solid #9E9E9E; text-align: left; padding: 20px; font-size: 20px;"">
                                            ${total}
                                        </td>
                                    </tr>
                                    <tr>";
            }

            bodyHtml += $@"                              
                                    	<td colspan=""4"" style=""border-bottom: 1px solid #9E9E9E; text-align: left; padding: 20px; font-size: 25px;"">
                                    		<h2>Orders totals</h2> <h4>Total: ${total}</h4>
                                        </td>
                                    </tr>

                            </tbody>
</table>";


            return bodyHtml;
        }
    }
}
