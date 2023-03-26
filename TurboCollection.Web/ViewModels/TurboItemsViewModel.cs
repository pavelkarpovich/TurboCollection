using Microsoft.AspNetCore.Mvc.Rendering;

namespace TurboCollection.Web.ViewModels
{
    public class TurboItemsViewModel
    {
        public List<TurboItemViewModel> Items { get; set; }
        public List<SelectListItem>? Collections { get; set; }
        public int? CollectionFilerApplied { get; set; }
        public string? Search { get; set; }
        public List<SelectListItem> Tags { get; set; }
        public int[] TagIds { get; set; }
    }
}
