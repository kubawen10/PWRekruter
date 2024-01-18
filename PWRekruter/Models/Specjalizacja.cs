using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PWRekruter.Models
{
    [Table("Specjalizacje")]
    public class Specjalizacja
    {
        public long Id { get; set; }
        [Required]
        public string Nazwa{ get; set; }
		[Required]
		public string Symbol {  get; set; }
		[Required]
		public string Opis {  get; set; }
        public long IdKierunku { get; set; }
        public Kierunek Kierunek { get; set; }
    }
}
