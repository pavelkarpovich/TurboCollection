namespace TurboCollection.Web.ViewModels
{
    public class UserEditViewModel
    {
        public UserEditViewModel()
        {
        }

        public UserEditViewModel(string userId, string firstName, string lastName, string pictureUrl)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PictureUrl = pictureUrl;
        }

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile Picture { get; set; }
        public string PictureUrl { get; set; }
    }
}
