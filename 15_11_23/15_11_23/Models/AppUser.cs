using _15_11_23.Utilities.Enums;
using Microsoft.AspNetCore.Identity;

namespace _15_11_23.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Genders { get; set; }
        public string Img { get; set; } = "default-profile.png";
        public List<BasketItem> BasketItems { get; set; }
    }
}
