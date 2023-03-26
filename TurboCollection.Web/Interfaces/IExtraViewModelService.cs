using Microsoft.AspNetCore.Mvc.Rendering;
using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Interfaces
{
    public interface IExtraViewModelService
    {
        Task<List<ExtraTurboItemViewModel>> GetExtraTurboItems(string userId);
        Task<ExtraTurboItemEditViewModel> GetExtraTurboItem(int id);
        List<SelectListItem> GetCollections();
        Task AddNewExtra(ExtraTurboItemEditViewModel productIndexViewModel, string userId);
        Task UpdateExtra(ExtraTurboItemEditViewModel productIndexViewModel);
        Task DeleteExtra(int id);
    }
}
