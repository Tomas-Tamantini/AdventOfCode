namespace AdventOfCode.Tests
{
    public class TestDay24Hailstones
    {
        [Fact]
        public void TestCanFindIntersectionOfTwoHailstonePathsOnXYAxis()
        {
            Hailstone h1 = new(Pos: (10, 20, 30), Vel: (2, -2, 0));
            Hailstone h2 = new(Pos: (7, 8, -5), Vel: (1, 2, 3));
            (double, double) xyIntersection = Hailstones.XYIntersection(h1, h2);
            Assert.Equal((12, 18), xyIntersection);
        }

        [Fact]
        public void TestParallelHailstonesDoNotIntersect()
        {
            Hailstone h1 = new(Pos: (10, 20, 30), Vel: (2, -2, 0));
            Hailstone h2 = new(Pos: (10, 40, 30), Vel: (2, -2, 0));
            (double, double) xyIntersection = Hailstones.XYIntersection(h1, h2);
            Assert.Equal((double.NaN, double.NaN), xyIntersection);
        }

        [Fact]
        public void TestCanTellWhenTwoPathsAreCoincident()
        {
            Hailstone h1 = new(Pos: (10, 20, 30), Vel: (2, -2, 0));
            Hailstone h2 = new(Pos: (110, -80, 19), Vel: (-15, 15, 0));
            Hailstone h3 = new(Pos: (7, 8, -5), Vel: (1, 2, 3));
            Hailstone h4 = new(Pos: (10, 40, 30), Vel: (2, -2, 0));
            Assert.True(Hailstones.XYPathsAreCoincident(h1, h2));
            Assert.False(Hailstones.XYPathsAreCoincident(h1, h3));
            Assert.False(Hailstones.XYPathsAreCoincident(h1, h4));
        }

        [Fact]
        public void TestCanCheckIfNonCoincidentPathsWillCrossWithinBoundingBoxInTheFuture()
        {
            Hailstone h1 = new(Pos: (10, 20, 30), Vel: (2, -2, 0));
            Hailstone h2 = new(Pos: (7, 8, -5), Vel: (1, 2, 3));
            BoundingBox inclusiveBoundingBox = new(Min: (0, 0), Max: (100, 100));
            BoundingBox exclusiveBoundingBox = new(Min: (-10, -10), Max: (0, 0));
            Hailstones hailstonesInclusive = new(new List<Hailstone> { }, inclusiveBoundingBox);
            Hailstones hailstonesExclusive = new(new List<Hailstone> { }, exclusiveBoundingBox);
            Assert.True(hailstonesInclusive.XYPathsWillCrossWithinBoundingBoxInTheFuture(h1, h2));
            Assert.False(hailstonesExclusive.XYPathsWillCrossWithinBoundingBoxInTheFuture(h1, h2));
        }

        [Fact]
        public void TestIfXYPathIntersectionHappenedInThePastForSomeStoneItDoesNotCount()
        {
            Hailstone h1 = new(Pos: (14, 16, 30), Vel: (2, -2, 0));
            Hailstone h2 = new(Pos: (7, 8, -5), Vel: (1, 2, 3));
            BoundingBox boundingBox = new(Min: (0, 0), Max: (100, 100));
            Hailstones hailstones = new(new List<Hailstone> { }, boundingBox);
            Assert.False(hailstones.XYPathsWillCrossWithinBoundingBoxInTheFuture(h1, h2));
        }

        [Fact(Skip = "Not implemented yet")]
        public void TestCoincidentPathsMustHaveIntersectionWithinBoundingBoxSuchThatTimeForBothStonesIsInTheFuture()
        {
            Hailstone h1 = new(Pos: (10, 5, 0), Vel: (2, 0, 0));
            Hailstone h2 = new(Pos: (20, 5, 0), Vel: (-7, 0, 0));
            BoundingBox inclusiveBoundingBox = new(Min: (9, 0), Max: (21, 10));
            BoundingBox exclusiveBoundingBox = new(Min: (8, -10), Max: (9, 0));
            Hailstones hailstonesInclusive = new(new List<Hailstone> { }, inclusiveBoundingBox);
            Hailstones hailstonesExclusive = new(new List<Hailstone> { }, exclusiveBoundingBox);
            Assert.True(hailstonesInclusive.XYPathsWillCrossWithinBoundingBoxInTheFuture(h1, h2));
            Assert.False(hailstonesExclusive.XYPathsWillCrossWithinBoundingBoxInTheFuture(h1, h2));
        }

        [Fact]
        public void TestCanCountTotalNumberOfFutureXYIntersectingPaths()
        {
            List<Hailstone> hailstones = new()
            {
                new(Pos: (19, 13, 30), Vel: (-2, 1, -2)),
                new(Pos: (18, 19, 22), Vel: (-1, -1, -2)),
                new(Pos: (20, 25, 34), Vel: (-2, -2, -4)),
                new(Pos: (12, 31, 28), Vel: (-1, -2, -1)),
                new(Pos: (20, 19, 15), Vel: (1, -5, -3)),
            };
            BoundingBox boundingBox = new(Min: (7, 7), Max: (27, 27));
            Hailstones hailstonesSimulator = new(hailstones, boundingBox);
            int numXYIntersections = hailstonesSimulator.NumFutureXYIntersections();
            Assert.Equal(2, numXYIntersections);
        }
    }
}