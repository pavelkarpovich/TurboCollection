using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Controllers
{
    public class MyPageController : Controller
    {
        private readonly IUserViewModelService _userViewModelService;
        private readonly ICollectionViewModelService _collectionViewModelService;
        private readonly IExtraViewModelService _extraViewModelService;

        public MyPageController(IUserViewModelService userViewModelService, 
            ICollectionViewModelService collectionViewModelService, IExtraViewModelService extraViewModelService)
        {
            _userViewModelService = userViewModelService;
            _collectionViewModelService = collectionViewModelService;
            _extraViewModelService = extraViewModelService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userViewModelService.GetUser(userId);
            return View(user);
        }

        public async Task<IActionResult> MyCollection(PrivateTurboItemsViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model = await _collectionViewModelService.GetPrivateTurboItems(userId, model.CollectionFilerApplied);
            model.User = await _userViewModelService.GetUser(userId);
            return View(model);
        }

        public async Task<IActionResult> MyExtra()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _extraViewModelService.GetExtraTurboItems(userId);
            var model = new ExtraTurboItemsViewModel();
            model.Items = items;
            model.User = await _userViewModelService.GetUser(userId);
            return View(model);
        }

        public async Task<IActionResult> AddNewExtra()
        {
            var model = new ExtraTurboItemEditViewModel()
            {
                Collections = (await _extraViewModelService.GetCollections()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewExtra(ExtraTurboItemEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _extraViewModelService.AddNewExtra(model, userId);
                return RedirectToAction(nameof(MyExtra));
            }
            return View();
        }

        public async Task<IActionResult> Search()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _collectionViewModelService.SearchForNeeded(userId);
            var model = new ExtraTurboItemSearchResultsViewModel();
            model.Items = list;
            model.User = await _userViewModelService.GetUser(userId);
            return View(model);
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
