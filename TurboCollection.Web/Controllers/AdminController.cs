using Microsoft.AspNetCore.Mvc;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICollectionViewModelService _collectionViewModelService;
        private const int BATCH_SIZE = 50;

        public AdminController(ICollectionViewModelService collectionViewModelService)
        {
            _collectionViewModelService = collectionViewModelService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> TurboItems(TurboItemsViewModel model)
        {
            //model = await _collectionViewModelService.GetTurboItems(model.CollectionFilerApplied, model.Search, 0, BATCH_SIZE);
            var modelOutput = await _collectionViewModelService.GetTurboItems(model);

            return View(modelOutput);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTurboItems(int collectionId, string searchString, int[] tagValues, int firstItem = 0)
        {

            var viewmodel = await _collectionViewModelService.GetTurboItems(collectionId, searchString, tagValues, firstItem, BATCH_SIZE);

            return PartialView(viewmodel);
        }

        public async Task<IActionResult> EditTurboItem(int id)
        {
            TurboItemEditViewModel modelOutput = await _collectionViewModelService.GetTurboItem(id);

            return View(modelOutput);
        }

        [HttpPost]
        public async Task<IActionResult> EditTurboItem(TurboItemEditViewModel model)
        {
             _collectionViewModelService.UpdateTurboItem(model);
            return RedirectToAction("TurboItems", "Admin");
        }

        public async Task<IActionResult> Tags()
        {
            var model = await _collectionViewModelService.GetAllTags();
            return View(model);
        }

        public IActionResult CreateNewTag()
        {
            TagViewModel tagViewModel = new TagViewModel();
            return View(tagViewModel);
        }

        [HttpPost]
        public IActionResult CreateNewTag(TagViewModel model)
        {
            _collectionViewModelService.CreateNewTag(model);
            return RedirectToAction("Tags", "Admin");
        }

        public IActionResult Users()
        {
            return View();
        }
    }
}
