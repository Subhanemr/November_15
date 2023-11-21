namespace _15_11_23.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
    }
}
