using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;
using TurboCollection.Models;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.Models;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _stringLocalizer;

        public HomeController(IStringLocalizer<HomeController> stringLocalizer, ILogger<HomeController> logger)
        {
            _stringLocalizer = stringLocalizer;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Collection"] = _stringLocalizer["Collection"].Value;
            ViewData["Registration"] = _stringLocalizer["Registration"].Value;
            ViewData["Login"] = _stringLocalizer["Login"].Value;
            ViewData["Welcome"] = _stringLocalizer["Welcome"].Value;
            return View();
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