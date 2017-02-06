using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeatGeneration.Models;

namespace HeatGeneration
{
    class Program
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

            heatCount = HeatCount.GenerateHeatCount(laneCount, racers.Count);

            Console.WriteLine("Racer Count: " + racers.Count);
            Console.WriteLine("Heat Count: " + heatCount);

            for (int i = 1; i <= heatCount; i++)
            {
                var heat = new Heat();
                heat.Id = i;
                heat.RaceId = 1;
                heat.Contestants = new List<Contestant>();

                foreach (var lane in FillHeat(racers.ToList(), heat.Id))
                {
                    var _lane = lane;
                    Contestant _contestant = new Contestant();
                    _contestant.HeatId = heat.Id;
                    _contestant.Car = _lane.Scout.Name;//lookup.FirstOrDefault(x => x.Id == _lane.Scout.Id).Name;
                    _contestant.Lane = _lane.LaneNumber;
                    heat.Contestants.Add(_contestant);
                }

            }

            Console.ReadKey();
        }
        private static List<Lane> FillHeat(List<Racer> allRacers, int heatId)
        {
            var lineUp = new List<Lane>();
            List<Contestant> previousHeat = usedContestants.Where(x => x.HeatId == (heatId - 1)).OrderBy(x => x.Lane).ToList();
            var lanes = Helpers.LaneGen.GetLanes(laneCount);

            if (!usedContestants.Any())
            {
                return LoadFirstHeat(lineUp, allRacers, lanes, heatId);
            }
            //allRacers.Shuffle();

            return LoadNextHeat(previousHeat, lineUp, allRacers, lanes, heatId);

            //while (leftOver.Count < laneCount)
            //{
            //    if (topNLeftOver.Any())
            //    {
            //        var _racerLeftOver = topNLeftOver.Take(1).First();
            //        leftOver.Add(_racerLeftOver);
            //        topNLeftOver.Remove(_racerLeftOver);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            //foreach (var lane in lanes)
            //{
            //    var _racer = leftOver.Take(1).First();
            //    lineUp.Add(LoadLane(_racer, lane, heatId));
            //    //allRacers.Remove(_racer);
            //    leftOver.Remove(_racer);
            //}

            //CleanupAllRacers();
            //return lineUp;
        }
        private static List<Lane> LoadFirstHeat(List<Lane> lineUp, List<Racer> allRacers, ICollection<int> lanes, int heatId)
        {
            // If this is the first heat, just load the top 4 racers and return
            //if (!previousHeat.Any())
            //{
            List<Racer> topN = allRacers.Take(laneCount).ToList();
            foreach (var lane in lanes)
            {
                if (!topN.Any())
                    continue;

                var _racer = topN.Take(1).First();
                lineUp.Add(LoadLane(_racer, lane, heatId));
                allRacers.Remove(_racer);
                topN.Remove(_racer);
            }

            return lineUp;
            //}
        }






        private static Lane LoadLane(Racer _racer, int lane, int heatId)
        {
            usedContestants.Add(new Contestant()
            {
                Car = _racer.Name,
                HeatId = heatId,
                Lane = lane,
                RacerId = _racer.Id
            });
            return new Lane() { Scout = _racer, LaneNumber = lane };
        }
    }
}
