using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webOdev.Models
{
    public class DersSeans
    {
        [Key]
        public int Id { get; set; }

        public int AntrenorId { get; set; }
        [ForeignKey("AntrenorId")]
        public Antrenor? Antrenor { get; set; }

        [Required]
        public DateTime Tarih { get; set; } // UTC olarak tutulacak

        [Required]
        public string SaatAraligi { get; set; } // Örn: "14:00 - 15:00"

        [Required]
        public string DersAdi { get; set; } // Örn: "Pilates"

        public decimal Ucret { get; set; }

        public int Kapasite { get; set; } = 1;

        public bool DoluMu { get; set; } = false;
    }
}