using System.ComponentModel.DataAnnotations;

namespace Музыкальный_портал__Music_portal_.Models
{
    public class Login_Model
    {
        [Required(ErrorMessage = "Поле Логин является обязательным.")]
        [Display(Name = "Логин:")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Поле Пароль является обязательным.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string? Password { get; set; }
    }
}
