using System.ComponentModel.DataAnnotations;

namespace _15_11_23.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "User Name must be entered mutled")]
        [MinLength(1, ErrorMessage = "It should not exceed 1-255 characters")]
        [MaxLength(255, ErrorMessage = "It should not exceed 1-255 characters")]
        public string UserNameOrEmail { get; set; }
        [Required(ErrorMessage = "Password must be entered mutled")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
    }
}
