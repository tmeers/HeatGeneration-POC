namespace HeatGeneration
{
    internal class HeatCount
    {
        public static int CalculateHeatCount(int laneCount, int racersCount, int raceCount)
        {
            return (racersCount * laneCount) / raceCount;
        }
    }
}
