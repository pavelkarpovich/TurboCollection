using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using TurboCollection.Models;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.Models;
using TurboCollection.Web.Services;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICollectionViewModelService _collectionViewModelService;
        private readonly IUserViewModelService _userViewModelService;

        public HomeController(ILogger<HomeController> logger, ICollectionViewModelService collectionViewModelService,
            IUserViewModelService userViewModelService)
        {
            _logger = logger;
            _collectionViewModelService = collectionViewModelService;
            _userViewModelService = userViewModelService;
        }

        public async Task<IActionResult> PreIndex()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _collectionViewModelService.SeedPrivateTurboItems(userId);
            
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult Index()
        {
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