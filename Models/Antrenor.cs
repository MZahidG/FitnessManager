using System.ComponentModel.DataAnnotations;

namespace webOdev.Models
{
    public class Antrenor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        [Display(Name = "Uzmanlık Alanı")]
        public string UzmanlikAlani { get; set; }

        // İlişki: Bir hocanın birden fazla dersi olabilir.
        // Soru işareti (?) koyarak bunun boş olabileceğini belirtiyoruz (Hata 4'ün çözümü).
        public ICollection<DersSeans>? DersSeanslari { get; set; }
    }
}