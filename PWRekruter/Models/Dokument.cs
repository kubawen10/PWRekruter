using System;
using System.ComponentModel.DataAnnotations;

namespace PWRekruter.Models
{
    public enum TypDokumentu
    {
        [Display(Name = "Świadectwo dojrzałośći")]
        SwiadectwoDojrzalosci,
        Kwestionariusz,
        [Display(Name = "Zaświadczenie o złożonym egzaminie dyplomowym")]
        ZaswiadczenieEgzaminDyplomowy,
        [Display(Name = "Dyplom ukończenia studiów I stopnia")]
        DyplomUkonczeniaIStopnia,
        [Display(Name = "Podanie o przyjęcie na studia")]
        Podanie,
        [Display(Name = "Deklaracja wyboru specjalności")]
        WyborSpecjalnosci
    }

    public class Dokument
    {
        public long Id { get; set; }
        public string SciezkaPliku {  get; set; }
        public DateTime DataUzyskania { get; set; }
        public TypDokumentu Typ {  get; set; }
        public long IdAplikacji { get; set; }
        public Aplikacja Aplikacja { get; set; }
    }

   
}
