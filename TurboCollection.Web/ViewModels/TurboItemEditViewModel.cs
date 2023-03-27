using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TurboCollection.Web.ViewModels
{
    public class TurboItemEditViewModel
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public string Name { get; set; }

        [Display(Name = "Collection")]
        public List<SelectListItem>? Collections { get; set; }

        [Display(Name = "Number")]
        public int Number { get; set; }
        public string PictureUrl { get; set; }

        [Display(Name = "Picture")]
        public IFormFile Picture { get; set; }

        public List<SelectListItem> Tags { get; set; }
        public int[] TagIds { get; set; }
    }
}
