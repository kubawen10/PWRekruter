using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PWRekruter.Models
{

    public enum StatusAplikacji
    {
        [Display(Name ="Złożona")]       
        Zlozona,
        Oceniona
    }
    public class Aplikacja
    {
        public long Id { get; set; }
        public StatusAplikacji Status {  get; set; }
        public bool Oplacona { get; set; }
        public DateTime DataZlozenia { get; set; }
        public int IdKandydata { get; set; }
        public Kandydat Kandydat { get; set; }
        public ICollection<Preferencja> Preferencje { get; set; }
    }
}
