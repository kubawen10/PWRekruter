﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PWRekruter.Enums;

namespace PWRekruter.Models
{
    

    [Table("Kierunki")]
    public class Kierunek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Skrot { get; set; }
        public string Nazwa { get; set; }
        [Display(Name ="Stopień")]
        public StopienStudiow Stopien { get; set; }
        public TrybStudiow Tryb { get; set; }
        [Display(Name ="Forma studiów")]
        public FormaStudiow Forma { get; set; }
        [Display(Name ="Czas trwania")]
        public short CzasTrwania { get; set; }
        [Display(Name ="Dyscyplina naukowa")]
        public string? DyscyplinaNaukowa { get; set; } //jesli kierunek jednak moze miec wiele dyscyplin to trzeba bedzie to zmienic
        [Display(Name="Język")]
        public Jezyk JezykWykladowy { get; set; }
        public ProfilKierunku? Profil { get; set; }
        public double? Czesne { get; set; }
        [Display(Name = "Czesne dla cudzoziemców")]
        public double? CzesneDlaCudzoziemcow { get; set; }
        [Display(Name = "Liczba miejsc")]
        public int LiczbaMiejsc { get; set; }
        public double OplataRekrutacyjna { get; set; }
        public string? Opis { get; set; }

        public string SymbolWydzialu { get; set; }
        public Wydzial Wydzial { get; set; }
        public long? IdProgramuStudiow { get; set; }
        public ProgramStudiow? ProgramStudiow { get; set; }
        public List<ProgPunktowy> HistoryczneProgi {  get; set; }

        [AllowNull]
        public ICollection<Specjalizacja> Specjalizacje { get; set; }
    }
}