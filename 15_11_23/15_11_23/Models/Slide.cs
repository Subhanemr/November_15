using System.ComponentModel.DataAnnotations.Schema;

namespace _15_11_23.Models
{
    public class Slide
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}