using Microsoft.EntityFrameworkCore;
using PWRekruter.Models;

namespace PWRekruter.Data
{
    public class PWRekruterDbContext:DbContext
    {
        public PWRekruterDbContext(DbContextOptions<PWRekruterDbContext> options) : base(options) { }

        public DbSet<Konto> Konta { get; set; }
        public DbSet<Kandydat> Kandydaci { get; set; }
        public DbSet<Rekruter> Rekruterzy { get; set; }
        public DbSet<Wiadomosc> Wiadomosci { get; set; }
        public DbSet<OdbiorcaWiadomosci> OdbiorcyWiadomosci { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OdbiorcaWiadomosci>()
                .HasKey(ow => new {ow.WiadomoscId, ow.OdbiorcaId});

            modelBuilder.Entity<Wiadomosc>()
                .HasMany(w => w.Odbiorcy)
                .WithOne(ow => ow.Wiadomosc)
                .HasForeignKey(ow => ow.WiadomoscId);

            modelBuilder.Entity<OdbiorcaWiadomosci>()
                .HasOne(ow => ow.Odbiorca)
                .WithMany(k => k.OdebraneWiadomosci)
                .HasForeignKey(ow => ow.OdbiorcaId);

            modelBuilder.Entity<Wiadomosc>()
                .HasOne(w => w.Nadawca)
                .WithMany(k => k.NadaneWiadomosci)
                .HasForeignKey(w => w.NadawcaId);


            modelBuilder.Entity<Kandydat>()
                .HasData(
                    new Kandydat { Id = 1, Email = "kandydat 1", Haslo = "haslo" },
                    new Kandydat { Id = 2, Email = "kandydat 2", Haslo = "haslo" });

            modelBuilder.Entity<Rekruter>()
                .HasData(
                    new Rekruter { Id = 3, Email = "rekruter 1", Haslo = "haslo" },
                    new Rekruter { Id = 4, Email = "rekruter 2", Haslo = "haslo" });
        }
    }
}
