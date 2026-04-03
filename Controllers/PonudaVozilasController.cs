using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using carGooBackend.Data;
using carGooBackend.Models;
using Microsoft.AspNetCore.Authorization;
using carGooBackend.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PonudaVozilasController : ControllerBase
    {
        private readonly CarGooDataContext _context;
        private readonly UserManager<Korisnik> _userManager; 

        public PonudaVozilasController(CarGooDataContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/PonudaVozilas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PonudaVozila>>> GetPonudaVozila()
        {
             return await _context.PonudaVozila
            .Include(p => p.Preduzece)
            .Include(p => p.Korisnik)
            .ToListAsync();
        }

        // GET: api/PonudaVozilas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PonudaVozila>> GetPonudaVozila(Guid id)
        {
            var ponudaVozila = await _context.PonudaVozila
                .Include(p => p.Preduzece)
                .Include(p => p.Korisnik)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (ponudaVozila == null)
            {
                return NotFound();
            }

            return ponudaVozila;
        }

        // PUT: api/PonudaVozilas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPonudaVozila(Guid id, UpdatePonudaVoziloDTO dto)
        {
            var ponudaVozila = await _context.PonudaVozila.FirstOrDefaultAsync(p => p.Id == id);

            if (ponudaVozila == null)
            {
                return NotFound();
            }

            ponudaVozila.DrzavaU = dto.DrzavaU;
            ponudaVozila.DrzavaI = dto.DrzavaI;
            ponudaVozila.MestoU = dto.MestoU;
            ponudaVozila.MestoI = dto.MestoI;
            ponudaVozila.RadiusI = dto.RadiusI;
            ponudaVozila.Utovar = DateTime.SpecifyKind(dto.Utovar, DateTimeKind.Utc);
            ponudaVozila.Istovar = DateTime.SpecifyKind(dto.Istovar, DateTimeKind.Utc);
            ponudaVozila.Duzina = dto.Duzina;
            ponudaVozila.Tezina = dto.Tezina;
            ponudaVozila.TipNadogradnje = dto.TipNadogradnje;
            ponudaVozila.TipKamiona = dto.TipKamiona;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/PonudaVozilas
        [HttpPost]
        public async Task<ActionResult<PonudaVozila>> PostPonudaVozila(CreatePonudaVozilaDTO createPonudaVozilaDTO)
        {
            try
            {
                // Pronalazak Preduzeća
                var preduzece = await _context.Preduzeca.FindAsync(createPonudaVozilaDTO.IdPreduzeca);
                if (preduzece == null)
                {
                    return NotFound(new { Message = "Preduzeće nije pronađeno." });
                }

                // Pronalazak Korisnika - using _userManager instead of userManager
                var korisnik = await _userManager.FindByIdAsync(createPonudaVozilaDTO.IdKorisnika);
                if (korisnik == null)
                {
                    return NotFound(new { Message = "Korisnik nije pronađen." });
                }

                // Mapiranje DTO-a na model
                var ponudaVozila = new PonudaVozila
                {
                    Id = Guid.NewGuid(),
                    DrzavaU = createPonudaVozilaDTO.DrzavaU,
                    DrzavaI = createPonudaVozilaDTO.DrzavaI,
                    MestoU = createPonudaVozilaDTO.MestoU,
                    MestoI = createPonudaVozilaDTO.MestoI,
                    RadiusI = createPonudaVozilaDTO.RadiusI,
                    Utovar = createPonudaVozilaDTO.Utovar,
                    Istovar = createPonudaVozilaDTO.Istovar,
                    Duzina = createPonudaVozilaDTO.Duzina,
                    Tezina = createPonudaVozilaDTO.Tezina,
                    TipNadogradnje = createPonudaVozilaDTO.TipNadogradnje,
                    TipKamiona = createPonudaVozilaDTO.TipKamiona,
                    IdPreduzeca = createPonudaVozilaDTO.IdPreduzeca,
                    IdKorisnika = createPonudaVozilaDTO.IdKorisnika,
                    Vreme = DateTime.UtcNow,
                    Preduzece = preduzece,
                    Korisnik = korisnik
                };

                _context.PonudaVozila.Add(ponudaVozila);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPonudaVozila), new { id = ponudaVozila.Id }, ponudaVozila);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = "Došlo je do greške prilikom kreiranja PonudaVozila.",
                        Error = ex.Message,
                        StackTrace = ex.StackTrace
                    });
            }
        }
        // GET: api/PonudaVozilas/preduzeceponude/{idPreduzece}
        [HttpGet("preduzeceponude/{idPreduzece}")]
        public async Task<ActionResult<IEnumerable<PonudaVozila>>> GetPonudaVozilaByPreduzece(Guid idPreduzece)
        {
            // Proveri da li preduzeće postoji
            var preduzece = await _context.Preduzeca.FindAsync(idPreduzece);
            if (preduzece == null)
            {
                return NotFound(new { Message = "Preduzeće nije pronađeno." });
            }

            // Filtriraj ponude vozila prema ID-u preduzeća
            var ponude = await _context.PonudaVozila
                .Where(p => p.IdPreduzeca == idPreduzece)
                .Include(p => p.Preduzece)
                .Include(p => p.Korisnik)
                .ToListAsync();

            if (ponude == null || !ponude.Any())
            {
                return NotFound(new { Message = "Nema ponuda za ovo preduzeće." });
            }

            return Ok(ponude);
        }

        // DELETE: api/PonudaVozilas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePonudaVozila(Guid id)
        {
            var ponudaVozila = await _context.PonudaVozila.FindAsync(id);
            if (ponudaVozila == null)
            {
                return NotFound();
            }

            _context.PonudaVozila.Remove(ponudaVozila);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PonudaVozilaExists(Guid id)
        {
            return _context.PonudaVozila.Any(e => e.Id == id);
        }
    }
}
