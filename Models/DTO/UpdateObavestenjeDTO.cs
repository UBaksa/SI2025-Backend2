using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace carGooBackend.DTOs
{
    public class UpdateObavestenjeDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Naslov { get; set; } = string.Empty;

        [Required]
        public string Sadrzaj { get; set; } = string.Empty;

        public string? ProfilePicture { get; set; }

        public IFormFile? Image { get; set; }
    }
}
