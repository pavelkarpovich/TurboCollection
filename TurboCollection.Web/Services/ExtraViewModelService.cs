using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExtraViewModelService(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
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

        public async Task<IEnumerable<SelectListItem>> GetCollections()
        {
            var items = await _dbContext.Collections
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                //.OrderBy(brand => brand.Text)
                .ToListAsync();
            return items;
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
    }
}
