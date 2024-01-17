using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PWRekruter.Models
{
    public class ProgPunktowy
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
        public int Rok {  get; set; }
        public double Wartosc { get; set; }
        public long IdKierunku { get; set; }
        public Kierunek Kierunek { get; set; }


    }
}
