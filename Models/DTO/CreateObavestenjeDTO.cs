using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace carGooBackend.DTOs
{
    public class CreateObavestenjeDTO
    {
        [Required]
        [MaxLength(100)]
        public string Naslov { get; set; } = string.Empty;

        [Required]
        public string Sadrzaj { get; set; } = string.Empty;

        public string? ProfilePicture { get; set; }

        public IFormFile? Image { get; set; }

        public string? AutorId { get; set; }
    }
}
