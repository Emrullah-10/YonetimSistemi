using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepartmanProjem.Models
{
    public class Personel
    {
        [Key]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        [Display(Name = "Ad")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Birim seçimi zorunludur")]
        [Display(Name = "Birim")]
        public int BirimId { get; set; }

        // Navigation Property - Personelin bağlı olduğu birim
        [ForeignKey("BirimId")]
        public virtual Birim Birim { get; set; } = null!;
    }
}
