namespace TurboCollection.Web.ViewModels
{
    public class ExtraTurboItemSearchResultViewModel
    {
        public ExtraTurboItemSearchResultViewModel(long id, int collectionId, int number, string pictureUrl, UserViewModel userViewModel)
        {
            Id = id;
            CollectionId = collectionId;
            Number = number;
            PictureUrl = pictureUrl;
            UserViewModel = userViewModel;
        }

        public long Id { get; set; }
        public int CollectionId { get; set; }
        public int Number { get; set; }
        public string PictureUrl { get; set; }
        public UserViewModel UserViewModel { get; set; }

    }
}
