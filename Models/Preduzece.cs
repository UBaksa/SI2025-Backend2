namespace carGooBackend.Models
{
    public class Preduzece
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyState{ get; set;}
        public string CompanyCity { get; set; }
        public string CompanyMail { get; set; }
        public string CompanyPIB { get; set; }
        public string CompanyPhone { get; set; }

        public virtual ICollection<Korisnik> Korisnici{ get; set; } = new List<Korisnik>();
    }
}
