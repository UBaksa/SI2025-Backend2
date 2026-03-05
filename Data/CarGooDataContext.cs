using carGooBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace carGooBackend.Data
{
    public class CarGooDataContext : IdentityDbContext<Korisnik>
    {
        public CarGooDataContext(DbContextOptions<CarGooDataContext> options ) : base(options)
        {
        }

        public DbSet<Preduzece> Preduzeca { get; set;}
        public DbSet<Ponuda> Ponude { get; set;}
        public DbSet<PonudaVozila> PonudaVozilas { get; set;}
        public DbSet<Obavestenje> Obavestenja { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Korisnik>()
                .HasOne(k => k.Preduzece)  
                .WithMany(p => p.Korisnici) 
                .HasForeignKey(k => k.PreduzeceId)  
                .OnDelete(DeleteBehavior.Restrict);  

            builder.Entity<Ponuda>()
                .HasOne(p => p.Korisnik)
                .WithMany()
                .HasForeignKey(p => p.IdKorisnika)
                .OnDelete(DeleteBehavior.Restrict);

            var dispecerRoleId = "8a000e3b-b915-43f1-b90e-b28075ec8cac";
            var prevoznikRoleId = "415a7c65-81dd-4fe3-9c44-9493db860c4b";
            var kontrolerRoleId = "415a7c65-81dd-4fe3-9c44-9493db860c4c";
            var roles = new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = dispecerRoleId,
            ConcurrencyStamp = dispecerRoleId,
            Name="Dispecer",
            NormalizedName="Dispecer".ToUpper()
        },
        new IdentityRole
        {
            Id = prevoznikRoleId,
            ConcurrencyStamp = prevoznikRoleId,
            Name="Prevoznik",
            NormalizedName="Prevoznik".ToUpper()
        },
        new IdentityRole
        {
            Id = kontrolerRoleId,
            ConcurrencyStamp = kontrolerRoleId,
            Name="Kontroler",
            NormalizedName="Kontroler".ToUpper()
        }
    };
            builder.Entity<IdentityRole>().HasData(roles);
        }
        public DbSet<carGooBackend.Models.PonudaVozila> PonudaVozila { get; set; } = default!;
        public DbSet<carGooBackend.Models.Obavestenje> Obavestenje { get; set; } = default!;


    }
}
