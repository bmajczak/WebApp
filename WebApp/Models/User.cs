using Microsoft.AspNetCore.Identity;

namespace WebApp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public byte[]? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public string? Interests { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}