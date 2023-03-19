using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Infrastructure.Data;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Services
{
    public class ChatViewModelService : IChatViewModelService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AccountDbContext _dbContextAccount;
        public ChatViewModelService(ApplicationDbContext dbContext, AccountDbContext dbContextAccount)
        {
            _dbContext = dbContext;
            _dbContextAccount = dbContextAccount;
        }

        public async Task AddChatPost(string text, string fromUserId, string toUserId)
        {
            await _dbContext.ChatPosts.AddAsync(new ChatPost(text, DateTime.Now, fromUserId, toUserId, false));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ChatPostViewModel>> GetChatPosts(string fromUserId, string toUserId)
        {
            return await _dbContext.ChatPosts
                .Where(x => (x.FromUserId == fromUserId && x.ToUserId == toUserId) || (x.FromUserId == toUserId && x.ToUserId == fromUserId))
                .Select(x => new ChatPostViewModel()
                {
                    Text = x.Text,
                    DateTime = x.DateTime,
                    FromUserId = x.FromUserId,
                    ToUserId = x.ToUserId
                })
                .ToListAsync();
            //throw new NotImplementedException();
        }

        //public Task SetUnread(string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task ResetUnread(string userId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<UserViewModel>> GetChatUsers(string userId)
        {
            var fromUsers = _dbContext.ChatPosts
                .Where(x => x.FromUserId == userId || x.ToUserId == userId)
                .Select(x => x.FromUserId).ToArray();
            var toUsers = _dbContext.ChatPosts
                .Where(x => x.FromUserId == userId || x.ToUserId == userId)
                .Select(x => x.ToUserId).ToArray();
            string[] users = { userId };
            var allUsers = fromUsers.Except(users).Union(toUsers.Except(users));

            //var user = await _dbContext.Accounts.Where(x => x.Id == userId).Select(x =>
            //    new UserViewModel(x.Id, x.FirstName, x.LastName)).FirstOrDefaultAsync();

            List<UserViewModel> userList = new List<UserViewModel>();
            foreach (var itemUserId in allUsers)
            {
                var user = _dbContextAccount.Accounts.Where(x => x.Id == itemUserId).Select(x =>
                    new UserViewModel(x.Id, x.FirstName, x.LastName)).FirstOrDefault();
                userList.Add(user);
            }
            return userList;
            //var allUsers = fromUsers.Except<string>(userId).Union(toUsers).Except<string>(userId);
            throw new NotImplementedException();
        }
    }
}
