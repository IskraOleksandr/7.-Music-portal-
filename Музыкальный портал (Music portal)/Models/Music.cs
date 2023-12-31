using System.ComponentModel.DataAnnotations;

namespace Музыкальный_портал__Music_portal_.Models
{
    public class Music
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле название должно быть установлено.")]
        [Display(Name = "Название:")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Длина название должна быть от 3 до 25 символов")]
        public string? Video_Name { get; set; }

        [Required(ErrorMessage = "Поле альбом должно быть установлено.")]
        [Display(Name = "Альбом:")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Длина альбома должна быть от 3 до 25 символов")]
        public string? Album { get; set; }

        [Required(ErrorMessage = "Поле года выпуска должно быть установлено.")]
        [Display(Name = "Год выпуска:")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Длина го должна быть от 3 до 25 символов")]
        public string? Year { get; set; }

        [Display(Name = "Видео:")]
        [Required(ErrorMessage = "Поле файла должно быть установлено.")]
        public string? Video_URL { get; set; }

        [Display(Name = "Дата публикации:")]
        public DateTime VideoDate { get; set; }
        
        public virtual MusicStyle MusicStyle { get; set; }
         
        public virtual User User { get; set; }
        
        public virtual Singer Singer { get; set; }

        [Display(Name = "Стиль:")]
        public int MusicStyleId { get; set; }

        [Display(Name = "Пользователь:")]
        public int UserId { get; set; }

        [Display(Name = "Исполнитель:")]
        public int SingerId { get; set; }
    }
}