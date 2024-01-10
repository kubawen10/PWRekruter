using Microsoft.EntityFrameworkCore;
using PWRekruter.Enums;
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
        public DbSet<WynikEgzaminu> WynikiEgzaminow {  get; set; }
        public DbSet<WynikOlimpiady> WynikiOlimpiady { get; set; }
        public DbSet<WynikMaturyOKE> WynikiMaturyOKE { get; set; }
        public DbSet<WynikPrzedmiotowy> WynikiPrzedmiotowe { get; set; }

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

            modelBuilder.Entity<WynikEgzaminu>()
                .HasOne(w => w.Kandydat)
                .WithMany(k => k.WynikiEgzaminow)
                .HasForeignKey(w => w.KandydatId);

            modelBuilder.Entity<WynikPrzedmiotowy>()
                .HasOne(wp => wp.WynikMaturyOKE)
                .WithMany(w => w.WynikiPrzedmiotowe)
                .HasForeignKey(wp => wp.WynikMaturyOKEId);

            modelBuilder.Entity<WynikEgzaminu>()
                .HasDiscriminator(w=>w.TypWynikuEgzaminu)
                .HasValue<WynikMaturyOKE>(TypWynikuEgzaminu.MaturaOKE)
                .HasValue<WynikOlimpiady>(TypWynikuEgzaminu.Olimpiada);

            modelBuilder.Entity<Kandydat>()
                .HasData(
                    new Kandydat { Id = 1, Email = "kandydat1", Haslo = "haslo" },
                    new Kandydat { Id = 2, Email = "kandydat2", Haslo = "haslo" },
                    new Kandydat { Id = 3, Email = "kandydat3", Haslo = "haslo" },
                    new Kandydat { Id = 4, Email = "kandydat4", Haslo = "haslo" },
                    new Kandydat { Id = 5, Email = "kandydat5", Haslo = "haslo" });

            modelBuilder.Entity<Rekruter>()
                .HasData(
                    new Rekruter { Id = 6, Email = "rekruter1", Haslo = "haslo" },
                    new Rekruter { Id = 7, Email = "rekruter2", Haslo = "haslo" },
                    new Rekruter { Id = 8, Email = "rekruter3", Haslo = "haslo" },
                    new Rekruter { Id = 9, Email = "rekruter4", Haslo = "haslo" },
                    new Rekruter { Id = 10, Email = "rekruter5", Haslo = "haslo" });
        }
    }
}
