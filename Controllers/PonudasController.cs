using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using carGooBackend.Data;
using carGooBackend.Models;
using carGooBackend.Models.DTO;
using carGooBackend.Services;

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PonudasController : ControllerBase
    {
        private readonly CarGooDataContext _context;

        public PonudasController(CarGooDataContext context)
        {
            _context = context;
        }
        // GET ALL PONUDA
        // GET: api/Ponudas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // get all ponude from database to domain
            var ponude = await _context.Ponude.ToListAsync();

            //map domain models to DTOs
            var ponudeDTO = new List<PonudeDTO>();
            foreach (var ponuda in ponude)
            {
                ponudeDTO.Add(new PonudeDTO()
                {
                    PonudaId = ponuda.PonudaId,
                    DrzavaU = ponuda.DrzavaU,
                    DrzavaI = ponuda.DrzavaI,
                    MestoU = ponuda.MestoU,
                    MestoI = ponuda.MestoI,
                    Utovar = ponuda.Utovar,
                    Istovar = ponuda.Istovar,
                    Duzina = ponuda.Duzina,
                    Tezina = ponuda.Tezina,
                    TipNadogradnje = ponuda.TipNadogradnje,
                    TipKamiona = ponuda.TipKamiona,
                    VrstaTereta = ponuda.VrstaTereta,
                    Cena = ponuda.Cena,
                    ZamenaPaleta = ponuda.ZamenaPaleta,
                    IdKorisnika = ponuda.IdKorisnika,
                    IdPreduzeca = ponuda.IdPreduzeca,
                    Vreme = ponuda.Vreme
                });
            }

            //returning DTOs
            return Ok(ponudeDTO);
        }

        // GET ONE PONUDA by id
        // GET: api/Ponudas/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var ponuda = await _context.Ponude
                .Include(p => p.Korisnik)
                .Include(p => p.Preduzece)
                .FirstOrDefaultAsync(p => p.PonudaId == id);

            if (ponuda == null)
            {
                return NotFound();
            }

            var ponudaDTO = PonudaMapper.ToPonudaDetailsDTO(ponuda);
            return Ok(ponudaDTO);
        }

        // POST to create Ponuda
        // frombody is because we are receving from client to domain,previous are from domain to client
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePonudeDTO createPonudeDTO)
        {
            //convert DTO to domain
            var ponuda = new Ponuda
            {
                DrzavaU = createPonudeDTO.DrzavaU,
                DrzavaI = createPonudeDTO.DrzavaI,
                MestoU = createPonudeDTO.MestoU,
                MestoI = createPonudeDTO.MestoI,
                Utovar = createPonudeDTO.Utovar,
                Istovar = createPonudeDTO.Istovar,
                Duzina = createPonudeDTO.Duzina,
                Tezina = createPonudeDTO.Tezina,
                TipNadogradnje = createPonudeDTO.TipNadogradnje,
                TipKamiona = createPonudeDTO.TipKamiona,
                VrstaTereta = createPonudeDTO.VrstaTereta,
                ZamenaPaleta = createPonudeDTO.ZamenaPaleta,
                Cena = createPonudeDTO.Cena,
                IdPreduzeca = createPonudeDTO.IdPreduzeca,
                IdKorisnika = createPonudeDTO.IdKorisnika,
                Vreme = DateTime.Now

            };
            //use domain to create Ponuda 

            await _context.Ponude.AddAsync(ponuda);
            await _context.SaveChangesAsync();

            //map domain model back to dto

            var ponudaDTO = new PonudeDTO
            {
                PonudaId = ponuda.PonudaId,
                DrzavaU = ponuda.DrzavaU,
                DrzavaI = ponuda.DrzavaI,
                MestoU = ponuda.MestoU,
                MestoI = ponuda.MestoI,
                Utovar = ponuda.Utovar,
                Istovar = ponuda.Istovar,
                Duzina = ponuda.Duzina,
                Tezina = ponuda.Tezina,
                TipNadogradnje = ponuda.TipNadogradnje,
                TipKamiona = ponuda.TipKamiona,
                VrstaTereta = ponuda.VrstaTereta,
                IdKorisnika = ponuda.IdKorisnika,
                IdPreduzeca = ponuda.IdPreduzeca,
                ZamenaPaleta = ponuda.ZamenaPaleta,
                Cena = ponuda.Cena
            };

            return CreatedAtAction(nameof(GetById), new { id = ponudaDTO.PonudaId }, ponudaDTO);

        }

        //update ponuda
        //PUT METHOD

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePonudeDTO updatePonudeDTO)
        {
            var ponuda = await _context.Ponude.FirstOrDefaultAsync(x => x.PonudaId == id);

            if (ponuda == null)
            {
                return NotFound();
            }
            //ponuda with id found,map DTO to domain

            ponuda.DrzavaU = updatePonudeDTO.DrzavaU;
            ponuda.DrzavaI = updatePonudeDTO.DrzavaI;
            ponuda.MestoU = updatePonudeDTO.MestoU;
            ponuda.MestoI = updatePonudeDTO.MestoI;
            ponuda.Utovar = updatePonudeDTO.Utovar;
            ponuda.Istovar = updatePonudeDTO.Istovar;
            ponuda.Duzina = updatePonudeDTO.Duzina;
            ponuda.Tezina = updatePonudeDTO.Tezina;
            ponuda.TipNadogradnje = updatePonudeDTO.TipNadogradnje;
            ponuda.TipKamiona = updatePonudeDTO.TipKamiona;
            ponuda.VrstaTereta = updatePonudeDTO.VrstaTereta;
            ponuda.ZamenaPaleta = updatePonudeDTO.ZamenaPaleta;
            ponuda.Cena = updatePonudeDTO.Cena;

            await _context.SaveChangesAsync();

            //convert domain to DTO

            var ponudaDTO = new PonudeDTO
            {
                PonudaId = ponuda.PonudaId,
                DrzavaU = ponuda.DrzavaU,
                DrzavaI = ponuda.DrzavaI,
                MestoU = ponuda.MestoU,
                MestoI = ponuda.MestoI,
                Utovar = ponuda.Utovar,
                Istovar = ponuda.Istovar,
                Duzina = ponuda.Duzina,
                Tezina = ponuda.Tezina,
                TipNadogradnje = ponuda.TipNadogradnje,
                TipKamiona = ponuda.TipKamiona,
                VrstaTereta = ponuda.VrstaTereta,
                ZamenaPaleta = ponuda.ZamenaPaleta,
                Cena = ponuda.Cena

            };

            return Ok(ponudaDTO);

        }
        //Delete ponuda
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var ponuda = await _context.Ponude.FirstOrDefaultAsync(x => x.PonudaId == id);

            if (ponuda == null)
            {
                return NotFound();
            }

            //delete ponuda

            _context.Ponude.Remove(ponuda);
            await _context.SaveChangesAsync();

            //return deleted ponuda back

            var ponudaDTO = new PonudeDTO
            {
                PonudaId = ponuda.PonudaId,
                DrzavaU = ponuda.DrzavaU,
                DrzavaI = ponuda.DrzavaI,
                MestoU = ponuda.MestoU,
                MestoI = ponuda.MestoI,
                Utovar = ponuda.Utovar,
                Istovar = ponuda.Istovar,
                Duzina = ponuda.Duzina,
                Tezina = ponuda.Tezina,
                TipNadogradnje = ponuda.TipNadogradnje,
                TipKamiona = ponuda.TipKamiona,
                VrstaTereta = ponuda.VrstaTereta,
                ZamenaPaleta = ponuda.ZamenaPaleta,
                Cena = ponuda.Cena
            };
            return Ok(ponudaDTO);
        }
        // GET: api/Ponudas/preduzeceponude/{idPreduzece}
        [HttpGet("preduzeceponude/{idPreduzece:Guid}")]
        public async Task<IActionResult> GetByPreduzece([FromRoute] Guid idPreduzece)
        {
            // Pridruživanje ponuda sa preduzećima
            var ponude = await _context.Ponude
                .Where(p => p.IdPreduzeca == idPreduzece)
                .Include(p => p.Preduzece) // Pretpostavka: Ponuda ima navigaciono svojstvo `Preduzece`
                .ToListAsync();

            if (!ponude.Any())
            {
                return NotFound("Nema ponuda za zadato preduzeće.");
            }

            // Base URL za slike preduzeća
            var baseUrl = $"{Request.Scheme}://{Request.Host}/images/";

            // Mapiranje podataka u DTO
            var ponudeDTO = ponude.Select(ponuda => new PonudeDTO
            {
                PonudaId = ponuda.PonudaId,
                DrzavaU = ponuda.DrzavaU,
                DrzavaI = ponuda.DrzavaI,
                MestoU = ponuda.MestoU,
                MestoI = ponuda.MestoI,
                Utovar = ponuda.Utovar,
                Istovar = ponuda.Istovar,
                Duzina = ponuda.Duzina,
                Tezina = ponuda.Tezina,
                TipNadogradnje = ponuda.TipNadogradnje,
                TipKamiona = ponuda.TipKamiona,
                VrstaTereta = ponuda.VrstaTereta,
                Cena = ponuda.Cena,
                ZamenaPaleta = ponuda.ZamenaPaleta,
                Vreme = ponuda.Vreme,
            }).ToList();

            return Ok(ponudeDTO);
        }
    }
    }
