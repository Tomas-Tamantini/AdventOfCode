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
        public void TestCrossingEmptyRowsTakesTwoStepsByDefult()
        {
            List<(int, int)> galaxies = new() { (0, 0), (1, 2) };
            CosmicExpansion expansion = new(universeWidth: 2, universeHeight: 3, galaxies);
            Assert.Equal(4, expansion.DistanceBetweenGalaxies(0, 1));
        }

        [Fact]
        public void TestCrossingEmptyColumnsTakedTwoStepsByDefault()
        {
            List<(int, int)> galaxies = new() { (0, 0), (2, 1) };
            CosmicExpansion expansion = new(universeWidth: 3, universeHeight: 2, galaxies);
            Assert.Equal(4, expansion.DistanceBetweenGalaxies(0, 1));
        }

        [Fact]
        public void TestExpansionRateCanBeDifferentThanDefaultTwo()
        {
            List<(int, int)> galaxies = new() { (0, 0), (1, 2) };
            CosmicExpansion expansion = new(universeWidth: 2, universeHeight: 3, galaxies);
            Assert.Equal(7, expansion.DistanceBetweenGalaxies(0, 1, expansionRate: 5));
        }

        [Fact]
        public void TestCanSumDistancesBetweenAllPairsOfGalaxies()
        {
            List<(int, int)> galaxies = new() { (3, 0), (7, 1), (0, 2), (6, 4), (1, 5), (9, 6), (7, 8), (0, 9), (4, 9) };
            CosmicExpansion expansion = new(universeWidth: 10, universeHeight: 10, galaxies);
            Assert.Equal(374, expansion.SumDistancesBetweenAllPairsOfGalaxies(expansionRate: 2));
            Assert.Equal(1030, expansion.SumDistancesBetweenAllPairsOfGalaxies(expansionRate: 10));
            Assert.Equal(8410, expansion.SumDistancesBetweenAllPairsOfGalaxies(expansionRate: 100));
        }
    }
}