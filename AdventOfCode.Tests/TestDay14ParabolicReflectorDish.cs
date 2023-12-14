namespace AdventOfCode.Tests
{
    public class TestDay14ParabolicReflectorDish
    {
        private static string TrimInput(string input)
        {
            string[] lines = input.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }
            return string.Join(Environment.NewLine, lines);
        }
        [Fact]
        public void TestColumnWithNoRoundRocksDoesNotChange()
        {
            string noRoundRocksInput = @".
                                         #
                                         .";
            ParabolicReflectorDish dish = new(noRoundRocksInput);
            dish.RollNorth();
            Assert.Equal(TrimInput(noRoundRocksInput), dish.ToString());
        }

        [Fact]
        public void TestRoundRockOnFirstRowDoesNotMove()
        {
            string roundRockOnFirstRowInput = @"O
                                                .
                                                .";
            ParabolicReflectorDish dish = new(roundRockOnFirstRowInput);
            dish.RollNorth();
            Assert.Equal(TrimInput(roundRockOnFirstRowInput), dish.ToString());
        }

        [Fact]
        public void TestCubeRockActsAsFixedObstacle()
        {
            string roundRockWithObstacleAbove = @".
                                                  #
                                                  O
                                                  .";
            ParabolicReflectorDish dish = new(roundRockWithObstacleAbove);
            dish.RollNorth();
            Assert.Equal(TrimInput(roundRockWithObstacleAbove), dish.ToString());
        }

        [Fact]
        public void TestRoundRockActsAsObstacleToOtherRoundRocks()
        {
            string roundRockWithObstacleAbove = @"O
                                                  O
                                                  O
                                                  .";
            ParabolicReflectorDish dish = new(roundRockWithObstacleAbove);
            dish.RollNorth();
            Assert.Equal(TrimInput(roundRockWithObstacleAbove), dish.ToString());
        }

        [Fact]
        public void UnimpededRoundedRockRollsNorth()
        {
            string unimpededRockInput = @".
                                          .
                                          .
                                          O";
            string expected = @"O
                                .
                                .
                                .";
            ParabolicReflectorDish dish = new(unimpededRockInput);
            dish.RollNorth();
            Assert.Equal(TrimInput(expected), dish.ToString());
        }

        [Fact]
        public void RoundRockRollsNorthUntilItHitsObstacle()
        {
            string roundRockWithObstacleAbove = @".
                                                 #
                                                 .
                                                 O
                                                 O";
            string expected = @".
                                #
                                O
                                O
                                .";
            ParabolicReflectorDish dish = new(roundRockWithObstacleAbove);
            dish.RollNorth();
            Assert.Equal(TrimInput(expected), dish.ToString());
        }

        [Fact]
        public void RocksRollInATwoDimensionalPlane()
        {
            string inputTwoD = @"O....#....
                                 O.OO#....#
                                 .....##...
                                 OO.#O....O
                                 .O.....O#.
                                 O.#..O.#.#
                                 ..O..#O..O
                                 .......O..
                                 #....###..
                                 #OO..#....";

            string expected = @"OOOO.#.O..
                                OO..#....#
                                OO..O##..O
                                O..#.OO...
                                ........#.
                                ..#....#.#
                                ..O..#.O.O
                                ..O.......
                                #....###..
                                #....#....";
            ParabolicReflectorDish dish = new(inputTwoD);
            dish.RollNorth();
            Assert.Equal(TrimInput(expected), dish.ToString());
        }

        [Fact]
        public void TestCanCountNumberOfRoundRocksPerRow()
        {
            string input = @"O....#
                             O.OO#.
                             .....#
                             OO.#..";
            ParabolicReflectorDish dish = new(input);
            int[] numRoundRocks = dish.RoundRocksPerRow().ToArray();
            int[] expected = { 1, 3, 0, 2 };
            Assert.Equal(expected, numRoundRocks);
        }

        [Fact]
        public void TestTorqueOnSouthHingeIsLoadTimesDistanceToBottom()
        {
            string input = @"OOOO.#.O..
                                OO..#....#
                                OO..O##..O
                                O..#.OO...
                                ........#.
                                ..#....#.#
                                ..O..#.O.O
                                ..O.......
                                #....###..
                                #....#....";
            ParabolicReflectorDish dish = new(input);
            Assert.Equal(136, dish.TorqueOnSouthHinge());
        }
    }
}