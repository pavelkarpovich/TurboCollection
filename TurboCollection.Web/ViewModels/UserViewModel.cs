namespace TurboCollection.Web.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {

        }
        public UserViewModel(string userId, string firstName, string lastName, string pictureUrl)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PictureUrl = pictureUrl;
        }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public bool IsNewChatPost { get; set; }
    }
}
