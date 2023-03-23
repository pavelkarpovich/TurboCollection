using Microsoft.AspNetCore.Mvc.Rendering;
using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Interfaces
{
    public interface IChatViewModelService
    {
        Task AddChatPost(string text, string fromUserId, string toUserId);
        Task<List<ChatPostViewModel>> GetChatPosts(string fromUserId, string toUserId);
        Task<List<UserViewModel>> GetChatUsers(string userId);
        Task<bool> IsNewChatPost(string userId);
        //Task SetUnread(string userId);
        Task ResetUnread(string myUserId, string userId);
    }
}
