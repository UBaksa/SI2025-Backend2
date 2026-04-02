namespace carGooBackend.Models.DTO
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Languages { get; set; }

        public string? NewPassword { get; set; }
        public IFormFile? UserPicture { get; set; }
    }
}
