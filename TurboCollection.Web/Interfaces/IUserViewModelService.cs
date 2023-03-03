using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Interfaces
{
    public interface IUserViewModelService
    {
        Task<List<UserViewModel>> GetUsers();

        Task<UserViewModel> GetUser(string userId);
        UserViewModel GetUserSync(string userId);
    }
}
