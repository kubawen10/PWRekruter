using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PWRekruter.Models
{
    [Table("TuryRekrutacji")]
    public class TuraRekrutacji
    {
        public long Id { get; set; }
        public DateTime TerminSkladaniaAplikacji { get; set; }
        public DateTime TerminWnoszeniaOplatRekrutacyjnych {  get; set; }
        [AllowNull]
        public List<Aplikacja>? Aplikacje {  get; set; }
    }
}
