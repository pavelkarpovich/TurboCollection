using Microsoft.AspNetCore.Mvc.Rendering;
using TurboCollection.ApplicationCore.Entities;

namespace TurboCollection.Web.ViewModels
{
    public class PrivateTurboItemsViewModel
    {
        public List<PrivateTurboItemViewModel> Items { get; set; }
        public List<SelectListItem>? Collections { get; set; }
        public int? CollectionFilerApplied { get; set; }
        public UserViewModel User { get; set; }
    }
}
