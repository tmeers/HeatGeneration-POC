using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
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
            laneCount = 3;
            raceCount = 4;

            racers = Helpers.RacerGen.GetRacers();
            lookup = Helpers.RacerGen.GetRacers();

            heatCount = HeatCount.GenerateHeatCount(laneCount, racers.Count);//.CalculateHeatCount(laneCount, racers.Count, raceCount);

            //if (racers.Count < laneCount)
            while (racers.Count < laneCount)
            {
                var _bye = new Racer();
                _bye.Id = -1;
                _bye.Name = "BYE";
                _bye.Finished = false;

                racers.Add(_bye);
                lookup.Add(_bye);
            }

            Console.WriteLine("Total racers: " + racers.Count());
            Console.WriteLine("Total lanes: " + laneCount);
            Console.WriteLine("Total heats: " + heatCount);

            List<Heat> Heats = new List<Heat>();
            ICollection<int> lanes = new Collection<int>();
            int heatId = 1;

            for (int i = 0; i <= (heatCount - 1); i++)
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
                    _contestant.Car = _lane.Scout.Name;//lookup.FirstOrDefault(x => x.Id == _lane.Scout.Id).Name;
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


        private static List<Lane> LoadNextHeat(List<Contestant> previousHeat, List<Lane> lineUp, List<Racer> allRacers, ICollection<int> lanes, int heatId)
        {
            //// If this is the second heat, use the remaining racers first
            List<Racer> previous = new List<Racer>();
            foreach (var _last in previousHeat)
            {
                previous.Add(lookup.FirstOrDefault(x => x.Id == _last.RacerId));
            }
            if (previous.Any(x => x.Id == -1))
            {
                var _bye = new Racer();
                _bye.Id = -1;
                _bye.Name = "BYE";
                _bye.Finished = false;
                previous.Remove(_bye);
            }

            List<Racer> leftOver = allRacers.Where(r => previous.All(c => c.Id != r.Id)).ToList();
            List<Racer> remainingRacers = new List<Racer>();
            remainingRacers.AddRange(allRacers.Where(r => leftOver.All(c => c.Id != r.Id)).ToList());

            //List<Racer> topNRacers = new List<Racer>();
            //remainingRacers.Shuffle();
            while (leftOver.Count < laneCount)
            {
                foreach (var racer in remainingRacers)
                {                        
                    var previousRaces = usedContestants.Where(x => x.RacerId == racer.Id).OrderBy(x => x.Lane).ToList();
                    if (previousRaces.Count() == (heatCount - 1))
                    {
                        continue;
                    }

                    if (leftOver.Any(x => x.Id == racer.Id))
                    {

                        //var _bye = new Racer(-1);
                        //leftOver.Add(_bye);
                        continue;
                    }


                    Debug.WriteLine(racer.Name);
                    leftOver.Add(racer);
                }
            }

            //leftOver.Shuffle();
            foreach (var lane in lanes)
            {
                var _racer = leftOver.Take(1).First();

                if (lineUp.Any(x => x.Scout.Id == _racer.Id))
                    continue;

                lineUp.Add(LoadLane(_racer, lane, heatId));
                allRacers.Remove(_racer);
                leftOver.Remove(_racer);
            }

            return lineUp;
        } 

        private static List<Lane> FillLineup(List<Racer> allRacers, int heatId)
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
            return new Lane() { Scout = _racer, LaneNumber = lane };
        }
    }
}
