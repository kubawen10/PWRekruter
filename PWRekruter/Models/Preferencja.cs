﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PWRekruter.Models
{
    public enum WynikAplikacji
    {
        Zakwalifikowano,
        Odrzucono
    }
    public class Preferencja
    {
        public long Id { get; set; }
        public int Priorytet { get; set; }
        [Display(Name ="Wskaźnik")]
        public double WartoscWskaznika { get; set; }
        public WynikAplikacji? Wynik {  get; set; }
        public long IdKierunku { get; set; }
        public Kierunek Kierunek { get; set; }
        public long? IdWybranejSpecjalizacji { get; set; }
        [AllowNull]
        public Specjalizacja WybranaSpecjalizacja { get; set; }
        public long IdAplikacji {  get; set; }
        public Aplikacja Aplikacja { get; set; }
    }
}
