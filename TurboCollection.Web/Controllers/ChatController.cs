using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUserViewModelService _userViewModelService;
        private readonly IChatViewModelService _chatViewModelService;
        public ChatController(IUserViewModelService userViewModelService, IChatViewModelService chatViewModelService)
        {
            _userViewModelService = userViewModelService;
            _chatViewModelService = chatViewModelService;
        }

        public async Task<IActionResult> ChatWith(string userId)
        {
            ChatViewModel model = new ChatViewModel();
            string myUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _chatViewModelService.ResetUnread(myUserId, userId);
            model.ChatPosts = await _chatViewModelService.GetChatPosts(myUserId, userId);
            model.User = await _userViewModelService.GetUser(myUserId);
            model.ChatWithUser = await _userViewModelService.GetUser(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChatWith(string toUserId, string text)
        {
            var myUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _chatViewModelService.AddChatPost(text, myUserId, toUserId);
            ChatViewModel model = new ChatViewModel();
            model.ChatPosts = await _chatViewModelService.GetChatPosts(myUserId, toUserId);
            model.User = await _userViewModelService.GetUser(myUserId);
            model.ChatWithUser = await _userViewModelService.GetUser(toUserId);
            return View(model);
        }

        public async Task<IActionResult> MyChats()
        {
            string myUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ChatsViewModel model = new ChatsViewModel();
            model.User = await _userViewModelService.GetUser(myUserId);
            model.Users = await _chatViewModelService.GetChatUsers(myUserId);
            model.User.IsNewChatPost = await _chatViewModelService.IsNewChatPost(myUserId);
            return View(model);
        }
    }
}
