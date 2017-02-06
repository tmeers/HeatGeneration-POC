namespace HeatGeneration
{
    internal class HeatCount
    {
        public static int CalculateHeatCount(int laneCount, int racersCount, int raceCount)
        {
            return (racersCount * laneCount) / raceCount;
        }

        public static int GenerateHeatCount(int laneCount, int racersCount)
        {
            if (laneCount == 3 && (racersCount <= 2))
                return laneCount;

            if (laneCount == 3 && (racersCount >= 3))
                return racersCount;

            if (laneCount == 4)
                return racersCount;

            if (laneCount == 6 && ((racersCount <= 7 && racersCount > 3) || racersCount == 9 || racersCount == 10 || racersCount == 12))
                return racersCount;

            if (laneCount == 6 && (racersCount <= 3))
                return 4;

            return laneCount;
        }
    }
}
