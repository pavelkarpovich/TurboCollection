using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using TurboCollection.Controllers;
using TurboCollection.Models;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICollectionViewModelService _collectionViewModelService;

        public CollectionController(ILogger<HomeController> logger, ICollectionViewModelService collectionViewModelService)
        {
            _logger = logger;
            _collectionViewModelService = collectionViewModelService;
        }

        private const int BATCH_SIZE = 50;
        public async Task<IActionResult> FullCollection(TurboItemsViewModel model)
        {
            model = await _collectionViewModelService.GetTurboItems(model.CollectionFilerApplied, model.Search, 0, BATCH_SIZE);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> _TestData(int sortOrder, string searchString, int firstItem = 0)
        {

            var viewmodel = await _collectionViewModelService.GetTurboItems(sortOrder, searchString, firstItem, BATCH_SIZE);

            return PartialView(viewmodel);
        }

        public async Task<IActionResult> MyCollection(TurboItemsViewModel model)
        {
            model = await _collectionViewModelService.GetTurboItems(model.CollectionFilerApplied, model.Search, 0, BATCH_SIZE);

            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
