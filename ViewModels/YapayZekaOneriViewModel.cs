using Microsoft.AspNetCore.Http; // Resim yükleme için gerekli
using System.ComponentModel.DataAnnotations;

namespace webOdev.ViewModels
{
    public class YapayZekaOneriViewModel
    {
        [Required(ErrorMessage = "Lütfen boyunuzu girin (cm).")]
        [Display(Name = "Boyunuz (cm)")]
        public int Boy { get; set; }

        [Required(ErrorMessage = "Lütfen kilonuzu girin (kg).")]
        [Display(Name = "Kilonuz (kg)")]
        public int Kilo { get; set; }

        [Required(ErrorMessage = "Lütfen bir hedef girin.")]
        [Display(Name = "Ana Hedefiniz")]
        public string Hedef { get; set; }

        // YENİ EKLENEN KISIM: Fotoğraf Yükleme
        [Display(Name = "Vücut Fotoğrafınız (İsteğe Bağlı)")]
        public IFormFile? Fotograf { get; set; }
    }
}