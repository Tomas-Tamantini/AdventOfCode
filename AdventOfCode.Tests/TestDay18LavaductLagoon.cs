namespace AdventOfCode.Tests
{
    public class TestDay18LavaductLagoon
    {
        [Fact]
        public void TestEmptyDigPlanResultsInOneMeterCubedLagoon()
        {
            IEnumerable<DigCommand> digPlan = Enumerable.Empty<DigCommand>();
            var lagoon = new LavaductLagoon(digPlan);
            Assert.Equal(1, lagoon.Volume());
        }

        [Fact]
        public void TestDigPlanWithSingleStepDigdResultsInTwoMeterCubedLagoon()
        {
            IEnumerable<DigCommand> digPlan = new List<DigCommand> { new(CardinalDirection.North, 1) };
            var lagoon = new LavaductLagoon(digPlan);
            Assert.Equal(2, lagoon.Volume());
        }

        [Fact]
        public void TestLagoonVolumeTakesIntoAccountInteriorOfShape()
        {
            IEnumerable<DigCommand> digPlan = new List<DigCommand> {
                new(CardinalDirection.North, 2),
                new(CardinalDirection.East, 2),
                new(CardinalDirection.South, 2),
                new(CardinalDirection.West, 2)
            };
            var lagoon = new LavaductLagoon(digPlan);
            Assert.Equal(9, lagoon.Volume());
        }

        [Fact]
        public void TestLagoonVolumeWorksWithNonConvexShapes()
        {
            IEnumerable<DigCommand> digPlan = new List<DigCommand> {
                new(CardinalDirection.East, 6),
                new(CardinalDirection.South, 5),
                new(CardinalDirection.West, 2),
                new(CardinalDirection.South, 2),
                new(CardinalDirection.East, 2),
                new(CardinalDirection.South, 2),
                new(CardinalDirection.West, 5),
                new(CardinalDirection.North, 2),
                new(CardinalDirection.West, 1),
                new(CardinalDirection.North, 2),
                new(CardinalDirection.East, 2),
                new(CardinalDirection.North, 3),
                new(CardinalDirection.West, 2),
                new(CardinalDirection.North, 2),
            };
            var lagoon = new LavaductLagoon(digPlan);
            Assert.Equal(62, lagoon.Volume());
        }
    }
}