using System.ComponentModel.DataAnnotations;

namespace _15_11_23.Areas.ProniaAdmin.ViewModels
{
    public class CreateBlogVM
    {
        [Required(ErrorMessage = "Title must be entered mutled")]
        [MaxLength(25, ErrorMessage = "It should not exceed 25 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description must be entered mutled")]
        [MaxLength(100, ErrorMessage = "It should not exceed 100 characters")]
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        [Required(ErrorMessage = "ByWho must be entered mutled")]
        [MaxLength(25, ErrorMessage = "It should not exceed 25 characters")]
        public string ByWho { get; set; }
        [Required(ErrorMessage = "Photo must be entered mutled")]
        public IFormFile Photo { get; set; }
    }
}
