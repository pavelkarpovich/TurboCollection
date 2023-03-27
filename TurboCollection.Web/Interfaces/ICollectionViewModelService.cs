using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Web.Models;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Interfaces
{
    public interface ICollectionViewModelService
    {
        Task<TurboItemsViewModel> GetTurboItems(int? collectionId, string? search, int[] tagValues, int skip, int take);
        Task<TurboItemsViewModel> GetTurboItems(TurboItemsViewModel model);
        Task<TurboItemEditViewModel> GetTurboItem(int id);
        Task<PrivateTurboItemsViewModel> GetPrivateTurboItems(string userId, int? collectionId);
        Task SeedPrivateTurboItems(string userId);
        void UpdateStatus(string userId, long privateTurboStatusId, int statusId);
        int GetStatus(string userId, long privateTurboStatusId);
        Task<List<ExtraTurboItemSearchResultViewModel>> SearchForNeeded(string userId);
        void UpdateTurboItem(TurboItemEditViewModel model);
        Task<List<TagViewModel>> GetAllTags();
        void CreateNewTag(TagViewModel model);
    }
}
