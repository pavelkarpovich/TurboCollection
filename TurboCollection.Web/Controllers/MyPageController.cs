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
        private readonly IChatViewModelService _chatViewModelService;

        public MyPageController(IUserViewModelService userViewModelService, 
            ICollectionViewModelService collectionViewModelService, IExtraViewModelService extraViewModelService,
            IChatViewModelService chatViewModelService)
        {
            _userViewModelService = userViewModelService;
            _collectionViewModelService = collectionViewModelService;
            _extraViewModelService = extraViewModelService;
            _chatViewModelService = chatViewModelService;
        }

        public async Task<IActionResult> Info()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userViewModelService.GetUser(userId);
            user.IsNewChatPost = await _chatViewModelService.IsNewChatPost(userId);
            return View(user);
        }

        public async Task<IActionResult> EditInfo(string userId)
        {
            var user = await _userViewModelService.GetUserEdit(userId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditInfo(UserEditViewModel model)
        {
            await _userViewModelService.UpdateUser(model);
            return RedirectToAction("Info");
        }

        public async Task<IActionResult> MyCollection(PrivateTurboItemsViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _collectionViewModelService.SeedPrivateTurboItems(userId);
            model = await _collectionViewModelService.GetPrivateTurboItems(userId, model.CollectionFilerApplied);
            model.User = await _userViewModelService.GetUser(userId);
            model.User.IsNewChatPost = await _chatViewModelService.IsNewChatPost(userId);
            return View(model);
        }

        public async Task<IActionResult> MyExtra()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _extraViewModelService.GetExtraTurboItems(userId);
            var model = new ExtraTurboItemsViewModel();
            model.Items = items;
            model.User = await _userViewModelService.GetUser(userId);
            model.User.IsNewChatPost = await _chatViewModelService.IsNewChatPost(userId);
            return View(model);
        }

        public async Task<IActionResult> AddNewExtra()
        {
            var model = new ExtraTurboItemEditViewModel()
            {
                Collections = _extraViewModelService.GetCollections()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewExtra(ExtraTurboItemEditViewModel model)
        {
            //if (ModelState.IsValid)
            //{
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _extraViewModelService.AddNewExtra(model, userId);
                return RedirectToAction(nameof(MyExtra));
            //}
            //return View();
        }
        
        public async Task<IActionResult> EditExtra(int id)
        {
            var model = await _extraViewModelService.GetExtraTurboItem(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditExtra(ExtraTurboItemEditViewModel model)
        {

            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _extraViewModelService.UpdateExtra(model);
            return RedirectToAction(nameof(MyExtra));
        }

        public async Task<IActionResult> DeleteExtra(int id)
        {
                await _extraViewModelService.DeleteExtra(id);
                return RedirectToAction(nameof(MyExtra));
        }

        public async Task<IActionResult> Search()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _collectionViewModelService.SearchForNeeded(userId);
            var model = new ExtraTurboItemSearchResultsViewModel();
            model.Items = list;
            model.User = await _userViewModelService.GetUser(userId);
            model.User.IsNewChatPost = await _chatViewModelService.IsNewChatPost(userId);
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
