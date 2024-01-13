using System.ComponentModel.DataAnnotations.Schema;

namespace PWRekruter.Models
{
    [Table("Specjalizacje")]
    public class Specjalizacja
    {
        public long Id { get; set; }
        public string Nazwa{ get; set; }
        public string Symbol {  get; set; }
        public string Opis {  get; set; }
        public long IdKierunku { get; set; }
    }
}
