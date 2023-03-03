using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.Services;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserViewModelService _userViewModelService;
        private readonly ICollectionViewModelService _collectionViewModelService;
        private readonly IExtraViewModelService _extraViewModelService;

        public UserController(IUserViewModelService userViewModelService, ICollectionViewModelService collectionViewModelService, IExtraViewModelService extraViewModelService)
        {
            _userViewModelService = userViewModelService;
            _collectionViewModelService = collectionViewModelService;
            _extraViewModelService = extraViewModelService;
        }

        public async Task<IActionResult> Index()
        {
            var items =  await _userViewModelService.GetUsers();
            return View(items);
        }

        public async Task<IActionResult> UserPage(string userId)
        {
            var user = await _userViewModelService.GetUser(userId);
            return View(user);
        }

        public async Task<IActionResult> UserCollection(string userId)
        {
            var user = await _userViewModelService.GetUser(userId);
            var model = await _collectionViewModelService.GetPrivateTurboItems(userId, null);
            model.User = user;
            return View(model);
        }

        public async Task<IActionResult> UserExtra(string userId)
        {
            //var user = await _userViewModelService.GetUser(userId);
            //var model = await _collectionViewModelService.GetPrivateTurboItems(userId, null);
            //model.User = user;
            //return View(model);
            var items = await _extraViewModelService.GetExtraTurboItems(userId);
            var model = new ExtraTurboItemsViewModel();
            model.Items = items;
            model.User = await _userViewModelService.GetUser(userId);
            return View(model);
        }
    }
}
