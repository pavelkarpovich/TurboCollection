using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TurboCollection.Controllers;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Api
{
    [Route("collection/api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly ICollectionViewModelService _collectionViewModelService;
        public CollectionController(ICollectionViewModelService collectionViewModelService)
        {
            _collectionViewModelService = collectionViewModelService;
        }

        [HttpPost("SetStatus")]
        //[HttpPost]
        public string SetStatus(int privateTurboItemId)
        //public string SetStatus(int sortOrder, int searchString)
        //public string GetProjects([FromBody] StatusModel model )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //_collectionViewModelService.UpdateStatus(userId, privateTurboItemId, statusId);
            return "dfdf";
        }
    }
}
