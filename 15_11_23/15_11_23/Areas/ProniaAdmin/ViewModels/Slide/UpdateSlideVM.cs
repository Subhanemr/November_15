using System.ComponentModel.DataAnnotations;

namespace _15_11_23.Areas.ProniaAdmin.ViewModels
{
    public class UpdateSlideVM
    {
        [Required(ErrorMessage = "Name must be entered mutled")]
        [MaxLength(25, ErrorMessage = "It should not exceed 25 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Name must be entered mutled")]
        [MaxLength(50, ErrorMessage = "It should not exceed 50 characters")]
        public string SubTitle { get; set; }
        [Required(ErrorMessage = "Name must be entered mutled")]
        [MaxLength(100, ErrorMessage = "It should not exceed 100 characters")]
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
