using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;

        public AccountController(UserManager<Account> userManager,
            SignInManager<Account> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account
                {
                    UserName = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(account, viewModel.Password);

                if (result.Succeeded)
                {
                    //await _userManager.AddToRoleAsync(account, "User");
                    await _signInManager.SignInAsync(account, isPersistent: false);
                    Thread.Sleep(2000);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    return RedirectToAction("PreIndex", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            //Trash!
            string url = Request.Headers["Referer"].ToString();
            string returnUrl = RemovePrefixFromUrl(url);

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    viewModel.Email,
                    viewModel.Password,
                    viewModel.RememberMe,
                    false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(viewModel.ReturnUrl) && Url.IsLocalUrl(viewModel.ReturnUrl))
                    {
                        return Redirect(viewModel.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong email and (or) password");
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private static string RemovePrefixFromUrl(string url)
        {
            return new Regex(@"(.*):(\d*)").Replace(url, string.Empty);
        }

        [HttpPost]
        public IActionResult SetLanguage1(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
            return LocalRedirect(returnUrl);
        }
    }
}
