namespace TurboCollection.Web.ViewModels
{
    public class ChatViewModel
    {
        public List<ChatPostViewModel> ChatPosts { get; set; }
        public UserViewModel User { get; set; }
        public UserViewModel ChatWithUser { get; set; }
    }
}
