using carGooBackend.Models.DTO;
using carGooBackend.Models;

namespace carGooBackend.Services
{
    public static class PonudaMapper
    {
        public static PonudaDetailsDTO ToPonudaDetailsDTO(Ponuda ponuda)
        {
            var dto = new PonudaDetailsDTO
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
                IdPreduzeca = ponuda.IdPreduzeca,  
                IdKorisnika = ponuda.IdKorisnika,  
                Preduzece = ponuda.Preduzece != null ? new PreduzeceDetailsDTO
                {
                    CompanyName = ponuda.Preduzece.CompanyName,
                    CompanyState = ponuda.Preduzece.CompanyState,
                    CompanyCity = ponuda.Preduzece.CompanyCity,
                    CompanyPhone = ponuda.Preduzece.CompanyPhone,
                    CompanyMail = ponuda.Preduzece.CompanyMail,
                    CompanyPIB = ponuda.Preduzece.CompanyPIB,
                } : null,
                Korisnik = ponuda.Korisnik != null ? new KorisnikDetailsDTO
                {
                    FirstName = ponuda.Korisnik.FirstName,
                    LastName = ponuda.Korisnik.LastName,
                    Email = ponuda.Korisnik.Email,
                    PhoneNumber = ponuda.Korisnik.PhoneNumber,
                    Languages = ponuda.Korisnik.Languages,
                    UserPicture = ponuda.Korisnik.UserPicture
                } : null
            };
            return dto;
        }
    }
}
