using System.ComponentModel.DataAnnotations;

namespace DepartmanProjem.Models
{
    public class Birim
    {
        [Key]
        public int BirimId { get; set; }

        [Required(ErrorMessage = "Birim adı zorunludur")]
        [StringLength(100, ErrorMessage = "Birim adı en fazla 100 karakter olabilir")]
        [Display(Name = "Birim Adı")]
        public string BirimAd { get; set; } = string.Empty;

        // Navigation Property - Bir birime ait tüm personeller
        public virtual ICollection<Personel> Personeller { get; set; } = new List<Personel>();
    }
}
