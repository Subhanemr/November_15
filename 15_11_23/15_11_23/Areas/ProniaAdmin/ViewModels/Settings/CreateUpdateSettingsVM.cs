using System.ComponentModel.DataAnnotations;

namespace _15_11_23.Areas.ProniaAdmin.ViewModels
{
    public class CreateUpdateSettingsVM
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
