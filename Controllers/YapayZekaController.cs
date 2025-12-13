using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using webOdev.ViewModels;

namespace webOdev.Controllers
{
    [Authorize]
    public class YapayZekaController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public YapayZekaController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult OneriAl()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OneriAl(YapayZekaOneriViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. API Anahtarını Kontrol Et
            string apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                ViewBag.Oneri = "HATA: 'Gemini:ApiKey' appsettings.json dosyasında bulunamadı. Lütfen API anahtarınızı ekleyin.";
                return View(model);
            }

            // 2. FOTOĞRAF İŞLEME
            string? base64Image = null;
            string? mimeType = null;
            string resimDataUrl = null;

            if (model.Fotograf != null && model.Fotograf.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await model.Fotograf.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    base64Image = Convert.ToBase64String(fileBytes);
                    mimeType = model.Fotograf.ContentType;

                    resimDataUrl = $"data:{mimeType};base64,{base64Image}";
                }
            }
            ViewBag.YuklenenResim = resimDataUrl;

            // 3. KOMUT (PROMPT) HAZIRLAMA
            string promptText = $"Sen uzman bir spor koçusun. Üye Bilgileri -> Boy: {model.Boy} cm, Kilo: {model.Kilo} kg. Ana Hedef: '{model.Hedef}'. ";

            if (base64Image != null)
            {
                promptText += "Eklenen fotoğraftaki kişinin vücut tipini analiz et. ";
            }

            promptText += "Buna göre 3 günlük, başlangıç seviyesinde egzersiz ve beslenme programı hazırla. Cevabı Türkçe ve Markdown formatında ver.";

            // 4. API İSTEĞİ (DÜZELTİLEN KISIM)
            try
            {
                var partsList = new List<object>
                {
                    new { text = promptText }
                };

                if (base64Image != null)
                {
                    partsList.Add(new
                    {
                        inline_data = new
                        {
                            mime_type = mimeType,
                            data = base64Image
                        }
                    });
                }

                var requestBody = new
                {
                    contents = new[]
                    {
                        new { parts = partsList }
                    }
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                // GÜNCELLEME: Model ismi 'gemini-2.5-flash' olarak değiştirildi.
                // Bu model hem çok daha hızlı hem de görsel okuma yeteneğine sahip.
                string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

                var response = await _httpClient.PostAsync(url, jsonContent);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using (JsonDocument doc = JsonDocument.Parse(responseString))
                    {
                        try
                        {
                            var text = doc.RootElement
                                .GetProperty("candidates")[0]
                                .GetProperty("content")
                                .GetProperty("parts")[0]
                                .GetProperty("text")
                                .GetString();

                            ViewBag.Oneri = text;
                        }
                        catch
                        {
                            // Bazen yanıt boş gelebilir veya format farklı olabilir
                            ViewBag.Oneri = "HATA: Gemini yanıtı işlenemedi. Yanıt içeriği: " + responseString;
                        }
                    }
                }
                else
                {
                    // Hata durumunda detaylı bilgi göster
                    ViewBag.Oneri = $"HATA ({response.StatusCode}): {responseString}";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Oneri = "Sistem Hatası: " + ex.Message;
            }

            return View(model);
        }
    }
}