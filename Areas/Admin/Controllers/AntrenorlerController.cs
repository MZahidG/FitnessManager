using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webOdev.Data;
using webOdev.Models;

namespace webOdev.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AntrenorlerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AntrenorlerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hoca Listesi
        public async Task<IActionResult> Index()
        {
            return View(await _context.Antrenorler.ToListAsync());
        }

        // Yeni Hoca Ekleme Sayfası
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Antrenor antrenor)
        {
            // BU SATIR EKLENMELİ: Listenin boş gelmesini dert etme diyoruz.
            ModelState.Remove("DersSeanslari");

            if (ModelState.IsValid)
            {
                _context.Add(antrenor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = antrenor.Id });
            }
            return View(antrenor);
        }

        // --- DÜZENLEME EKRANI (Dersler ve Randevular burada görünür) ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var antrenor = await _context.Antrenorler.FindAsync(id);
            if (antrenor == null) return NotFound();

            // 1. Hoca'nın açtığı seansları getir
            ViewBag.Seanslar = await _context.DersSeanslari
                .Where(d => d.AntrenorId == id)
                .OrderBy(d => d.Tarih)
                .ToListAsync();

            // 2. Hoca'ya gelen randevu taleplerini getir
            ViewBag.BekleyenRandevular = await _context.Randevular
                .Include(r => r.Uye)
                .Include(r => r.DersSeans)
                .Where(r => r.DersSeans.AntrenorId == id && r.OnayDurumu == false)
                .ToListAsync();

            return View(antrenor);
        }
        // Eksik olan Details Metodu
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenor = await _context.Antrenorler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (antrenor == null)
            {
                return NotFound();
            }

            return View(antrenor);
        }

        // Hoca Bilgisi Güncelleme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInfo(Antrenor model)
        {
            var hoca = await _context.Antrenorler.FindAsync(model.Id);
            if (hoca != null)
            {
                hoca.AdSoyad = model.AdSoyad;
                hoca.UzmanlikAlani = model.UzmanlikAlani;
                // Not: Antrenor modelinde SeansUcreti olmadığı için o satırı kaldırdım.
                // Ücret bilgisi artık DersSeans tablosunda tutuluyor.

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Edit", new { id = model.Id });
        }

        // YENİ SEANS EKLEME (Kapasiteli)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SeansEkle(int antrenorId, DateTime tarih, string saatAraligi, string dersAdi, decimal ucret, int kapasite)
        {
            var yeniSeans = new DersSeans
            {
                AntrenorId = antrenorId,
                Tarih = tarih.ToUniversalTime(), // Postgres için UTC ayarı
                SaatAraligi = saatAraligi,
                DersAdi = dersAdi,
                Ucret = ucret,
                Kapasite = kapasite > 0 ? kapasite : 1, // En az 1 kişi
                DoluMu = false
            };

            _context.Add(yeniSeans);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = antrenorId });
        }

        // --- RANDEVU YÖNETİMİ (Eski Controller Silindiği İçin Buraya Alındı) ---

        // 1. RANDEVU ONAYLAMA VE KAPASİTE KONTROLÜ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RandevuOnayla(int randevuId, int antrenorId)
        {
            var randevu = await _context.Randevular.FindAsync(randevuId);
            if (randevu != null)
            {
                // A. Randevuyu onayla
                randevu.OnayDurumu = true;

                // B. Seansı bul ve kapasite kontrolü yap
                var seans = await _context.DersSeanslari.FindAsync(randevu.DersSeansId);
                if (seans != null)
                {
                    // Şu anki onaylı kişi sayısını bul (+1 şu an onayladığımız için)
                    int onayliSayisi = await _context.Randevular
                        .CountAsync(r => r.DersSeansId == seans.Id && r.OnayDurumu == true);

                    // Eğer (Mevcut + Yeni) >= Kapasite ise -> DOLDU!
                    if ((onayliSayisi + 1) >= seans.Kapasite)
                    {
                        seans.DoluMu = true;
                        _context.Update(seans);
                    }
                }

                _context.Update(randevu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Edit", new { id = antrenorId });
        }

        // 2. RANDEVU REDDETME (SİLME) VE YER AÇMA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RandevuReddet(int randevuId, int antrenorId)
        {
            var randevu = await _context.Randevular.FindAsync(randevuId);
            if (randevu != null)
            {
                // Randevuyu silmeden önce seansı bul
                var seans = await _context.DersSeanslari.FindAsync(randevu.DersSeansId);

                // Randevuyu sil
                _context.Randevular.Remove(randevu);

                // Eğer seans daha önce dolduysa, bir kişi sildiğimiz için tekrar yer açıldı demektir
                if (seans != null && seans.DoluMu == true)
                {
                    seans.DoluMu = false;
                    _context.Update(seans);
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Edit", new { id = antrenorId });
        }

        // Hoca Silme İşlemleri
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var antrenor = await _context.Antrenorler.FindAsync(id);
            if (antrenor == null) return NotFound();
            return View(antrenor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var antrenor = await _context.Antrenorler.FindAsync(id);
            if (antrenor != null)
            {
                _context.Antrenorler.Remove(antrenor);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}