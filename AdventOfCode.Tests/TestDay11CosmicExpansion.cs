namespace AdventOfCode.Tests
{
    public class TestDay11CosmicExpansion
    {
        [Fact]
        public void TestDistancesBetweenGalaxiesAreCalculatedUsingManhattanDistance()
        {
            List<(int, int)> galaxies = new() { (0, 0), (1, 1) };
            CosmicExpansion expansion = new(universeWidth: 2, universeHeight: 2, galaxies);
            Assert.Equal(2, expansion.DistanceBetweenGalaxies(0, 1));
        }

        [Fact]
        public void TestCrossingEmptyRowsTakeTwoSteps()
        {
            List<(int, int)> galaxies = new() { (0, 0), (1, 2) };
            CosmicExpansion expansion = new(universeWidth: 2, universeHeight: 3, galaxies);
            Assert.Equal(4, expansion.DistanceBetweenGalaxies(0, 1));
        }

        [Fact]
        public void TestCrossingEmptyColumnsTakeTwoSteps()
        {
            List<(int, int)> galaxies = new() { (0, 0), (2, 1) };
            CosmicExpansion expansion = new(universeWidth: 3, universeHeight: 2, galaxies);
            Assert.Equal(4, expansion.DistanceBetweenGalaxies(0, 1));
        }

        [Fact]
        public void TestCanSumDistancesBetweenAllPairsOfGalaxies()
        {
            List<(int, int)> galaxies = new() { (3, 0), (7, 1), (0, 2), (6, 4), (1, 5), (9, 6), (7, 8), (0, 9), (4, 9) };
            CosmicExpansion expansion = new(universeWidth: 10, universeHeight: 10, galaxies);
            Assert.Equal(374, expansion.SumDistancesBetweenAllPairsOfGalaxies());
        }
    }
}