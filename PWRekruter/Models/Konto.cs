using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWRekruter.Models
{
    [Table("Konta")]
    public abstract class Konto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Haslo { get; set; }

        public ICollection<Wiadomosc> NadaneWiadomosci { get; set; } = new List<Wiadomosc>();
        public ICollection<OdbiorcaWiadomosci> OdebraneWiadomosci { get; set; } = new List<OdbiorcaWiadomosci>();
    }
}
