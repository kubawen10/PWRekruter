using System;
using System.Collections.Generic;
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
        [Display(Name = "Skrót")]
        [Required]
        public string Skrot { get; set; }
		[Required]
		public string Nazwa { get; set; }
        [Display(Name ="Stopień")]
        public StopienStudiow Stopien { get; set; }
        public TrybStudiow Tryb { get; set; }
        [Display(Name ="Forma studiów")]
		[NotNull]
		public FormaStudiow Forma { get; set; }
        [Display(Name ="Czas trwania")]
        [Range(1, 12, ErrorMessage = "Czas trwania jest poza dopuszczalnym zakresem")]
		[NotNull]
		public short CzasTrwania { get; set; }
        [Display(Name ="Dyscyplina naukowa")]
        public string? DyscyplinaNaukowa { get; set; } //jesli kierunek jednak moze miec wiele dyscyplin to trzeba bedzie to zmienic
        [Display(Name="Język")]
		[NotNull]
		public Jezyk JezykWykladowy { get; set; }
        public ProfilKierunku? Profil { get; set; }
		[Range(0, Double.MaxValue, ErrorMessage = "Czesne nie może być ujemne")]
		public double? Czesne { get; set; }
		[Range(0, Double.MaxValue, ErrorMessage = "Czesne nie może być ujemne")]
		[Display(Name = "Czesne dla cudzoziemców")]
        public double? CzesneDlaCudzoziemcow { get; set; }
		[Range(0, Int32.MaxValue, ErrorMessage = "Liczba miejsc nie może być ujemna")]
		[Display(Name = "Liczba miejsc")]
        public int LiczbaMiejsc { get; set; }
		[Range(0, Double.MaxValue, ErrorMessage = "Opłata rekrutacyjna nie może być ujemna")]
		[Display(Name = "Opłata rekrutacyjna")]
        public double OplataRekrutacyjna { get; set; }
        public string? Opis { get; set; }

        public string SymbolWydzialu { get; set; }
        public Wydzial Wydzial { get; set; }
		[Display(Name = "Program studiów")]
		public long? IdProgramuStudiow { get; set; }
        public ProgramStudiow? ProgramStudiow { get; set; }
        public List<ProgPunktowy> HistoryczneProgi {  get; set; }

        [AllowNull]
        public ICollection<Specjalizacja> Specjalizacje { get; set; }
    }
}
