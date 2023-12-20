﻿using PWRekruter.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace PWRekruter.ViewModels
{
    public class WiadomoscViewModel
    {
        public string? Kierunek { get; set; }
        [Display(Name = "Wydział")]
        public string? Wydzial {  get; set; }
        [Display(Name = "Imię")]
        public string? Imie {  get; set; }
        public string? Nazwisko { get; set; }
        public string? Maile {  get; set; }
        public Boolean Zakwalifikowani { get; set; }

        [Required(ErrorMessage = "Podaj tytuł wiadomości")]
        [Display(Name = "Tytuł")]
        public string Tytul { get; set; }

        [Required(ErrorMessage = "Podaj treść wiadomości")]
        [Display(Name = "Treść")]
        public string Tresc { get; set; }
    }
}
