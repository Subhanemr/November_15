using _15_11_23.Models;
using System.ComponentModel.DataAnnotations;

namespace _15_11_23.ViewModel
{
    public class OrderVM
    {
        [Required(ErrorMessage ="Please enter your address")]
        public string Address{ get; set; }
        public List<BasketItem>? BasketItems { get; set; }

    }
}
