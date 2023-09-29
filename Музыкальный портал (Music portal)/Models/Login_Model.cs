using System.ComponentModel.DataAnnotations;

namespace Музыкальный_портал__Music_portal_.Models
{
    public class Login_Model
    {
        [Required]
        [Display(Name = "Логин:")]
        public string? Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string? Password { get; set; }
    }
}
