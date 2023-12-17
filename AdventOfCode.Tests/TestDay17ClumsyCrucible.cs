namespace AdventOfCode.Tests
{
    public class TestDay17ClumsyCrucible
    {
        [Fact]
        public void TestCrucibleLosesNoHeatIfOriginEqualsDestination()
        {
            string cityBlocks = "3";
            ClumsyCrucible crucible = new(cityBlocks);
            Assert.Equal(0, crucible.MinimumHeatLoss(start: (0, 0), destination: (0, 0)));
        }

        [Fact]
        public void TestCrucibleChoosesPathWithLeastHeatLoss()
        {
            string cityBlocks = @"12
                                  43";
            ClumsyCrucible crucible = new(cityBlocks);
            Assert.Equal(5, crucible.MinimumHeatLoss(start: (0, 0), destination: (1, 1)));
        }

        [Fact]
        public void TestCrucibleCannotGoInTheSameDirectionForMoreThanGivenNumberOfSteps()
        {
            string cityBlocks = @"11111
                                  55555";
            CrucibleInertia inertia = new(MinStepsSameDirection: null, MaxStepsSameDirection: 3);
            ClumsyCrucible crucible = new(cityBlocks, inertia);
            Assert.Equal(14, crucible.MinimumHeatLoss(start: (0, 0), destination: (4, 0)));
        }

        [Fact]
        public void TestIfNoPossiblePathsHeatLossIsMinus1()
        {
            string cityBlocks = @"11111";
            CrucibleInertia inertia = new(MinStepsSameDirection: null, MaxStepsSameDirection: 3);
            ClumsyCrucible crucible = new(cityBlocks, inertia);
            Assert.Equal(-1, crucible.MinimumHeatLoss(start: (0, 0), destination: (4, 0)));
        }

        [Fact]
        public void TestCrucibleStartPointIsTopLeftAndEndPointIsBottomRightByDefault()
        {
            string cityBlocks = @"2413432311323
                                  3215453535623
                                  3255245654254
                                  3446585845452
                                  4546657867536
                                  1438598798454
                                  4457876987766
                                  3637877979653
                                  4654967986887
                                  4564679986453
                                  1224686865563
                                  2546548887735
                                  4322674655533";
            CrucibleInertia inertia = new(MinStepsSameDirection: null, MaxStepsSameDirection: 3);
            ClumsyCrucible crucible = new(cityBlocks, inertia);
            Assert.Equal(102, crucible.MinimumHeatLoss());
        }

        [Fact]
        public void TestCrucibleCannotTakeFewerStepsInTheSameDirectionAsItsInertiaLimit()
        {
            string cityBlocks = @"2413432311323
                                  3215453535623
                                  3255245654254
                                  3446585845452
                                  4546657867536
                                  1438598798454
                                  4457876987766
                                  3637877979653
                                  4654967986887
                                  4564679986453
                                  1224686865563
                                  2546548887735
                                  4322674655533";
            CrucibleInertia inertia = new(MinStepsSameDirection: 4, MaxStepsSameDirection: 10);
            ClumsyCrucible crucible = new(cityBlocks, inertia);
            Assert.Equal(94, crucible.MinimumHeatLoss());
        }
    }
}