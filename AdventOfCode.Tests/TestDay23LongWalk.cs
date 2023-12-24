namespace AdventOfCode.Tests
{
    public class TestDay23LongWalk
    {
        private static LongWalk CreateLongWalk(string input)
        {

            Forest forest = new(input);
            return new LongWalk(forest);
        }
        [Fact]
        public void TestHikeWithSingleTileHasPathOfLengthOne()
        {
            string forestStr = @".";
            LongWalk walk = CreateLongWalk(forestStr);
            Assert.Equal(1, walk.LengthLongestPath());
        }

        [Fact]
        public void TestHikeStartsOnTopHikeTileAndEndsInBottomHikeTile()
        {
            string forestStr = @".##
                                 ...
                                 ##.";
            LongWalk walk = CreateLongWalk(forestStr);
            Assert.Equal(5, walk.LengthLongestPath());
        }

        [Fact]
        public void TestHikeCannotGoThroughForest()
        {
            string forestStr = @".
                                 #
                                 .";
            LongWalk walk = CreateLongWalk(forestStr);
            Assert.Equal(-1, walk.LengthLongestPath());
        }

        [Fact]
        public void TestHikePicksLongestNonSelfIntersectingPath()
        {
            string forestStr = @"#.#
                                 ...
                                 ...
                                 ...
                                 #.#";
            LongWalk walk = CreateLongWalk(forestStr);
            Assert.Equal(9, walk.LengthLongestPath());
        }

        [Fact]
        public void TestSlopesCanOnlyBeCrossedDownHill()
        {
            string forestStr = @"#.##
                                 #...
                                 #v#^
                                 #...
                                 #.##";
            LongWalk walk = CreateLongWalk(forestStr);
            Assert.Equal(5, walk.LengthLongestPath());
        }

        private static LongWalk WalkExample()
        {
            string forestStr = @"#.#####################
                                 #.......#########...###
                                 #######.#########.#.###
                                 ###.....#.>.>.###.#.###
                                 ###v#####.#v#.###.#.###
                                 ###.>...#.#.#.....#...#
                                 ###v###.#.#.#########.#
                                 ###...#.#.#.......#...#
                                 #####.#.#.#######.#.###
                                 #.....#.#.#.......#...#
                                 #.#####.#.#.#########v#
                                 #.#...#...#...###...>.#
                                 #.#.#v#######v###.###v#
                                 #...#.>.#...>.>.#.###.#
                                 #####v#.#.###v#.#.###.#
                                 #.....#...#...#.#.#...#
                                 #.#########.###.#.#.###
                                 #...###...#...#...#.###
                                 ###.###.#.###v#####v###
                                 #...#...#.#.>.>.#.>.###
                                 #.###.###.#.###.#.#v###
                                 #.....###...###...#...#
                                 #####################.#";
            return CreateLongWalk(forestStr);
        }

        [Fact]
        public void TestLongestPathRunsEfficiently()
        {

            LongWalk walk = WalkExample();
            Assert.Equal(95, walk.LengthLongestPath());
        }

        [Fact]
        public void TestCanFindLongestPathIgnoringSlopes()
        {
            LongWalk walk = WalkExample();
            Assert.Equal(155, walk.LengthLongestPathIgnoringSlopes());
        }
    }
}