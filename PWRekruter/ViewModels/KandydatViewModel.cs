using PWRekruter.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace PWRekruter.ViewModels
{
    public class KandydatViewModel
    {
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Podaj imię")]
        public string? Imie { get; set; }
        [Display(Name = "Drugie imię")]
        public string? DrugieImie { get; set; }
        [Required(ErrorMessage = "Podaj nazwisko")]
        public string? Nazwisko { get; set; }
        [Display(Name = "PESEL")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL musi składać się z 11 cyfr.")]
        public string? Pesel { get; set; }
        [Display(Name = "Płeć")]
        [EnumDataType(typeof(Plec))]
        [Required(ErrorMessage = "Podaj płeć")]
        public Plec? Plec { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Podaj datę urodzenia")]
        public DateTime? DataUrodzenia { get; set; }
        [Display(Name = "Państwo")]
        [Required(ErrorMessage = "Podaj państwo")]
        public string? Panstwo { get; set; }
        [Display(Name = "Kod pocztowy", Prompt = "00-000")]
        [RegularExpression(@"\d\d-\d\d\d", ErrorMessage = "Kod pocztowy powinien być w formacie 00-000")]
        [Required(ErrorMessage = "Podaj kod pocztowy")]
        public string? KodPocztowy { get; set; }
        [Display(Name = "Miejscowość")]
        [Required(ErrorMessage = "Podaj miejscowość")]
        public string? Miejscowosc { get; set; }
        public string? Ulica { get; set; }
        [Display(Name = "Numer budynku")]
        [Required(ErrorMessage = "Podaj numer budynku")]
        public string? NumerBudynku { get; set; }
        [Display(Name = "Numer mieszkania")]
        public string? NumerMieszkania { get; set; }
    }
}
