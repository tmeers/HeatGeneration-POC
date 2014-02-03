using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HeatGeneration.Helpers
{
    internal class LaneGen
    {
        public static ICollection<int> GetLanes(int laneCount)
        {
            ICollection<int> lanes = new Collection<int>();
            for (int i = 1; i <= laneCount; i++)
            {
                lanes.Add(i);
            }
            return lanes;
        }
    }
}
