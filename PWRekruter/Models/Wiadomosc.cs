using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWRekruter.Models
{
    [Table("Wiadomosci")]
    public class Wiadomosc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int NadawcaId { get; set; }
        public Konto Nadawca { get; set; }

        public ICollection<OdbiorcaWiadomosci> Odbiorcy { get; set; }


        [Required]
        [Display(Name = "Tytuł")]
        public string Tytul { get; set; }

        [Required]
        [Display(Name = "Treść")]
        public string Tresc { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data {  get; set; }
    }

    [Table("OdbiorcyWiadomosci")]
    public class OdbiorcaWiadomosci
    {
        public int WiadomoscId { get; set; }
        public Wiadomosc Wiadomosc { get; set; }

        public int OdbiorcaId { get; set; }
        public Konto Odbiorca { get; set; }
    }
}
