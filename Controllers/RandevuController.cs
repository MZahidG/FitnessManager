using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webOdev.Data;
using webOdev.Models;

namespace webOdev.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RandevuController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Kullanıcının dersleri gördüğü sayfa
        public async Task<IActionResult> Index()
        {
            // DÜZELTME BURADA YAPILDI:
            // DateTime.Today (Yerel Saat) yerine DateTime.UtcNow.Date (Evrensel Saat) kullanıldı.
            // Bu sayede PostgreSQL hatası giderildi.
            var bugunUtc = DateTime.UtcNow.Date;

            var musaitSeanslar = await _context.DersSeanslari
                .Include(s => s.Antrenor)
                .Where(s => s.DoluMu == false && s.Tarih >= bugunUtc)
                .OrderBy(s => s.Tarih)
                .ToListAsync();

            return View(musaitSeanslar);
        }

        // Randevu Talebi Oluşturma (POST)
        [HttpPost]
        public async Task<IActionResult> TalepOlustur(int seansId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // Daha önce bu seansa talep atmış mı kontrol et
            bool zatenVar = await _context.Randevular.AnyAsync(r => r.DersSeansId == seansId && r.UyeId == user.Id);
            if (zatenVar)
            {
                TempData["Mesaj"] = "Bu derse zaten talebiniz var.";
                return RedirectToAction("Index");
            }

            var randevu = new Randevu
            {
                DersSeansId = seansId,
                UyeId = user.Id,
                OnayDurumu = false, // Onay bekliyor
                OlusturulmaTarihi = DateTime.UtcNow // Burayı da UTC yaptık ki tutarlı olsun
            };

            _context.Add(randevu);
            await _context.SaveChangesAsync();

            TempData["Basari"] = "Randevu talebiniz hocaya iletildi!";
            return RedirectToAction("Index");
        }
    }
}