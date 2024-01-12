using Microsoft.EntityFrameworkCore;
using PWRekruter.Enums;
using PWRekruter.Models;
using System;

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

        public DbSet<Kierunek> Kierunki { get; set; }
        public DbSet<Wydzial> Wydzialy { get; set; }
        public DbSet<ProgramStudiow> ProgramyStudiow { get; set; }
        public DbSet<ProgPunktowy> ProgiPunktowe {  get; set; }
        public DbSet<Aplikacja> Aplikacje { get; set; }
        public DbSet<Preferencja> Preferencje { get; set; }
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
                    new Kandydat { Id = 1, Email = "kandydat1", Haslo = "haslo", 
                        Imie="Adam", DrugieImie="Paweł", Nazwisko="Nowak"},
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

            modelBuilder.Entity<Kierunek>().HasKey(x => x.Id);
            modelBuilder.Entity<Kierunek>()
                .HasData(
                    new Kierunek
                    {
                        Id=1,
                        Nazwa="Informatyka stosowana",
                        Skrot = "IST",
                        Stopien=StopienStudiow.Istopien,
                        Forma = FormaStudiow.Stacjonarne,
                        Tryb = TrybStudiow.Dzienne,
                        CzasTrwania=7,
                        Czesne=0.0,
                        CzesneDlaCudzoziemcow = 1250.0,
                        JezykWykladowy = Jezyk.Polski,
                        DyscyplinaNaukowa="Informatyka",
                        LiczbaMiejsc=150,
                        OplataRekrutacyjna = 80,
                        Opis = "Studia na tym kierunku umożliwiają doskonalenie umiejętności i zdobywanie wiedzy z szeroko pojmowanej informatyki. Studia pokazują różnorodność zastosowań informatyki, m.in. w rozwiązywaniu problemów biznesowych, technicznych, czy w obszarze gier komputerowych. Informatyka stosowana jest uzupełniana wiedzą z fizyki i matematyki, podstaw zarządzania i komunikacji społecznej.<br><br>Studenci kształcą się w zakresie:<br>- programowania oraz języków i technik programowania;<br>- algorytmów i struktury danych<br>- sieci komputerowych<br>- baz i hurtowni danych<br>- systemów mobilnych, rozproszonych i webowych<br>- multimediów oraz inteligentnych systemów informatycznych<br>- zarządzania projektami informatycznymi",
                        Profil = ProfilKierunku.Ogolnoakademicki,
                        SymbolWydzialu = "W4",
                        IdProgramuStudiow = 1
                    }
                );

            modelBuilder.Entity<Wydzial>()
                .HasData(
                new Wydzial { Symbol = "W4", Nazwa = "Wydział informatyki i telekomunikacji" },
                new Wydzial { Symbol = "W8", Nazwa = "Wydział zarządzania"}
                );
            modelBuilder.Entity<Wydzial>()
            .HasKey(w => w.Symbol);
            modelBuilder.Entity<Kierunek>()
            .HasOne(k => k.Wydzial)
            .WithMany(w => w.Kierunki)
            .HasForeignKey(k => k.SymbolWydzialu);

            modelBuilder.Entity<ProgramStudiow>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Kierunek>()
           .HasOne(k => k.ProgramStudiow)
           .WithMany()
           .HasForeignKey(k => k.IdProgramuStudiow)
           .IsRequired(false);
            modelBuilder.Entity<ProgramStudiow>().HasData(
                new ProgramStudiow { Id = 1, ProgramSciezka = "programy/pr1.pdf", PlanSciezka="plany/pl1.pdf" });
            
            modelBuilder.Entity<ProgPunktowy>()
                .HasKey(pp=>pp.Id);
            modelBuilder.Entity<Kierunek>()
                .HasMany(k => k.HistoryczneProgi)
                .WithOne(pp => pp.Kierunek)
                .HasForeignKey(pp => pp.IdKierunku);
            modelBuilder.Entity<ProgPunktowy>()
                .HasData(
                new ProgPunktowy { Id = 1, Rok = 2021, Wartosc = 446.0, IdKierunku =1 },
                new ProgPunktowy { Id = 2, Rok = 2022, Wartosc = 467.6, IdKierunku = 1 },
                new ProgPunktowy { Id = 3, Rok = 2023, Wartosc = 479.2, IdKierunku = 1 }
                );

            modelBuilder.Entity<Aplikacja>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<Kandydat>()
                .HasOne(k => k.Aplikacja)
                .WithOne(a => a.Kandydat)
                .HasForeignKey<Aplikacja>(a => a.IdKandydata)
                .IsRequired(false);

            modelBuilder.Entity<Preferencja>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Aplikacja>()
                .HasMany(a => a.Preferencje)
                .WithOne(p => p.Aplikacja)
                .HasForeignKey(p => p.IdAplikacji)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Preferencja>()
                .HasOne(p => p.Kierunek)
                .WithMany()
                .HasForeignKey(p => p.IdKierunku);

            modelBuilder.Entity<Aplikacja>()
                .HasData(
                new Aplikacja { Id=1, DataZlozenia=DateTime.Now, IdKandydata=1, Oplacona=true, Status=StatusAplikacji.Zlozona }
                );
            modelBuilder.Entity<Preferencja>()
                .HasData(
                new Preferencja { Id=1, IdAplikacji=1, IdKierunku=1, Priorytet=1, WartoscWskaznika = 477.7 });
           //TODO: ograniczenie ze jedna aplikacja moze miec max 6 pozycji w liscie preferencji


        }
    }
}
