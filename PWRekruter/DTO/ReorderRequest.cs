using System.Collections.Generic;

namespace PWRekruter.DTO
{
    public class ReorderRequest
    {
        public long IdAplikacji {  get; set; }
        public Dictionary<int, int> Priorytety { get; set; }
    }
}
