using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
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
        private readonly IUserViewModelService _userViewModelService;
        private const int BATCH_SIZE = 50;
        //private readonly string _userId;

        public CollectionController(ILogger<HomeController> logger, ICollectionViewModelService collectionViewModelService
            , IUserViewModelService userViewModelService)
        {
            _logger = logger;
            _collectionViewModelService = collectionViewModelService;
            _userViewModelService = userViewModelService;
            //_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

        //[HttpPost("SetStatus")]
        [HttpPost]
        //public string SetStatus(string sortOrder, string searchString)
        public string SetStatus(int privateTurboItemId, int statusId)
        //public string GetProjects([FromBody] StatusModel model )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _collectionViewModelService.UpdateStatus(userId, privateTurboItemId, statusId);
            return "dfdf";
        }

        [HttpPost]
        public string GetStatus(int privateTurboItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var statusId = _collectionViewModelService.GetStatus(userId, privateTurboItemId);
            return statusId.ToString();
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
    }
}
