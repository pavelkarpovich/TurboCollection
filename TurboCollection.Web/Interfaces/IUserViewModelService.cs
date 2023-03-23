using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Interfaces
{
    public interface IUserViewModelService
    {
        Task<List<UserViewModel>> GetUsers();

        Task<UserViewModel> GetUser(string userId);
        Task<UserEditViewModel> GetUserEdit(string userId);
        UserViewModel GetUserSync(string userId);
        Task UpdateUser(UserEditViewModel model);

    }
}
