using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeatGeneration.Models;

namespace HeatGeneration
{
    class Programnew
    {
        public static int laneCount = 4;
        public static int raceCount = 4;
        public static int heatCount { get; set; }

        private static List<Contestant> usedContestants = new List<Contestant>();
        private static List<Racer> racers = new List<Racer>();
        private static List<Racer> lookup = new List<Racer>();
        private static List<Heat> heats = new List<Heat>();


        static void Main(string[] args)
        {
            laneCount = 3;
            //raceCount = 4;

            racers = Helpers.RacerGen.GetRacers();
            //lookup = Helpers.RacerGen.GetRacers();

            heatCount = HeatCount.GenerateHeatCount(laneCount, racers.Count);//.CalculateHeatCount(laneCount, racers.Count, raceCount);

            Console.WriteLine("Racer Count: " + racers.Count);
            Console.WriteLine("Heat Count: " + heatCount);



            Console.ReadKey();
        }
    }
}
