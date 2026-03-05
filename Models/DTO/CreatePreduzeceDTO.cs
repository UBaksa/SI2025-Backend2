namespace carGooBackend.Models.DTO
{
    public class CreatePreduzeceDTO
    {
        public string CompanyName { get; set; }
        public string CompanyState { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyMail { get; set; }
        public string CompanyPIB { get; set; }
        public string CompanyPhone { get; set; }
        public string[] KorisnikIds { get; set; }

        public IFormFile? companyPhoto { get; set; }
    }
}
