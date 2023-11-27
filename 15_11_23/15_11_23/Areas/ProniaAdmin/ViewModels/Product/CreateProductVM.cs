using _15_11_23.Models;
using System.ComponentModel.DataAnnotations;

namespace _15_11_23.Areas.ProniaAdmin.ViewModels
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Name must be entered mutled")]
        [MaxLength(25, ErrorMessage = "It should not exceed 25 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price must be entered mutled")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0 ")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Order must be entered mutled")]
        [Range(1, int.MaxValue, ErrorMessage = "Order must be greater than 0 ")]
        public int CountId { get; set; }
        [Required(ErrorMessage = "SKU must be entered mutled")]
        public string SKU { get; set; }
        [Required(ErrorMessage = "Description must be entered mutled")]
        [MaxLength(100, ErrorMessage = "It should not exceed 25 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Category must be entered mutled")]
        [Range(1, int.MaxValue, ErrorMessage = "Category must be greater than 0 ")]
        public int? CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<Size>? Sizes { get; set; }
        public List<Color>? Colors { get; set; }
        public List<int> TagIds { get; set; }
        public List<int> ColorIds { get; set; }
        public List<int> SizeIds { get; set; }
    }
}
