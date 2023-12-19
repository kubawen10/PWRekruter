using PWRekruter.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace PWRekruter.ViewModels
{
    public class WiadomoscViewModel
    {
        public string? Kierunek { get; set; }
        public string? Wydzial {  get; set; }
        public string? Imie {  get; set; }
        public string? Nazwisko { get; set; }
        public string? Maile {  get; set; }
        public Boolean Zweryfikowani { get; set; }

        [Required(ErrorMessage = "Podaj tytuł wiadomości")]
        [Display(Name = "Tytuł")]
        public string Tytul { get; set; }

        [Required(ErrorMessage = "Podaj treść wiadomości")]
        [Display(Name = "Treść")]
        public string Tresc { get; set; }
    }
}
