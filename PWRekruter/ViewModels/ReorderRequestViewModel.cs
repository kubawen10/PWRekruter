using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PWRekruter.DTO
{
    public class ReorderRequestViewModel
    {
        [Required]
        public long IdAplikacji {  get; set; }
        [Required]
        public Dictionary<int, int> Priorytety { get; set; }
    }
}
