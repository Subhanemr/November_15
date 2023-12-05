using _15_11_23.DAL;
using _15_11_23.Models;
using _15_11_23.Utilities.Enums;
using _15_11_23.Utilities.Extendions;
using _15_11_23.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace _15_11_23.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM, string? returnUrl)
        {
            if (!ModelState.IsValid) return View(registerVM);
            if (!Regex.IsMatch(registerVM.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                ModelState.AddModelError("Email", "Invalid email format");
                return View(registerVM);
            }
            AppUser appUser = new AppUser
            {
                Name = registerVM.Name.Capitalize(),
                Surname = registerVM.Surname.Capitalize(),
                Email = registerVM.Email,
                UserName = registerVM.UserName,
                Genders = registerVM.Genders.ToString(),
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(appUser, UserRole.Member.ToString());
            await _signInManager.SignInAsync(appUser, false);
            if (returnUrl == null)
            {
                return RedirectToAction("index", "Home");
            }

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> LogOut(string? returnUrl)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl == null)
            {
                return RedirectToAction("index", "Home");
            }
            return Redirect(returnUrl);
        }
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM loginVM, string? returnUrl)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username, Email or Password is incorrect");
                    return View(loginVM);
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsRemembered, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Login is not enable please try latter");
                return View(loginVM);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username, Email or Password is incorrect");
                return View(loginVM);
            }
            if (returnUrl == null)
            {
                return RedirectToAction("index", "Home");
            }
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!(await _roleManager.RoleExistsAsync(role.ToString())))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });

                }
            }
            return RedirectToAction("index", "Home");
        }
    }
}
