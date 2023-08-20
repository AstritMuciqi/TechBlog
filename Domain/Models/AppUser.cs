using Microsoft.AspNetCore.Identity;

namespace TechBlogApp.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        //public string Image { get; set; }
        public List<Article> Articles { get; set; }


    }
}
