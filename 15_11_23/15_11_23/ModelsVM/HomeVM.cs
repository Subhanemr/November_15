using _15_11_23.Models;

namespace _15_11_23.ModelsVM
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Slide> Slides { get; set; }

        public HomeVM(List<Product> products, List<Slide> slides)
        {
            Products = products;
            Slides = slides;
        }
    }
}
