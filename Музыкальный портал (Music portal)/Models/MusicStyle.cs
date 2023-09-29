using System.ComponentModel.DataAnnotations;

namespace Музыкальный_портал__Music_portal_.Models
{
    public class MusicStyle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле стиля должно быть установлено.")]
        [Display(Name = "Название стиля")]
        public string? StyleName { get; set; }

        ICollection<Music> Musics { get; set; }
    }
} 
