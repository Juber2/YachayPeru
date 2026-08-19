namespace YachayPeru.Application.Common
{
    public static class AprendizLevelCalculator
    {
        private const int PointsPerLevel = 100;

        public static int CalculateLevel(int points) => points / PointsPerLevel + 1;

        public static int NextLevelPoints(int level) => level * PointsPerLevel;
    }
}
