using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Account> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserViewModelService(AccountDbContext dbContext, UserManager<Account> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<UserViewModel> GetUser(string userId)
        {
            var user = await _dbContext.Accounts.Where(x => x.Id == userId).Select(x => 
                new UserViewModel(x.Id, x.FirstName, x.LastName, x.PictureUrl)).FirstOrDefaultAsync();
            return user;
        }

        public async Task<UserEditViewModel> GetUserEdit(string userId)
        {
            var user = await _dbContext.Accounts.Where(x => x.Id == userId).Select(x =>
                new UserViewModel(x.Id, x.FirstName, x.LastName, x.PictureUrl)).FirstOrDefaultAsync();
            UserEditViewModel userEdit = new UserEditViewModel(user.UserId, user.FirstName, user.LastName, user.PictureUrl);
            return userEdit;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            var users = await _dbContext.Accounts.Select(x =>
                new UserViewModel(x.Id, x.FirstName, x.LastName, x.PictureUrl)).ToListAsync();
            return users;
        }

        public UserViewModel GetUserSync(string userId)
        {
            var user = _dbContext.Accounts.Where(x => x.Id == userId).Select(x =>
                new UserViewModel(x.Id, x.FirstName, x.LastName, x.PictureUrl)).FirstOrDefault();
            return user;
        }

        public async Task UpdateUser(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (model.Picture != null && user.PictureUrl != model.PictureUrl)
            {
                DeleteFile(user.PictureUrl);
                string uniqueFileName = UploadedFile(model);
                user.PictureUrl = uniqueFileName;
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            await _userManager.UpdateAsync(user);
        }

        private string UploadedFile(UserEditViewModel model)
        {
            string uniqueFileName = "";

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/info");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return "/images/info/" + uniqueFileName;
        }

        private void DeleteFile(string fileName)
        {
            string fullPath = _webHostEnvironment.WebRootPath + fileName;

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
