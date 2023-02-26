using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
        public async Task<IActionResult> TestData(TurboItemsViewModel model)
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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Collection(TurboItemsViewModel model)
        {
            var viewmodel = await _collectionViewModelService.GetTurboItems(model.CollectionFilerApplied, model.Search);

            return View(viewmodel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
