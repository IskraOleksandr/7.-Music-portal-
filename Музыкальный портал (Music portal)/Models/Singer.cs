using System.ComponentModel.DataAnnotations;

namespace Музыкальный_портал__Music_portal_.Models
{
    public class Singer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле имя исполнителя должно быть установлено.")]
        [Display(Name = "Имя исполнителя")]
        public string? SingerName { get; set; }

        ICollection<Music> Musics { get; set; }
    }
}
