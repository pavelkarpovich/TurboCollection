namespace TurboCollection.Web.ViewModels
{
    public class ExtraTurboItemViewModel
    {
        public ExtraTurboItemViewModel()
        {

        }
        public ExtraTurboItemViewModel(long id, int collectionId, int number, string pictureUrl, string userId)
        {
            Id = id;
            CollectionId = collectionId;
            Number = number;
            PictureUrl = pictureUrl;
            UserId = userId;
        }

        public long Id { get; set; }
        public int CollectionId { get; set; }
        public int Number { get; set; }
        public string PictureUrl { get; set; }
        public string UserId { get; set; }
    }
}
