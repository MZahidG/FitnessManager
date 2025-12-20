using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webOdev.Models
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;
        public bool OnayDurumu { get; set; } = false;

        public int DersSeansId { get; set; }
        [ForeignKey("DersSeansId")]
        public DersSeans? DersSeans { get; set; }

        public string UyeId { get; set; }
        [ForeignKey("UyeId")]
        public IdentityUser? Uye { get; set; }
    }
}