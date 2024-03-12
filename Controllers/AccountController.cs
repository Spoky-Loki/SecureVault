using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureVault.Models;
using SecureVault.ViewModels;
using System.Security.Claims;
using System.Text;
using SecureVault.Services;
using Microsoft.AspNetCore.Authorization;

namespace SecureVault.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly EmailService emailService;

        public AccountController(ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            emailService = new EmailService(configuration);
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
        public IActionResult EmailConfirmation()
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id.ToString() == userId);
            if (user == null)
                return View("Error");

            if (user.EmailConfirmationToken.Equals(code))
            {
                user.EmailConfirmationToken = null;
                user.EmailConfirmed = true;

                _context.Users.Update(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
                return View("Error");
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

                    var code = emailService.GenerateEmailConfirmationToken();
                    user.EmailConfirmationToken = code;

                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    var callbackUrl = Url.Action(
                       "ConfirmEmail",
                       "Account",
                       new { userId = user.Id, code = code },
                       protocol: HttpContext.Request.Scheme);
                    emailService.SendEmail(model.email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    return RedirectToAction("EmailConfirmation");
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
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return View(model);
                    }
                    else
                    {
                        await Authenticate(user);
                        return RedirectToAction("Index", "Home");
                    }
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
