﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Infrastructure.Data;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.Models;
using TurboCollection.Web.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace TurboCollection.Web.Services
{
    public class CollectionViewModelServicecs : ICollectionViewModelService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserViewModelService _userViewModelService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CollectionViewModelServicecs(ApplicationDbContext dbContext, IUserViewModelService userViewModelService, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _userViewModelService = userViewModelService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<TurboItemsViewModel> GetTurboItems(int? collectionId, string? search, int[] tagValues, int skip, int take)
        {
            List<TurboItemTag> turboItemTagList = new List<TurboItemTag>();
            turboItemTagList = _dbContext.TurboItemTags.ToList();

            var turboItemsList = await _dbContext.TurboItems.ToListAsync();
            var items = turboItemsList
                .Where(x => (!collectionId.HasValue || collectionId == 0 || x.CollectionId == collectionId)
            && (string.IsNullOrEmpty(search) || x.Name.Contains(search))
                &&
                (
                tagValues == null || tagValues.Count() == 0 ||
            turboItemTagList.Where(y => y.TurboItemId == x.Id && tagValues.Contains(y.TagId)).Any()
            )
            )
                .Select(x => new TurboItemViewModel()
                {
                    Id = x.Id,
                    CollectionId = x.CollectionId,
                    PictureUrl = x.PictureUrl,
                    Name = x.Name,
                    Speed = x.Speed,
                    EngineCapacity = x.EngineCapacity,
                    HorsePower = x.HorsePower,
                    Year = x.Year,
                    Tags = x.Tags
                }).Skip(skip).Take(take)
                .ToList();

            var collections = await _dbContext.Collections.ToListAsync();
            var collectionList = collections
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                //.OrderBy(x => x.Value)
                .ToList();
            var allitem = new SelectListItem() { Value = null, Text = "", Selected = true };
            collectionList.Insert(0, allitem);

            var tags = await _dbContext.Tags.ToListAsync();
            var tagList = tags
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                .ToList();

            TurboItemsViewModel model = new TurboItemsViewModel();
            model.Items = items;
            model.Collections = collectionList;
            model.CollectionFilerApplied = collectionId;
            model.Search = search;
            model.Tags = tagList;
            model.TagIds = tagValues;

            return model;
        }

        public async Task<TurboItemsViewModel> GetTurboItems(TurboItemsViewModel modelInput)
        {
            List<TurboItemTag> turboItemTagList = new List<TurboItemTag>();
            turboItemTagList = _dbContext.TurboItemTags.ToList();

            var turboItemsList = await _dbContext.TurboItems.ToListAsync();
            var items = turboItemsList
                .Where(x => (!modelInput.CollectionFilerApplied.HasValue || modelInput.CollectionFilerApplied == 0 || x.CollectionId == modelInput.CollectionFilerApplied)
            && (string.IsNullOrEmpty(modelInput.Search) || x.Name.Contains(modelInput.Search))
                &&
                (
                modelInput.TagIds == null || modelInput.TagIds.Count() == 0 ||
            turboItemTagList.Where(y => y.TurboItemId == x.Id && modelInput.TagIds.Contains(y.TagId)).Any()
            )
            )
                .Select(x => new TurboItemViewModel()
                {
                    Id = x.Id,
                    CollectionId = x.CollectionId,
                    PictureUrl = x.PictureUrl,
                    Name = x.Name,
                    Speed = x.Speed,
                    EngineCapacity = x.EngineCapacity,
                    HorsePower = x.HorsePower,
                    Year = x.Year,
                    Tags = x.Tags
                }).Take(50)
                .ToList();

            var collections = await _dbContext.Collections.ToListAsync();
            var collectionList = collections
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                //.OrderBy(x => x.Value)
                .ToList();
            var allitem = new SelectListItem() { Value = null, Text = "", Selected = true };
            collectionList.Insert(0, allitem);

            var tags = await _dbContext.Tags.ToListAsync();
            var tagList = tags
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                .ToList();

            TurboItemsViewModel model = new TurboItemsViewModel();
            model.Items = items;
            model.Collections = collectionList;
            model.CollectionFilerApplied = modelInput.CollectionFilerApplied;
            model.Search = modelInput.Search;
            model.Tags = tagList;
            model.TagIds = modelInput.TagIds;

            return model;
        }

        public async Task<PrivateTurboItemsViewModel> GetPrivateTurboItems(string userId, int? collectionId)
        {
            var items = await _dbContext.PrivateTurboItems
                .Where(x => (!collectionId.HasValue || collectionId == 0 || x.CollectionId == collectionId)
                && (x.UserId == userId))
                .Select(x => new PrivateTurboItemViewModel()
                {
                    Id = x.Id,
                    CollectionId = x.CollectionId,
                    Number = x.Number,
                    StatusId = x.StatusId
                })
                .ToListAsync();

            var collections = await _dbContext.Collections.ToListAsync();
            var collectionList = collections
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                .ToList();
            var allitem = new SelectListItem() { Value = null, Text = "", Selected = true };
            collectionList.Insert(0, allitem);

            PrivateTurboItemsViewModel model = new PrivateTurboItemsViewModel();
            model.Items = items;
            model.Collections = collectionList;
            model.CollectionFilerApplied = collectionId;

            return model;
        }

        public async Task<List<TagViewModel>> GetAllTags()
        {
            return await _dbContext.Tags.Select(x => new TagViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
        }

        public async Task SeedPrivateTurboItems(string userId)
        {
            var privateCount = _dbContext.PrivateTurboItems.Where(x => x.UserId == userId).Count();
            if (privateCount == 0)
            {
                await _dbContext.AddRangeAsync(GetPreconfiguredPrivateTurboItems(userId));
                await _dbContext.SaveChangesAsync();
            }
        }

        public void UpdateStatus(string userId, long privateTurboItemId, int statusId)
        {
            var item = _dbContext.PrivateTurboItems.Where(x => x.UserId == userId && x.Id == privateTurboItemId).FirstOrDefault();
            item.StatusId = statusId;
            _dbContext.SaveChanges();
            item = _dbContext.PrivateTurboItems.Where(x => x.UserId == userId && x.Id == privateTurboItemId).FirstOrDefault();
        }

        public int GetStatus(string userId, long privateTurboItemId)
        {
            return _dbContext.PrivateTurboItems.Where(x => x.UserId == userId && x.Id == privateTurboItemId).FirstOrDefault().StatusId;
        }

        public async Task<List<ExtraTurboItemSearchResultViewModel>> SearchForNeeded(string userId)
        {
            var absentList = _dbContext.PrivateTurboItems.Where(x => x.UserId == userId && x.StatusId == 2).ToList();
            List<ExtraTurboItemSearchResultViewModel> extraListFull = new List<ExtraTurboItemSearchResultViewModel>();
            foreach (var item in absentList) 
            {
                var extraList = _dbContext.ExtraTurboItems
                    .Where(x => x.UserId != userId && x.CollectionId == item.CollectionId && x.Number == item.Number)
                    .Select(x => new ExtraTurboItemSearchResultViewModel(x.Id, x.CollectionId, x.Number, x.PictureUrl, _userViewModelService.GetUserSync(x.UserId)))
                    .ToList();
                extraListFull.AddRange(extraList);
            }
            return extraListFull;
        }

        public async Task<TurboItemEditViewModel> GetTurboItem(int id)
        {
            var tagIds = await _dbContext.TurboItemTags.Where(x => x.TurboItemId == id).Select(x => x.TagId).ToArrayAsync();
            var collections = GetCollections();
            var tags = GetTags();
            TurboItemEditViewModel model = await _dbContext.TurboItems
                .Where(x => x.Id == id)
                .Select(x => new TurboItemEditViewModel()
                {
                    Id = x.Id,
                    CollectionId = x.CollectionId,
                    Number = x.Number,
                    Name = x.Name,
                    PictureUrl = x.PictureUrl,
                    Collections = collections,
                    Tags = tags,
                    TagIds = tagIds
            
                })
                .FirstOrDefaultAsync();
            return model;
        }

        public void UpdateTurboItem(TurboItemEditViewModel model)
        {
            _dbContext.TurboItemTags.RemoveRange(_dbContext.TurboItemTags.Where(x => x.TurboItemId == model.Id));
            _dbContext.SaveChanges();

            if (model.TagIds != null)
            {
                foreach (var tagId in model.TagIds)
                {
                    var itemtag = new TurboItemTag(model.Id, tagId);
                    _dbContext.TurboItemTags.Add(itemtag);
                    _dbContext.SaveChanges();
                }
            }

            var turboItem = _dbContext.TurboItems.Where(x => x.Id == model.Id).FirstOrDefault();
            if (model.Picture != null && turboItem.PictureUrl != model.PictureUrl)
            {
                DeleteFile(turboItem.PictureUrl);
                string uniqueFileName = UploadedFile(model);
                turboItem.PictureUrl = uniqueFileName;
            }
            turboItem.Name = model.Name;
            _dbContext.SaveChanges();
        }

        public List<SelectListItem> GetCollections()
        {
            var items = _dbContext.Collections
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                //.OrderBy(brand => brand.Text)
                .ToList();
            return items;
        }

        public List<SelectListItem> GetTags()
        {
            var items = _dbContext.Tags
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                //.OrderBy(brand => brand.Text)
                .ToList();
            return items;
        }

        private void DeleteFile(string fileName)
        {
            string fullPath = _webHostEnvironment.WebRootPath + fileName;

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private string UploadedFile(TurboItemEditViewModel model)
        {
            string uniqueFileName = "";

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/collection");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return "/images/collection/" + uniqueFileName;
        }

        private static IEnumerable<PrivateTurboItem> GetPreconfiguredPrivateTurboItems(string userId)
        {
            return new List<PrivateTurboItem>()
            {
                new(1, 1, 1, userId),
                new(1, 2, 1, userId),
                new(1, 3, 1, userId),
                new(1, 4, 1, userId),
                new(1, 5, 1, userId),
                new(1, 6, 1, userId),
                new(1, 7, 1, userId),
                new(1, 8, 1, userId),
                new(1, 9, 1, userId),
                new(1, 10, 1, userId),
                new(1, 11, 1, userId),
                new(1, 12, 1, userId),
                new(1, 13, 1, userId),
                new(1, 14, 1, userId),
                new(1, 15, 1, userId),
                new(1, 16, 1, userId),
                new(1, 17, 1, userId),
                new(1, 18, 1, userId),
                new(1, 19, 1, userId),
                new(1, 20, 1, userId),
                new(1, 21, 1, userId),
                new(1, 22, 1, userId),
                new(1, 23, 1, userId),
                new(1, 24, 1, userId),
                new(1, 25, 1, userId),
                new(1, 26, 1, userId),
                new(1, 27, 1, userId),
                new(1, 28, 1, userId),
                new(1, 29, 1, userId),
                new(1, 30, 1, userId),
                new(1, 31, 1, userId),
                new(1, 32, 1, userId),
                new(1, 33, 1, userId),
                new(1, 34, 1, userId),
                new(1, 35, 1, userId),
                new(1, 36, 1, userId),
                new(1, 37, 1, userId),
                new(1, 38, 1, userId),
                new(1, 39, 1, userId),
                new(1, 40, 1, userId),
                new(1, 41, 1, userId),
                new(1, 42, 1, userId),
                new(1, 43, 1, userId),
                new(1, 44, 1, userId),
                new(1, 45, 1, userId),
                new(1, 46, 1, userId),
                new(1, 47, 1, userId),
                new(1, 48, 1, userId),
                new(1, 49, 1, userId),
                new(1, 50, 1, userId),
                new(2, 51, 1, userId),
                new(2, 52, 1, userId),
                new(2, 53, 1, userId),
                new(2, 54, 1, userId),
                new(2, 55, 1, userId),
                new(2, 56, 1, userId),
                new(2, 57, 1, userId),
                new(2, 58, 1, userId),
                new(2, 59, 1, userId),
                new(2, 60, 1, userId),
                new(2, 61, 1, userId),
                new(2, 62, 1, userId),
                new(2, 63, 1, userId),
                new(2, 64, 1, userId),
                new(2, 65, 1, userId),
                new(2, 66, 1, userId),
                new(2, 67, 1, userId),
                new(2, 68, 1, userId),
                new(2, 69, 1, userId),
                new(2, 70, 1, userId),
                new(2, 71, 1, userId),
                new(2, 72, 1, userId),
                new(2, 73, 1, userId),
                new(2, 74, 1, userId),
                new(2, 75, 1, userId),
                new(2, 76, 1, userId),
                new(2, 77, 1, userId),
                new(2, 78, 1, userId),
                new(2, 79, 1, userId),
                new(2, 80, 1, userId),
                new(2, 81, 1, userId),
                new(2, 82, 1, userId),
                new(2, 83, 1, userId),
                new(2, 84, 1, userId),
                new(2, 85, 1, userId),
                new(2, 86, 1, userId),
                new(2, 87, 1, userId),
                new(2, 88, 1, userId),
                new(2, 89, 1, userId),
                new(2, 90, 1, userId),
                new(2, 91, 1, userId),
                new(2, 92, 1, userId),
                new(2, 93, 1, userId),
                new(2, 94, 1, userId),
                new(2, 95, 1, userId),
                new(2, 96, 1, userId),
                new(2, 97, 1, userId),
                new(2, 98, 1, userId),
                new(2, 99, 1, userId),
                new(2, 100, 1, userId),
                new(2, 101, 1, userId),
                new(2, 102, 1, userId),
                new(2, 103, 1, userId),
                new(2, 104, 1, userId),
                new(2, 105, 1, userId),
                new(2, 106, 1, userId),
                new(2, 107, 1, userId),
                new(2, 108, 1, userId),
                new(2, 109, 1, userId),
                new(2, 110, 1, userId),
                new(2, 111, 1, userId),
                new(2, 112, 1, userId),
                new(2, 113, 1, userId),
                new(2, 114, 1, userId),
                new(2, 115, 1, userId),
                new(2, 116, 1, userId),
                new(2, 117, 1, userId),
                new(2, 118, 1, userId),
                new(2, 119, 1, userId),
                new(2, 120, 1, userId),
                new(3, 121, 1, userId),
                new(3, 122, 1, userId),
                new(3, 123, 1, userId),
                new(3, 124, 1, userId),
                new(3, 125, 1, userId),
                new(3, 126, 1, userId),
                new(3, 127, 1, userId),
                new(3, 128, 1, userId),
                new(3, 129, 1, userId),
                new(3, 130, 1, userId),
                new(3, 131, 1, userId),
                new(3, 132, 1, userId),
                new(3, 133, 1, userId),
                new(3, 134, 1, userId),
                new(3, 135, 1, userId),
                new(3, 136, 1, userId),
                new(3, 137, 1, userId),
                new(3, 138, 1, userId),
                new(3, 139, 1, userId),
                new(3, 140, 1, userId),
                new(3, 141, 1, userId),
                new(3, 142, 1, userId),
                new(3, 143, 1, userId),
                new(3, 144, 1, userId),
                new(3, 145, 1, userId),
                new(3, 146, 1, userId),
                new(3, 147, 1, userId),
                new(3, 148, 1, userId),
                new(3, 149, 1, userId),
                new(3, 150, 1, userId),
                new(3, 151, 1, userId),
                new(3, 152, 1, userId),
                new(3, 153, 1, userId),
                new(3, 154, 1, userId),
                new(3, 155, 1, userId),
                new(3, 156, 1, userId),
                new(3, 157, 1, userId),
                new(3, 158, 1, userId),
                new(3, 159, 1, userId),
                new(3, 160, 1, userId),
                new(3, 161, 1, userId),
                new(3, 162, 1, userId),
                new(3, 163, 1, userId),
                new(3, 164, 1, userId),
                new(3, 165, 1, userId),
                new(3, 166, 1, userId),
                new(3, 167, 1, userId),
                new(3, 168, 1, userId),
                new(3, 169, 1, userId),
                new(3, 170, 1, userId),
                new(3, 171, 1, userId),
                new(3, 172, 1, userId),
                new(3, 173, 1, userId),
                new(3, 174, 1, userId),
                new(3, 175, 1, userId),
                new(3, 176, 1, userId),
                new(3, 177, 1, userId),
                new(3, 178, 1, userId),
                new(3, 179, 1, userId),
                new(3, 180, 1, userId),
                new(3, 181, 1, userId),
                new(3, 182, 1, userId),
                new(3, 183, 1, userId),
                new(3, 184, 1, userId),
                new(3, 185, 1, userId),
                new(3, 186, 1, userId),
                new(3, 187, 1, userId),
                new(3, 188, 1, userId),
                new(3, 189, 1, userId),
                new(3, 190, 1, userId),
                new(4, 191, 1, userId),
                new(4, 192, 1, userId),
                new(4, 193, 1, userId),
                new(4, 194, 1, userId),
                new(4, 195, 1, userId),
                new(4, 196, 1, userId),
                new(4, 197, 1, userId),
                new(4, 198, 1, userId),
                new(4, 199, 1, userId),
                new(4, 200, 1, userId),
                new(4, 201, 1, userId),
                new(4, 202, 1, userId),
                new(4, 203, 1, userId),
                new(4, 204, 1, userId),
                new(4, 205, 1, userId),
                new(4, 206, 1, userId),
                new(4, 207, 1, userId),
                new(4, 208, 1, userId),
                new(4, 209, 1, userId),
                new(4, 210, 1, userId),
                new(4, 211, 1, userId),
                new(4, 212, 1, userId),
                new(4, 213, 1, userId),
                new(4, 214, 1, userId),
                new(4, 215, 1, userId),
                new(4, 216, 1, userId),
                new(4, 217, 1, userId),
                new(4, 218, 1, userId),
                new(4, 219, 1, userId),
                new(4, 220, 1, userId),
                new(4, 221, 1, userId),
                new(4, 222, 1, userId),
                new(4, 223, 1, userId),
                new(4, 224, 1, userId),
                new(4, 225, 1, userId),
                new(4, 226, 1, userId),
                new(4, 227, 1, userId),
                new(4, 228, 1, userId),
                new(4, 229, 1, userId),
                new(4, 230, 1, userId),
                new(4, 231, 1, userId),
                new(4, 232, 1, userId),
                new(4, 233, 1, userId),
                new(4, 234, 1, userId),
                new(4, 235, 1, userId),
                new(4, 236, 1, userId),
                new(4, 237, 1, userId),
                new(4, 238, 1, userId),
                new(4, 239, 1, userId),
                new(4, 240, 1, userId),
                new(4, 241, 1, userId),
                new(4, 242, 1, userId),
                new(4, 243, 1, userId),
                new(4, 244, 1, userId),
                new(4, 245, 1, userId),
                new(4, 246, 1, userId),
                new(4, 247, 1, userId),
                new(4, 248, 1, userId),
                new(4, 249, 1, userId),
                new(4, 250, 1, userId),
                new(4, 251, 1, userId),
                new(4, 252, 1, userId),
                new(4, 253, 1, userId),
                new(4, 254, 1, userId),
                new(4, 255, 1, userId),
                new(4, 256, 1, userId),
                new(4, 257, 1, userId),
                new(4, 258, 1, userId),
                new(4, 259, 1, userId),
                new(4, 260, 1, userId),
                new(5, 261, 1, userId),
                new(5, 262, 1, userId),
                new(5, 263, 1, userId),
                new(5, 264, 1, userId),
                new(5, 265, 1, userId),
                new(5, 266, 1, userId),
                new(5, 267, 1, userId),
                new(5, 268, 1, userId),
                new(5, 269, 1, userId),
                new(5, 270, 1, userId),
                new(5, 271, 1, userId),
                new(5, 272, 1, userId),
                new(5, 273, 1, userId),
                new(5, 274, 1, userId),
                new(5, 275, 1, userId),
                new(5, 276, 1, userId),
                new(5, 277, 1, userId),
                new(5, 278, 1, userId),
                new(5, 279, 1, userId),
                new(5, 280, 1, userId),
                new(5, 281, 1, userId),
                new(5, 282, 1, userId),
                new(5, 283, 1, userId),
                new(5, 284, 1, userId),
                new(5, 285, 1, userId),
                new(5, 286, 1, userId),
                new(5, 287, 1, userId),
                new(5, 288, 1, userId),
                new(5, 289, 1, userId),
                new(5, 290, 1, userId),
                new(5, 291, 1, userId),
                new(5, 292, 1, userId),
                new(5, 293, 1, userId),
                new(5, 294, 1, userId),
                new(5, 295, 1, userId),
                new(5, 296, 1, userId),
                new(5, 297, 1, userId),
                new(5, 298, 1, userId),
                new(5, 299, 1, userId),
                new(5, 300, 1, userId),
                new(5, 301, 1, userId),
                new(5, 302, 1, userId),
                new(5, 303, 1, userId),
                new(5, 304, 1, userId),
                new(5, 305, 1, userId),
                new(5, 306, 1, userId),
                new(5, 307, 1, userId),
                new(5, 308, 1, userId),
                new(5, 309, 1, userId),
                new(5, 310, 1, userId),
                new(5, 311, 1, userId),
                new(5, 312, 1, userId),
                new(5, 313, 1, userId),
                new(5, 314, 1, userId),
                new(5, 315, 1, userId),
                new(5, 316, 1, userId),
                new(5, 317, 1, userId),
                new(1, 318, 1, userId),
                new(5, 319, 1, userId),
                new(5, 320, 1, userId),
                new(5, 321, 1, userId),
                new(5, 322, 1, userId),
                new(5, 323, 1, userId),
                new(5, 324, 1, userId),
                new(5, 325, 1, userId),
                new(5, 326, 1, userId),
                new(5, 327, 1, userId),
                new(5, 328, 1, userId),
                new(5, 329, 1, userId),
                new(5, 330, 1, userId),
                new(6, 331, 1, userId),
                new(6, 332, 1, userId),
                new(6, 333, 1, userId),
                new(6, 334, 1, userId),
                new(6, 335, 1, userId),
                new(6, 336, 1, userId),
                new(6, 337, 1, userId),
                new(6, 338, 1, userId),
                new(6, 339, 1, userId),
                new(6, 340, 1, userId),
                new(6, 341, 1, userId),
                new(6, 342, 1, userId),
                new(6, 343, 1, userId),
                new(6, 344, 1, userId),
                new(6, 345, 1, userId),
                new(6, 346, 1, userId),
                new(6, 347, 1, userId),
                new(6, 348, 1, userId),
                new(6, 349, 1, userId),
                new(6, 350, 1, userId),
                new(6, 351, 1, userId),
                new(6, 352, 1, userId),
                new(6, 353, 1, userId),
                new(6, 354, 1, userId),
                new(6, 355, 1, userId),
                new(6, 356, 1, userId),
                new(6, 357, 1, userId),
                new(6, 358, 1, userId),
                new(6, 359, 1, userId),
                new(6, 360, 1, userId),
                new(6, 361, 1, userId),
                new(6, 362, 1, userId),
                new(6, 363, 1, userId),
                new(6, 364, 1, userId),
                new(6, 365, 1, userId),
                new(6, 366, 1, userId),
                new(6, 367, 1, userId),
                new(6, 368, 1, userId),
                new(6, 369, 1, userId),
                new(6, 370, 1, userId),
                new(6, 371, 1, userId),
                new(6, 372, 1, userId),
                new(6, 373, 1, userId),
                new(6, 374, 1, userId),
                new(6, 375, 1, userId),
                new(6, 376, 1, userId),
                new(6, 377, 1, userId),
                new(6, 378, 1, userId),
                new(6, 379, 1, userId),
                new(6, 380, 1, userId),
                new(6, 381, 1, userId),
                new(6, 382, 1, userId),
                new(6, 383, 1, userId),
                new(6, 384, 1, userId),
                new(6, 385, 1, userId),
                new(6, 386, 1, userId),
                new(6, 387, 1, userId),
                new(6, 388, 1, userId),
                new(6, 389, 1, userId),
                new(6, 390, 1, userId),
                new(6, 391, 1, userId),
                new(6, 392, 1, userId),
                new(2, 393, 1, userId),
                new(6, 394, 1, userId),
                new(6, 395, 1, userId),
                new(6, 396, 1, userId),
                new(6, 397, 1, userId),
                new(6, 398, 1, userId),
                new(6, 399, 1, userId),
                new(6, 400, 1, userId),
                new(7, 401, 1, userId),
                new(7, 402, 1, userId),
                new(7, 403, 1, userId),
                new(7, 404, 1, userId),
                new(7, 405, 1, userId),
                new(7, 406, 1, userId),
                new(7, 407, 1, userId),
                new(7, 408, 1, userId),
                new(7, 409, 1, userId),
                new(7, 410, 1, userId),
                new(7, 411, 1, userId),
                new(7, 412, 1, userId),
                new(7, 413, 1, userId),
                new(7, 414, 1, userId),
                new(7, 415, 1, userId),
                new(7, 416, 1, userId),
                new(7, 417, 1, userId),
                new(7, 418, 1, userId),
                new(7, 419, 1, userId),
                new(7, 420, 1, userId),
                new(7, 421, 1, userId),
                new(7, 422, 1, userId),
                new(7, 423, 1, userId),
                new(7, 424, 1, userId),
                new(7, 425, 1, userId),
                new(7, 426, 1, userId),
                new(7, 427, 1, userId),
                new(7, 428, 1, userId),
                new(7, 429, 1, userId),
                new(7, 430, 1, userId),
                new(7, 431, 1, userId),
                new(7, 432, 1, userId),
                new(7, 433, 1, userId),
                new(7, 434, 1, userId),
                new(7, 435, 1, userId),
                new(7, 436, 1, userId),
                new(7, 437, 1, userId),
                new(7, 438, 1, userId),
                new(7, 439, 1, userId),
                new(7, 440, 1, userId),
                new(7, 441, 1, userId),
                new(7, 442, 1, userId),
                new(7, 443, 1, userId),
                new(7, 444, 1, userId),
                new(7, 445, 1, userId),
                new(7, 446, 1, userId),
                new(7, 447, 1, userId),
                new(7, 448, 1, userId),
                new(7, 449, 1, userId),
                new(7, 450, 1, userId),
                new(7, 451, 1, userId),
                new(7, 452, 1, userId),
                new(7, 453, 1, userId),
                new(7, 454, 1, userId),
                new(7, 455, 1, userId),
                new(7, 456, 1, userId),
                new(7, 457, 1, userId),
                new(7, 458, 1, userId),
                new(7, 459, 1, userId),
                new(7, 460, 1, userId),
                new(7, 461, 1, userId),
                new(7, 462, 1, userId),
                new(7, 463, 1, userId),
                new(7, 464, 1, userId),
                new(7, 465, 1, userId),
                new(7, 466, 1, userId),
                new(7, 467, 1, userId),
                new(7, 468, 1, userId),
                new(7, 469, 1, userId),
                new(7, 470, 1, userId),
                new(8, 471, 1, userId),
                new(8, 472, 1, userId),
                new(8, 473, 1, userId),
                new(8, 474, 1, userId),
                new(8, 475, 1, userId),
                new(8, 476, 1, userId),
                new(8, 477, 1, userId),
                new(8, 478, 1, userId),
                new(8, 479, 1, userId),
                new(8, 480, 1, userId),
                new(8, 481, 1, userId),
                new(8, 482, 1, userId),
                new(8, 483, 1, userId),
                new(8, 484, 1, userId),
                new(8, 485, 1, userId),
                new(8, 486, 1, userId),
                new(8, 487, 1, userId),
                new(8, 488, 1, userId),
                new(8, 489, 1, userId),
                new(8, 490, 1, userId),
                new(8, 491, 1, userId),
                new(8, 492, 1, userId),
                new(8, 493, 1, userId),
                new(8, 494, 1, userId),
                new(8, 495, 1, userId),
                new(8, 496, 1, userId),
                new(8, 497, 1, userId),
                new(8, 498, 1, userId),
                new(8, 499, 1, userId),
                new(8, 500, 1, userId),
                new(8, 501, 1, userId),
                new(8, 502, 1, userId),
                new(8, 503, 1, userId),
                new(8, 504, 1, userId),
                new(8, 505, 1, userId),
                new(8, 506, 1, userId),
                new(8, 507, 1, userId),
                new(8, 508, 1, userId),
                new(8, 509, 1, userId),
                new(8, 510, 1, userId),
                new(8, 511, 1, userId),
                new(8, 512, 1, userId),
                new(8, 513, 1, userId),
                new(8, 514, 1, userId),
                new(8, 515, 1, userId),
                new(8, 516, 1, userId),
                new(8, 517, 1, userId),
                new(8, 518, 1, userId),
                new(8, 519, 1, userId),
                new(8, 520, 1, userId),
                new(8, 521, 1, userId),
                new(8, 522, 1, userId),
                new(8, 523, 1, userId),
                new(8, 524, 1, userId),
                new(8, 525, 1, userId),
                new(8, 526, 1, userId),
                new(8, 527, 1, userId),
                new(8, 528, 1, userId),
                new(8, 529, 1, userId),
                new(8, 530, 1, userId),
                new(8, 531, 1, userId),
                new(8, 532, 1, userId),
                new(8, 533, 1, userId),
                new(8, 534, 1, userId),
                new(8, 535, 1, userId),
                new(8, 536, 1, userId),
                new(8, 537, 1, userId),
                new(8, 538, 1, userId),
                new(8, 539, 1, userId),
                new(8, 540, 1, userId),
                new(9, 1, 1, userId),
                new(9, 2, 1, userId),
                new(9, 3, 1, userId),
                new(9, 4, 1, userId),
                new(9, 5, 1, userId),
                new(9, 6, 1, userId),
                new(9, 7, 1, userId),
                new(9, 8, 1, userId),
                new(9, 9, 1, userId),
                new(9, 10, 1, userId),
                new(9, 11, 1, userId),
                new(9, 12, 1, userId),
                new(9, 13, 1, userId),
                new(9, 14, 1, userId),
                new(9, 15, 1, userId),
                new(9, 16, 1, userId),
                new(9, 17, 1, userId),
                new(9, 18, 1, userId),
                new(9, 19, 1, userId),
                new(9, 20, 1, userId),
                new(9, 21, 1, userId),
                new(9, 22, 1, userId),
                new(9, 23, 1, userId),
                new(9, 24, 1, userId),
                new(9, 25, 1, userId),
                new(9, 26, 1, userId),
                new(9, 27, 1, userId),
                new(9, 28, 1, userId),
                new(9, 29, 1, userId),
                new(9, 30, 1, userId),
                new(9, 31, 1, userId),
                new(9, 32, 1, userId),
                new(9, 33, 1, userId),
                new(9, 34, 1, userId),
                new(9, 35, 1, userId),
                new(9, 36, 1, userId),
                new(9, 37, 1, userId),
                new(9, 38, 1, userId),
                new(9, 39, 1, userId),
                new(9, 40, 1, userId),
                new(9, 41, 1, userId),
                new(9, 42, 1, userId),
                new(9, 43, 1, userId),
                new(9, 44, 1, userId),
                new(9, 45, 1, userId),
                new(9, 46, 1, userId),
                new(9, 47, 1, userId),
                new(9, 48, 1, userId),
                new(9, 49, 1, userId),
                new(9, 50, 1, userId),
                new(9, 51, 1, userId),
                new(9, 52, 1, userId),
                new(9, 53, 1, userId),
                new(9, 54, 1, userId),
                new(9, 55, 1, userId),
                new(9, 56, 1, userId),
                new(9, 57, 1, userId),
                new(9, 58, 1, userId),
                new(9, 59, 1, userId),
                new(9, 60, 1, userId),
                new(9, 61, 1, userId),
                new(9, 62, 1, userId),
                new(9, 63, 1, userId),
                new(9, 64, 1, userId),
                new(9, 65, 1, userId),
                new(9, 66, 1, userId),
                new(9, 67, 1, userId),
                new(9, 68, 1, userId),
                new(9, 69, 1, userId),
                new(9, 70, 1, userId),
                new(10, 71, 1, userId),
                new(10, 72, 1, userId),
                new(10, 73, 1, userId),
                new(10, 74, 1, userId),
                new(10, 75, 1, userId),
                new(10, 76, 1, userId),
                new(10, 77, 1, userId),
                new(10, 78, 1, userId),
                new(10, 79, 1, userId),
                new(10, 80, 1, userId),
                new(10, 81, 1, userId),
                new(10, 82, 1, userId),
                new(10, 83, 1, userId),
                new(10, 84, 1, userId),
                new(10, 85, 1, userId),
                new(10, 86, 1, userId),
                new(10, 87, 1, userId),
                new(10, 88, 1, userId),
                new(10, 89, 1, userId),
                new(10, 90, 1, userId),
                new(10, 91, 1, userId),
                new(10, 92, 1, userId),
                new(10, 93, 1, userId),
                new(10, 94, 1, userId),
                new(10, 95, 1, userId),
                new(10, 96, 1, userId),
                new(10, 97, 1, userId),
                new(10, 98, 1, userId),
                new(10, 99, 1, userId),
                new(10, 100, 1, userId),
                new(10, 101, 1, userId),
                new(10, 102, 1, userId),
                new(10, 103, 1, userId),
                new(10, 104, 1, userId),
                new(10, 105, 1, userId),
                new(10, 106, 1, userId),
                new(10, 107, 1, userId),
                new(10, 108, 1, userId),
                new(10, 109, 1, userId),
                new(10, 110, 1, userId),
                new(10, 111, 1, userId),
                new(10, 112, 1, userId),
                new(10, 113, 1, userId),
                new(10, 114, 1, userId),
                new(10, 115, 1, userId),
                new(10, 116, 1, userId),
                new(10, 117, 1, userId),
                new(10, 118, 1, userId),
                new(10, 119, 1, userId),
                new(10, 120, 1, userId),
                new(10, 121, 1, userId),
                new(10, 122, 1, userId),
                new(10, 123, 1, userId),
                new(10, 124, 1, userId),
                new(10, 125, 1, userId),
                new(10, 126, 1, userId),
                new(10, 127, 1, userId),
                new(10, 128, 1, userId),
                new(10, 129, 1, userId),
                new(10, 130, 1, userId),
                new(10, 131, 1, userId),
                new(10, 132, 1, userId),
                new(10, 133, 1, userId),
                new(10, 134, 1, userId),
                new(10, 135, 1, userId),
                new(10, 136, 1, userId),
                new(10, 137, 1, userId),
                new(10, 138, 1, userId),
                new(10, 139, 1, userId),
                new(10, 140, 1, userId),
                new(11, 141, 1, userId),
                new(11, 142, 1, userId),
                new(11, 143, 1, userId),
                new(11, 144, 1, userId),
                new(11, 145, 1, userId),
                new(11, 146, 1, userId),
                new(11, 147, 1, userId),
                new(11, 148, 1, userId),
                new(11, 149, 1, userId),
                new(11, 150, 1, userId),
                new(11, 151, 1, userId),
                new(11, 152, 1, userId),
                new(11, 153, 1, userId),
                new(11, 154, 1, userId),
                new(11, 155, 1, userId),
                new(11, 156, 1, userId),
                new(11, 157, 1, userId),
                new(11, 158, 1, userId),
                new(11, 159, 1, userId),
                new(11, 160, 1, userId),
                new(11, 161, 1, userId),
                new(11, 162, 1, userId),
                new(11, 163, 1, userId),
                new(11, 164, 1, userId),
                new(11, 165, 1, userId),
                new(11, 166, 1, userId),
                new(11, 167, 1, userId),
                new(11, 168, 1, userId),
                new(11, 169, 1, userId),
                new(11, 170, 1, userId),
                new(11, 171, 1, userId),
                new(11, 172, 1, userId),
                new(11, 173, 1, userId),
                new(11, 174, 1, userId),
                new(11, 175, 1, userId),
                new(11, 176, 1, userId),
                new(11, 177, 1, userId),
                new(11, 178, 1, userId),
                new(11, 179, 1, userId),
                new(11, 180, 1, userId),
                new(11, 181, 1, userId),
                new(11, 182, 1, userId),
                new(11, 183, 1, userId),
                new(11, 184, 1, userId),
                new(11, 185, 1, userId),
                new(11, 186, 1, userId),
                new(11, 187, 1, userId),
                new(11, 188, 1, userId),
                new(11, 189, 1, userId),
                new(11, 190, 1, userId),
                new(11, 191, 1, userId),
                new(11, 192, 1, userId),
                new(11, 193, 1, userId),
                new(11, 194, 1, userId),
                new(11, 195, 1, userId),
                new(11, 196, 1, userId),
                new(11, 197, 1, userId),
                new(11, 198, 1, userId),
                new(11, 199, 1, userId),
                new(11, 200, 1, userId),
                new(11, 201, 1, userId),
                new(11, 202, 1, userId),
                new(11, 203, 1, userId),
                new(11, 204, 1, userId),
                new(11, 205, 1, userId),
                new(11, 206, 1, userId),
                new(11, 207, 1, userId),
                new(11, 208, 1, userId),
                new(11, 209, 1, userId),
                new(11, 210, 1, userId),
                new(12, 1, 1, userId),
                new(12, 2, 1, userId),
                new(12, 3, 1, userId),
                new(12, 4, 1, userId),
                new(12, 5, 1, userId),
                new(12, 6, 1, userId),
                new(12, 7, 1, userId),
                new(12, 8, 1, userId),
                new(12, 9, 1, userId),
                new(12, 10, 1, userId),
                new(12, 11, 1, userId),
                new(12, 12, 1, userId),
                new(12, 13, 1, userId),
                new(12, 14, 1, userId),
                new(12, 15, 1, userId),
                new(12, 16, 1, userId),
                new(12, 17, 1, userId),
                new(12, 18, 1, userId),
                new(12, 19, 1, userId),
                new(12, 20, 1, userId),
                new(12, 21, 1, userId),
                new(12, 22, 1, userId),
                new(12, 23, 1, userId),
                new(12, 24, 1, userId),
                new(12, 25, 1, userId),
                new(12, 26, 1, userId),
                new(12, 27, 1, userId),
                new(12, 28, 1, userId),
                new(12, 29, 1, userId),
                new(12, 30, 1, userId),
                new(12, 31, 1, userId),
                new(12, 32, 1, userId),
                new(12, 33, 1, userId),
                new(12, 34, 1, userId),
                new(12, 35, 1, userId),
                new(12, 36, 1, userId),
                new(12, 37, 1, userId),
                new(12, 38, 1, userId),
                new(12, 39, 1, userId),
                new(12, 40, 1, userId),
                new(12, 41, 1, userId),
                new(12, 42, 1, userId),
                new(12, 43, 1, userId),
                new(12, 44, 1, userId),
                new(12, 45, 1, userId),
                new(12, 46, 1, userId),
                new(12, 47, 1, userId),
                new(12, 48, 1, userId),
                new(12, 49, 1, userId),
                new(12, 50, 1, userId),
                new(12, 51, 1, userId),
                new(12, 52, 1, userId),
                new(12, 53, 1, userId),
                new(12, 54, 1, userId),
                new(12, 55, 1, userId),
                new(12, 56, 1, userId),
                new(12, 57, 1, userId),
                new(12, 58, 1, userId),
                new(12, 59, 1, userId),
                new(12, 60, 1, userId),
                new(12, 61, 1, userId),
                new(12, 62, 1, userId),
                new(12, 63, 1, userId),
                new(12, 64, 1, userId),
                new(12, 65, 1, userId),
                new(12, 66, 1, userId),
                new(12, 67, 1, userId),
                new(12, 68, 1, userId),
                new(12, 69, 1, userId),
                new(12, 70, 1, userId),
                new(13, 71, 1, userId),
                new(13, 72, 1, userId),
                new(13, 73, 1, userId),
                new(13, 74, 1, userId),
                new(13, 75, 1, userId),
                new(13, 76, 1, userId),
                new(13, 77, 1, userId),
                new(13, 78, 1, userId),
                new(13, 79, 1, userId),
                new(13, 80, 1, userId),
                new(13, 81, 1, userId),
                new(13, 82, 1, userId),
                new(13, 83, 1, userId),
                new(13, 84, 1, userId),
                new(13, 85, 1, userId),
                new(13, 86, 1, userId),
                new(13, 87, 1, userId),
                new(13, 88, 1, userId),
                new(13, 89, 1, userId),
                new(13, 90, 1, userId),
                new(13, 91, 1, userId),
                new(13, 92, 1, userId),
                new(13, 93, 1, userId),
                new(13, 94, 1, userId),
                new(13, 95, 1, userId),
                new(13, 96, 1, userId),
                new(13, 97, 1, userId),
                new(13, 98, 1, userId),
                new(13, 99, 1, userId),
                new(13, 100, 1, userId),
                new(13, 101, 1, userId),
                new(13, 102, 1, userId),
                new(13, 103, 1, userId),
                new(13, 104, 1, userId),
                new(13, 105, 1, userId),
                new(13, 106, 1, userId),
                new(13, 107, 1, userId),
                new(13, 108, 1, userId),
                new(13, 109, 1, userId),
                new(13, 110, 1, userId),
                new(13, 111, 1, userId),
                new(13, 112, 1, userId),
                new(13, 113, 1, userId),
                new(13, 114, 1, userId),
                new(13, 115, 1, userId),
                new(13, 116, 1, userId),
                new(13, 117, 1, userId),
                new(13, 118, 1, userId),
                new(13, 119, 1, userId),
                new(13, 120, 1, userId),
                new(13, 121, 1, userId),
                new(13, 122, 1, userId),
                new(13, 123, 1, userId),
                new(13, 124, 1, userId),
                new(13, 125, 1, userId),
                new(13, 126, 1, userId),
                new(13, 127, 1, userId),
                new(13, 128, 1, userId),
                new(13, 129, 1, userId),
                new(13, 130, 1, userId),
                new(13, 131, 1, userId),
                new(13, 132, 1, userId),
                new(13, 133, 1, userId),
                new(13, 134, 1, userId),
                new(13, 135, 1, userId),
                new(13, 136, 1, userId),
                new(13, 137, 1, userId),
                new(13, 138, 1, userId),
                new(13, 139, 1, userId),
                new(13, 140, 1, userId),
            };
        }

        public void CreateNewTag(TagViewModel model)
        {
            Tag tag = new Tag(model.Name);
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();
        }
    }
}
