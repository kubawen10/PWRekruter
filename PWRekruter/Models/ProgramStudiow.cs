using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PWRekruter.Models
{
    public class ProgramStudiow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Nazwa {  get; set; }
        public string ProgramSciezka { get; set; }
        public string PlanSciezka { get; set; }

    }
}
