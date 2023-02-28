using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Infrastructure.Data;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.Models;
using TurboCollection.Web.ViewModels;

namespace TurboCollection.Web.Services
{
    public class CollectionViewModelServicecs : ICollectionViewModelService
    {
        private readonly ApplicationDbContext _dbContext;

        public CollectionViewModelServicecs(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TurboItemsViewModel> GetTurboItems(int? collectionId, string? search)
        {
            var items = await _dbContext.TurboItems.Where(x => (!collectionId.HasValue || x.CollectionId == collectionId) 
            && (string.IsNullOrEmpty(search) || x.Name.Contains(search)))
                .Select(x => new TurboItemViewModel()
                {
                    Id= x.Id,
                    CollectionId= x.CollectionId,
                    Picture = x.Picture,
                    Name= x.Name,
                    Speed= x.Speed,
                    EngineCapacity = x.EngineCapacity,
                    HorsePower= x.HorsePower,
                    Year= x.Year,
                    Tags = x.Tags
                })
                .ToListAsync();

            var collections = await _dbContext.Collections.ToListAsync();
            var collectionList = collections
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                //.OrderBy(x => x.Value)
                .ToList();
            var allitem = new SelectListItem() { Value = null, Text = "All", Selected = true };
            collectionList.Insert(0, allitem);


            TurboItemsViewModel model = new TurboItemsViewModel();
            model.Items = items;
            model.Collections = collectionList;

            return model;
        }

        public async Task<TurboItemsViewModel> GetTurboItems(int? collectionId, string? search, int skip, int take)
        {
            var items = await _dbContext.TurboItems
                .Where(x => (!collectionId.HasValue || collectionId == 0 || x.CollectionId == collectionId)
            && (string.IsNullOrEmpty(search) || x.Name.Contains(search)))
                .Select(x => new TurboItemViewModel()
                {
                    Id = x.Id,
                    CollectionId = x.CollectionId,
                    Picture = x.Picture,
                    Name = x.Name,
                    Speed = x.Speed,
                    EngineCapacity = x.EngineCapacity,
                    HorsePower = x.HorsePower,
                    Year = x.Year,
                    Tags = x.Tags
                //})
                }).Skip(skip).Take(take)
                .ToListAsync();

            var collections = await _dbContext.Collections.ToListAsync();
            var collectionList = collections
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                //.OrderBy(x => x.Value)
                .ToList();
            var allitem = new SelectListItem() { Value = null, Text = "", Selected = true };
            collectionList.Insert(0, allitem);

            TurboItemsViewModel model = new TurboItemsViewModel();
            model.Items = items;
            model.Collections = collectionList;
            model.CollectionFilerApplied = collectionId;
            model.Search= search;

            return model;
        }
    }
}
