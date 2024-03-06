using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureVault.Models;
using SecureVault.ViewModels;
using System.Security.Claims;
using System.Text;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net;

namespace SecureVault.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;

        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == User.Claims.First().Value.ToString());
                ProfileViewModel model = new ProfileViewModel(user);

                return View(model);
            }
            else
                return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.email);
                if (user == null)
                {
                    user = new User
                    {
                        Name = model.firstName,
                        Surname = model.lastName,
                        Patronymic = model.patronymic,
                        Email = model.email,
                        Password = Encoding.UTF8.GetBytes(model.password)
                    };

                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Set<User>().FirstOrDefaultAsync(e => e.Email == model.email);
                if (user != null && user.Password.SequenceEqual(Encoding.UTF8.GetBytes(model.password)))
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    if (model.Name != null)
                        user.Name = model.Name;
                    if (model.Surname != null)
                        user.Surname = model.Surname;
                    if (model.Patronymic != null)
                        user.Patronymic = model.Patronymic;
                    if (model.Address != null)
                        user.Address = model.Address;
                    if (model.PhoneNumber != null)
                        user.PhoneNumber = model.PhoneNumber;
                    if (model.Country != null)
                        user.Country = model.Country;
                    if (model.Region != null)
                        user.Region = model.Region;
                    if (model.City != null)
                        user.City= model.City;
                    if (model.Zip != null)
                        user.Zip = model.Zip;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректный пароль");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id));
        }
    }
}
