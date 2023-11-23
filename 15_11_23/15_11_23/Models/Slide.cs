using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _15_11_23.Models
{
    public class Slide
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name must be entered mutled")]
        [MaxLength(25, ErrorMessage = "It should not exceed 25 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Name must be entered mutled")]
        [MaxLength(50, ErrorMessage = "It should not exceed 25 characters")]
        public string SubTitle { get; set; }
        [Required(ErrorMessage = "Name must be entered mutled")]
        [MaxLength(100, ErrorMessage = "It should not exceed 25 characters")]
        public string Description { get; set; }
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}