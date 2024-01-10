using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWRekruter.Models
{
    [Table("Wydzialy")]
    public class Wydzial
    {
        public string Symbol { get; set; }
        public string Nazwa { get; set; }
        public string? Opis { get; set; }

        public ICollection<Kierunek> Kierunki { get; set; }
    }
}
