using Microsoft.AspNetCore.Mvc.Rendering;
using PWRekruter.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWRekruter.Models
{
    [Table("WynikiEgzaminow")]
    public abstract class WynikEgzaminu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int KandydatId {  get; set; }
        public Kandydat Kandydat { get; set; }
        public TypWynikuEgzaminu TypWynikuEgzaminu { get; set; }
    }

    public class WynikOlimpiady : WynikEgzaminu
    {
        public Olimpiada Olimpiada { get; set; }
        [Display(Name = "Uzyskany tytuł")]
        public TytulOlimpijczyka? TytulOlimpijczyka { get; set; }
    }

    [Table("WynikiPrzedmiotowe")]
    public class WynikPrzedmiotowy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public TypPrzedmiotu TypPrzedmiotu { get; set; }
        [Required]
        [Range(0, 100)]
        public int Wynik { get; set; }
        [Required]
        public PoziomPrzedmiotu PoziomPrzedmiotu { get; set; }

        public int WynikMaturyOKEId { get; set; }
        public WynikMaturyOKE WynikMaturyOKE { get; set; }
    }

    public class WynikMaturyOKE : WynikEgzaminu
    {
        public ICollection<WynikPrzedmiotowy> WynikiPrzedmiotowe { get; set; }
    }
}
