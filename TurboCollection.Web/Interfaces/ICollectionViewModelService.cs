using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Web.Models;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Interfaces
{
    public interface ICollectionViewModelService
    {
        Task<TurboItemsViewModel> GetTurboItems(int? collectionId, string? search);
        Task<TurboItemsViewModel> GetTurboItems(int? collectionId, string? search, int skip, int take);
    }
}
