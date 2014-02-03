using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        static void Main(string[] args)
        {
            laneCount = 4;
            raceCount = 4;

            racers = GetRacers();
            lookup = GetRacers();
            int totalHeats = HeatCount.CalculateHeatCount(laneCount, racers.Count, raceCount);

            Console.WriteLine("Total racers: " + racers.Count());
            Console.WriteLine("Total lanes: " + laneCount);
            Console.WriteLine("Total heats: " + totalHeats);

            List<Heat> Heats = new List<Heat>();
            ICollection<int> lanes = new Collection<int>();
            int heatId = 1;
            for (int i = 0; i <= (totalHeats - 1); i++)
            {
                ICollection<Lane> usedLanes = new Collection<Lane>();
                var heat = new Heat();
                heat.Id = heatId;
                heat.RaceId = 1;
                heat.Contestants = new List<Contestant>();

                foreach (var lane in FillLineup(racers.ToList(), heat.Id))
                {
                    var _lane = lane;
                    Contestant _contestant = new Contestant();
                    _contestant.HeatId = heat.Id;
                    _contestant.Car = lookup.FirstOrDefault(x => x.Id == _lane.RacerId).Name;
                    _contestant.Lane = _lane.LaneNumber;
                    heat.Contestants.Add(_contestant);
                }

                Heats.Add(heat);
                heatId++;
            }

            foreach (var heat in Heats)
            {
                var _heat = heat;
                Console.WriteLine("Id: " + _heat.Id);
                Console.WriteLine("-------");
                foreach (var racer in _heat.Contestants)
                {
                    Console.WriteLine(racer.Lane + ": " + racer.Car);
                }
                Console.WriteLine(" ");
            }

            Console.ReadKey();
        }

        private static List<Lane> FillLineup(List<Racer> allRacers, int heatId)
        {
            var lineUp = new List<Lane>();
            List<Contestant> previousHeat = usedContestants.Where(x => x.HeatId == (heatId - 1)).ToList();
            var lanes = GetLanes();

            // If this is the first heat, just load the top 4 racers and return
            if (!previousHeat.Any())
            {
                List<Racer> topN = allRacers.OrderBy(x => x.Id).Take(laneCount).ToList();
                foreach (var lane in lanes)
                {
                    var _racer = topN.Take(1).First();
                    lineUp.Add(LoadLane(_racer, lane, heatId));
                    allRacers.Remove(_racer);
                    topN.Remove(_racer);
                }

                return lineUp;
            }

            //// If this is the second heat, use the remaining racers first
            //List<Racer> previous = new List<Racer>();
            //foreach (var _last in previousHeat)
            //{
            //    previous.Add(lookup.FirstOrDefault(x => x.Id == _last.RacerId));
            //}

            //List<Racer> leftOver = racers.Where(r => previous.All(c => c.Id != r.Id)).ToList();
            //List<Racer> topNLeftOver = racers.OrderBy(x => x.Id).ToList();

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
            return lineUp;
        }

        private static void CleanupAllRacers()
        {
            foreach (var item in racers.ToList())
            {
                var _racer = item;
                var previousRaces = usedContestants.Where(x => x.RacerId == _racer.Id).ToList();

                if (previousRaces.Count() == (raceCount - 1))
                {
                    racers.Remove(_racer);
                }
            }
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
            return new Lane() { RacerId = _racer.Id, LaneNumber = lane };
        }

        private static ICollection<int> GetLanes()
        {
            ICollection<int> lanes = new Collection<int>();
            for (int i = 1; i <= laneCount; i++)
            {
                lanes.Add(i);
            }
            return lanes;
        }

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
