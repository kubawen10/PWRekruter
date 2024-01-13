using Microsoft.VisualBasic;
using PWRekruter.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PWRekruter.Models
{

    public class Aplikacja
    {
        public long Id { get; set; }
        public StatusAplikacji Status {  get; set; }
        public bool Oplacona { get; set; }
        public DateTime DataZlozenia { get; set; }
        public int IdKandydata { get; set; }
        public Kandydat Kandydat { get; set; }
        public ICollection<Preferencja> Preferencje { get; set; }
        public long IdTuryRekrutacji { get; set; }
        public TuraRekrutacji TuraRekrutacji { get; set; }
        public ICollection<Dokument> Dokumenty { get; set; }
    }
}
