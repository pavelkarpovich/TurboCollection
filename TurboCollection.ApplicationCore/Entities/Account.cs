using Microsoft.AspNetCore.Identity;

namespace TurboCollection.ApplicationCore.Entities
{
    public sealed class Account : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string PictureUrl { get; set; }
    }
}
