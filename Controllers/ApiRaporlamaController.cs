using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webOdev.Data;
using webOdev.Models;

namespace webOdev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiRaporlamaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiRaporlamaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Antrenorler")]
        public async Task<ActionResult<IEnumerable<Antrenor>>> GetAntrenorler()
        {
            return Ok(await _context.Antrenorler.ToListAsync());
        }

        [HttpGet("UygunAntrenorler")]
        public async Task<ActionResult<IEnumerable<Antrenor>>> GetUygunAntrenorler([FromQuery] DateTime tarih)
        {
            var tarihUtc = tarih.ToUniversalTime();

            // O tarihte, kapasitesi dolmuş (DoluMu = true) olan dersleri veren hocaları bul
            var doluHocaIdleri = await _context.DersSeanslari
                .Where(d => d.Tarih.Date == tarihUtc.Date && d.DoluMu == true)
                .Select(d => d.AntrenorId)
                .Distinct()
                .ToListAsync();

            // Bu hocalari listeden çıkar
            var uygunAntrenorler = await _context.Antrenorler
                .Where(a => !doluHocaIdleri.Contains(a.Id))
                .ToListAsync();

            return Ok(uygunAntrenorler);
        }
    }
}