using System.Collections.Generic;

namespace HeatGeneration.Models
{
    public class Heat
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public List<Contestant> Contestants { get; set; } 
    }
}
