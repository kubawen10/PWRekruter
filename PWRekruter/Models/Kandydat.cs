using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWRekruter.Models
{
    public enum Plec { Kobieta, Meżczyzna }

    [Table("Kandydaci")]
    public class Kandydat:Konto
    {
        [Display(Name = "Imię")]
        public string? Imie { get; set; }
        [Display(Name = "Drugie imię")]
        public string? DrugieImie { get; set; }
        public string? Nazwisko { get; set; }
        [Display(Name = "PESEL")]
        public string? Pesel { get; set; }
        [Display(Name = "Płeć")]
        public Plec? Plec { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataUrodzenia { get; set; }
        [Display(Name = "Państwo")]
        public string? Panstwo { get; set; }
        [Display(Name = "Kod pocztowy")]
        public string? KodPocztowy { get; set; }
        [Display(Name = "Miejscowość")]
        public string? Miejscowosc { get; set; }
        public string? Ulica { get; set; }
        [Display(Name = "Numer budynku")]
        public string? NumerBudynku { get; set; }
        [Display(Name = "Numer mieszkania")]
        public string? NumerMieszkania { get; set; }
    }
}
