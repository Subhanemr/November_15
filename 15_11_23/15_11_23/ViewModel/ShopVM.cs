using _15_11_23.Models;

namespace _15_11_23.ViewModel
{
    public class ShopVM
    {

        public int? Order { get; set; }
        public int? CategoryId { get; set; }
        public string? Search { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
