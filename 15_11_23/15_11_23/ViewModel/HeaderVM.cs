using _15_11_23.Models;

namespace _15_11_23.ViewModel
{
    public class HeaderVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public List<CartItemVM> Items { get; set; }
        public AppUser? User { get; set; }
    }
}
