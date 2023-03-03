namespace TurboCollection.Web.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(string userId, string firstName, string lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
