using System.ComponentModel.DataAnnotations.Schema;

namespace PWRekruter.Models
{
    [Table("Rekruterzy")]
    public class Rekruter:Konto
    {
        public string Nazwisko {  get; set; }
    }
}
