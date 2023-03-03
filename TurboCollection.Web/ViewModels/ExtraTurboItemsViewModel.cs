using Microsoft.AspNetCore.Mvc.Rendering;

namespace TurboCollection.Web.ViewModels
{
    public class ExtraTurboItemsViewModel
    {
        public List<ExtraTurboItemViewModel> Items { get; set; }
        public UserViewModel User { get; set; }
    }
}
