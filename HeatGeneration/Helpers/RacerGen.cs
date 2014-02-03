using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeatGeneration.Models;

namespace HeatGeneration.Helpers
{
    internal class RacerGen
    {
        public static List<Racer> GetRacers()
        {
            var racers = new List<Racer>();
            racers.Add(new Racer() { Id = 1, Name = "aaaa" });
            racers.Add(new Racer() { Id = 2, Name = "bbbb" });
            racers.Add(new Racer() { Id = 3, Name = "cccc" });
            racers.Add(new Racer() { Id = 4, Name = "dddd" });
            racers.Add(new Racer() { Id = 5, Name = "eeee" });

            return racers;
        }
    }
}
