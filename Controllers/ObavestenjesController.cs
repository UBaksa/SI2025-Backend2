using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using carGooBackend.Data;
using carGooBackend.DTOs;
using carGooBackend.Models;
using System.Security.Claims;
using carGooBackend.Services;
using Microsoft.AspNetCore.Authorization;

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObavestenjesController : ControllerBase
    {
        private readonly CarGooDataContext _context;
        private readonly CloudinaryService _cloudinary;

       
        public ObavestenjesController(CarGooDataContext context, CloudinaryService cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        // GET: api/Obavestenjes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObavestenjeDTO>>> GetObavestenje()
        {
            return await _context.Obavestenje
                .Include(o => o.Autor) // Uključuje podatke o autoru
                .Select(o => new ObavestenjeDTO
                {
                    Id = o.Id,
                    Naslov = o.Naslov,
                    DatumObjavljivanja = o.DatumObjavljivanja,
                    Sadrzaj = o.Sadrzaj,
                    ProfilePicture = o.ProfilePicture,
                    AutorId = o.AutorId,
                    AutorFirstName = o.Autor.FirstName,
                    AutorLastName = o.Autor.LastName
                })
                .ToListAsync();
        }

        // GET: api/Obavestenjes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ObavestenjeDTO>> GetObavestenje(Guid id)
        {
            var obavestenje = await _context.Obavestenje
                .Include(o => o.Autor)
                .Select(o => new ObavestenjeDTO
                {
                    Id = o.Id,
                    Naslov = o.Naslov,
                    DatumObjavljivanja = o.DatumObjavljivanja,
                    Sadrzaj = o.Sadrzaj,
                    AutorId = o.AutorId,
                    ProfilePicture = o.ProfilePicture,
                    AutorFirstName = o.Autor.FirstName,
                    AutorLastName = o.Autor.LastName
                })
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obavestenje == null)
            {
                return NotFound();
            }

            return obavestenje;
        }

        // PUT: api/Obavestenjes/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObavestenje(Guid id, [FromForm] UpdateObavestenjeDTO dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var obavestenje = await _context.Obavestenje.FirstOrDefaultAsync(o => o.Id == id);
            if (obavestenje == null) return NotFound();

            obavestenje.Naslov = dto.Naslov.Trim();
            obavestenje.Sadrzaj = dto.Sadrzaj.Trim();

            // Ako je poslat fajl -> upload i pregazi URL
            if (dto.Image != null && dto.Image.Length > 0)
            {
                if (!dto.Image.ContentType.StartsWith("image/"))
                    return BadRequest("Dozvoljene su samo slike.");

                obavestenje.ProfilePicture = await _cloudinary.UploadImageAsync(dto.Image, "obavestenja");
            }
            else
            {
                // Ako nije poslat fajl, ali je poslat URL string
                // (ako hoces da moze i ovako)
                if (dto.ProfilePicture != null)
                    obavestenje.ProfilePicture = dto.ProfilePicture;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }



        // POST: api/Obavestenjes
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ObavestenjeDTO>> PostObavestenje([FromForm] CreateObavestenjeDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Autor iz JWT-a (preporuka)
            var autorId =
                User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User?.FindFirstValue("sub") ??
                dto.AutorId;

            if (string.IsNullOrWhiteSpace(autorId))
                return Unauthorized("Nije moguće odrediti autora.");

            var autor = await _context.Users.FirstOrDefaultAsync(u => u.Id == autorId);
            if (autor == null)
                return NotFound("Autor ne postoji.");

            // 1) ako je poslat fajl -> upload na cloudinary
            // 2) ako nije -> ostaje dto.ProfilePicture (URL) ili null
            string? imageUrl = dto.ProfilePicture;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                // (opciono) basic validacija tipa fajla
                if (!dto.Image.ContentType.StartsWith("image/"))
                    return BadRequest("Dozvoljene su samo slike.");

                imageUrl = await _cloudinary.UploadImageAsync(dto.Image, "obavestenja");
            }

            var obavestenje = new Obavestenje
            {
                Id = Guid.NewGuid(),
                Naslov = dto.Naslov.Trim(),
                Sadrzaj = dto.Sadrzaj.Trim(),
                DatumObjavljivanja = DateTime.UtcNow,
                AutorId = autorId,
                ProfilePicture = imageUrl
            };

            _context.Obavestenje.Add(obavestenje);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetObavestenje), new { id = obavestenje.Id }, new ObavestenjeDTO
            {
                Id = obavestenje.Id,
                Naslov = obavestenje.Naslov,
                Sadrzaj = obavestenje.Sadrzaj,
                DatumObjavljivanja = obavestenje.DatumObjavljivanja,
                ProfilePicture = obavestenje.ProfilePicture,
                AutorId = obavestenje.AutorId,
                AutorFirstName = autor.FirstName,
                AutorLastName = autor.LastName
            });
        }

        // DELETE: api/Obavestenjes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObavestenje(Guid id)
        {
            var obavestenje = await _context.Obavestenje.FindAsync(id);
            if (obavestenje == null)
            {
                return NotFound();
            }

            _context.Obavestenje.Remove(obavestenje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ObavestenjeExists(Guid id)
        {
            return _context.Obavestenje.Any(e => e.Id == id);
        }
    }
}
