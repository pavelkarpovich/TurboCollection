using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Infrastructure.Data;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Services
{
    public class ExtraViewModelService : IExtraViewModelService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Account> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExtraViewModelService(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment, UserManager<Account> userManager)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task <List<ExtraTurboItemViewModel>> GetExtraTurboItems(string userId)
        {
            return await _dbContext.ExtraTurboItems
                .Where(x => x.UserId == userId)
                .Select(x => new ExtraTurboItemViewModel()
                {
                    Id = x.Id,
                    CollectionId = x.CollectionId,
                    Number = x.Number,
                    PictureUrl = x.PictureUrl,
                })
                .ToListAsync();
        }

        public List<SelectListItem> GetCollections()
        {
            var items = _dbContext.Collections
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                //.OrderBy(brand => brand.Text)
                .ToList();
            return items;
        }

        public async Task<ExtraTurboItemEditViewModel> GetExtraTurboItem(int id)
        {
            var collections = GetCollections();
            ExtraTurboItemEditViewModel model = await _dbContext.ExtraTurboItems
                .Where(x => x.Id == id)
                .Select(x => new ExtraTurboItemEditViewModel()
                {
                    Id = x.Id,
                    CollectionId = x.CollectionId,
                    Number = x.Number,
                    PictureUrl = x.PictureUrl,
                    Collections = collections
                }) 
                .FirstOrDefaultAsync();
            return model;
        }

        public async Task AddNewExtra(ExtraTurboItemEditViewModel model, string userId)
        {
            
            string uniqueFileName = UploadedFile(model);
            var productNumber = Guid.NewGuid();
            await _dbContext.ExtraTurboItems.AddAsync(new ExtraTurboItem(model.CollectionId, model.Number, userId, uniqueFileName));
            await _dbContext.SaveChangesAsync();
        }

        private string UploadedFile(ExtraTurboItemEditViewModel model)
        {
            string uniqueFileName = "";

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/extra");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return "/images/extra/" + uniqueFileName;
        }

        public async Task UpdateExtra(ExtraTurboItemEditViewModel model)
        {
            //var user = await _userManager.FindByIdAsync(userId);
            var extra = await _dbContext.ExtraTurboItems.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (model.Picture != null && extra.PictureUrl != model.PictureUrl)
            {
                DeleteFile(extra.PictureUrl);
                string uniqueFileName = UploadedFile(model);
                extra.PictureUrl = uniqueFileName;
            }
            extra.Number = model.Number;
            extra.CollectionId = model.CollectionId;
            _dbContext.SaveChangesAsync();
        }

        public async Task DeleteExtra(int id)
        {
            var itemToRemove = await _dbContext.ExtraTurboItems.SingleOrDefaultAsync(x => x.Id == id); //returns a single item.

            if (itemToRemove != null)
            {
                _dbContext.ExtraTurboItems.Remove(itemToRemove);
                _dbContext.SaveChangesAsync();
            }
            //throw new NotImplementedException();
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
