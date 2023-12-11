namespace AdventOfCode.Tests
{

    public class TestDay10PipeMaze
    {
        private const string simpleMaze = @".....
                                            .S-7.
                                            .|.|.
                                            .L-J.
                                            .....";
        private const string complexMaze = @"7-F7-
                                             .FJ|7
                                             SJLL7
                                             |F--J
                                             LJ.LJ";
        [Fact]
        public void TestLoopPassingThroughStartingPositionIsDetected()
        {
            PipeMaze maze = new(simpleMaze);
            Assert.Equal(8, maze.LoopLength());
            Assert.Equal(4, maze.LoopArea());
        }

        [Fact]
        public void TestLoopIsDetectedEvenWithNonLoopElementsAround()
        {
            PipeMaze maze = new(complexMaze);
            Assert.Equal(16, maze.LoopLength());
            Assert.Equal(8, maze.LoopArea());
        }

        [Fact]
        public void TestCanCountPointsStrictlyInsideLoop()
        {
            Assert.Equal(1, new PipeMaze(simpleMaze).NumPointsInsideLoop());
            Assert.Equal(1, new PipeMaze(complexMaze).NumPointsInsideLoop());
        }
    }
}