using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Infrastructure.Data;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Services
{
    public class UserViewModelService : IUserViewModelService
    {
        private readonly AccountDbContext _dbContext;

        public UserViewModelService(AccountDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserViewModel> GetUser(string userId)
        {
            var user = await _dbContext.Accounts.Where(x => x.Id == userId).Select(x => 
                new UserViewModel(x.Id, x.FirstName, x.LastName)).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            var users = await _dbContext.Accounts.Select(x =>
                new UserViewModel(x.Id, x.FirstName, x.LastName)).ToListAsync();
            return users;
        }

        public UserViewModel GetUserSync(string userId)
        {
            var user = _dbContext.Accounts.Where(x => x.Id == userId).Select(x =>
                new UserViewModel(x.Id, x.FirstName, x.LastName)).FirstOrDefault();
            return user;
        }
    }
}
