using System;
using Microsoft.AspNetCore.Http;
namespace carGooBackend.DTOs
{
    public class ObavestenjeDTO
    {
        public Guid Id { get; set; }
        public string Naslov { get; set; }
        public DateTime DatumObjavljivanja { get; set; }
        public string Sadrzaj { get; set; }
        public string ProfilePicture { get; set; }
        public string AutorFirstName { get; set; }
        public string AutorLastName { get; set; }
        public string AutorId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
