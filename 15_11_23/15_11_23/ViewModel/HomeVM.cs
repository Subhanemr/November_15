using _15_11_23.Models;

namespace _15_11_23.ModelsVM
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Slide> Slides { get; set; }
        public List<Client> Clients { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Settings> Settings { get; set; }

    }
}