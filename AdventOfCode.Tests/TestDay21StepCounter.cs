namespace AdventOfCode.Tests
{
    public class TestDay21StepCounter
    {
        [Fact]
        public void TestGardenPlotsAreValidNeighbors()
        {
            string gardenStr = @"...
                                 ...
                                 ...";
            Garden garden = new(gardenStr);
            IEnumerable<(int, int)> neighbors = garden.Neighbors((1, 1));
            Assert.Equal(4, neighbors.Count());
            Assert.Contains((0, 1), neighbors);
            Assert.Contains((1, 0), neighbors);
            Assert.Contains((1, 2), neighbors);
            Assert.Contains((2, 1), neighbors);
        }

        [Fact]
        public void TestPositionsOutsideGardenAreNotValidNeighbors()
        {
            string gardenStr = @"...
                                 ...
                                 ...";
            Garden garden = new(gardenStr);
            IEnumerable<(int, int)> neighbors = garden.Neighbors((2, 2));
            Assert.Equal(2, neighbors.Count());
            Assert.Contains((1, 2), neighbors);
            Assert.Contains((2, 1), neighbors);
        }

        [Fact]
        public void TestRocksAreNotValidNeighbors()
        {
            string gardenStr = @"...
                                 .#.
                                 ...";
            Garden garden = new(gardenStr);
            IEnumerable<(int, int)> neighbors = garden.Neighbors((1, 2));
            Assert.Equal(2, neighbors.Count());
            Assert.Contains((0, 2), neighbors);
            Assert.Contains((2, 2), neighbors);
        }

        [Fact]
        public void TestPositionOutsideGardenIsValidPacmanNeighbor()
        {
            string gardenStr = @"...
                                 ...
                                 #..";
            Garden garden = new(gardenStr);
            IEnumerable<(int, int)> neighbors = garden.PacmanNeighbors((0, 0));
            Assert.Equal(3, neighbors.Count());
            Assert.Contains((0, 1), neighbors);
            Assert.Contains((1, 0), neighbors);
            Assert.Contains((-1, 0), neighbors);
        }

        [Fact]
        public void TestGardenerCanStepInAllValidDirections()
        {
            string gardenStr = @"...
                                 #S.
                                 ...";
            Garden garden = new(gardenStr);
            StepCounter stepCounter = new(garden);
            HashSet<(int, int)> expectedPositions = new()
            {
                (1, 0),
                (1, 2),
                (2, 1),
            };
            HashSet<(int, int)> possiblePositions = stepCounter.PossiblePositionsAfterNSteps(numSteps: 1);
            Assert.Equal(expectedPositions, possiblePositions);
        }

        [Fact]
        public void TestGardenerCanStepIntoPreviouslyVisitedTile()
        {
            string gardenStr = @"S.";
            Garden garden = new(gardenStr);
            StepCounter stepCounter = new(garden);
            HashSet<(int, int)> expectedPositions = new()
            {
                (0, 0),
            };
            HashSet<(int, int)> possiblePositions = stepCounter.PossiblePositionsAfterNSteps(numSteps: 2);
            Assert.Equal(expectedPositions, possiblePositions);
        }

        [Fact]
        public void TestKeepsTrackOfPossiblePositionsAfterMultipleSteps()
        {
            string gardenStr = @"...........
                                 .....###.#.
                                 .###.##..#.
                                 ..#.#...#..
                                 ....#.#....
                                 .##..S####.
                                 .##..#...#.
                                 .......##..
                                 .##.#.####.
                                 .##..##.##.
                                 ...........";

            Garden garden = new(gardenStr);
            StepCounter stepCounter = new(garden);
            HashSet<(int, int)> possiblePositions = stepCounter.PossiblePositionsAfterNSteps(numSteps: 6);
            Assert.Equal(16, possiblePositions.Count);
        }

        [Fact]
        public void TestCanListNumberOfPossiblePositionsAfterEachStepInPacmanGarden()
        {
            string gardenStr = @"...........
                                 .....###.#.
                                 .###.##..#.
                                 ..#.#...#..
                                 ....#.#....
                                 .##..S####.
                                 .##..#...#.
                                 .......##..
                                 .##.#.####.
                                 .##..##.##.
                                 ...........";

            Garden garden = new(gardenStr);
            StepCounter stepCounter = new(garden);
            IEnumerable<long> numPossiblePositions = stepCounter.NumPossiblePositionsInPacmanGarden(totalSteps: 10);
            List<long> expectedNumPositions = new() { 1, 2, 4, 6, 9, 13, 16, 22, 30, 41, 50 };
            Assert.Equal(expectedNumPositions, numPossiblePositions.ToList());
        }
    }
}