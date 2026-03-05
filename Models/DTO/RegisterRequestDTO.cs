namespace carGooBackend.Models.DTO
{
    public class RegisterRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Guid PreduzeceId { get; set; }
        public string[] Roles { get; set; }
        public List<string> Languages { get; set; }
    }

}
