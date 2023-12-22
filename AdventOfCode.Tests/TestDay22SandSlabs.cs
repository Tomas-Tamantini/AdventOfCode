namespace AdventOfCode.Tests
{
    public class TestDay22SandSlabs
    {
        private static SandBrick MakeBrick(string id = "brick id", int x = 0, int y = 0, int z = 0, int width = 1, int height = 1, int depth = 1)
        {
            return new(id, new Coordinates(x, y, z), width, height, depth);
        }

        [Fact]
        public void TestCanLoopThroughCoordinatesOfBrickBottomCubes()
        {
            SandBrick brick = MakeBrick(x: 10, y: 17, z: 19, width: 2, height: 1, depth: 1);
            HashSet<Coordinates> expectedCoordinates = new()
            {
                new Coordinates(10, 17, 19),
                new Coordinates(11, 17, 19),
            };
            IEnumerable<Coordinates> actualCoordinates = brick.BottomCoordinates();
            Assert.Equal(expectedCoordinates, actualCoordinates.ToHashSet());
        }

        [Fact]
        public void TestCanLoopThroughCoordinatesOfBrickTopCubes()
        {
            SandBrick brick = MakeBrick(x: 10, y: 17, z: 19, height: 10, depth: 3);
            HashSet<Coordinates> expectedCoordinates = new()
            {
                new Coordinates(10, 17, 28),
                new Coordinates(10, 18, 28),
                new Coordinates(10, 19, 28),
            };
            IEnumerable<Coordinates> actualCoordinates = brick.TopCoordinates();
            Assert.Equal(expectedCoordinates, actualCoordinates.ToHashSet());
        }

        [Fact]
        public void TestCanResetCubeZCoordinate()
        {
            SandBrick brick = MakeBrick(x: 10, y: 17, z: 19, height: 10, depth: 3);
            brick.ResetZCoordinate(3);
            HashSet<Coordinates> expectedCoordinates = new()
            {
                new Coordinates(10, 17, 12),
                new Coordinates(10, 18, 12),
                new Coordinates(10, 19, 12),
            };
            IEnumerable<Coordinates> actualCoordinates = brick.TopCoordinates();
            Assert.Equal(expectedCoordinates, actualCoordinates.ToHashSet());
        }

        [Fact]
        public void TestFirstBrickLayedOnPileDoesNotTouchAnyOther()
        {
            BrickPile pile = new();
            SandBrick brick = MakeBrick(x: 10, y: 17, z: 19, height: 10, depth: 3);
            HashSet<string> supportingBricksIds = pile.PlaceBrickAndReturnSupportingBrickIds(brick);
            Assert.Empty(supportingBricksIds);
            Assert.True(brick.BottomCoordinates().All(c => c.Z == 1));
        }

        [Fact]
        public void TestBrickLayedDirectlyOnGroudDoesNotTouchAnyOther()
        {
            BrickPile pile = new();
            SandBrick firstBrick = MakeBrick(x: 10, y: 17, z: 1, depth: 3);
            pile.PlaceBrickAndReturnSupportingBrickIds(firstBrick);
            SandBrick secondBrick = MakeBrick(x: 9, y: 16, z: 5, depth: 2);
            HashSet<string> supportingBricksIds = pile.PlaceBrickAndReturnSupportingBrickIds(secondBrick);
            Assert.Empty(supportingBricksIds);
            Assert.True(secondBrick.BottomCoordinates().All(c => c.Z == 1));
        }

        [Fact]
        public void TestBrickLayedOnTopOfAnotherBrickIsSupportedByThatBrick()
        {
            BrickPile pile = new();
            SandBrick firstBrick = MakeBrick(id: "B1", x: 10, y: 17, z: 4, height: 5, depth: 3);
            pile.PlaceBrickAndReturnSupportingBrickIds(firstBrick);
            SandBrick secondBrick = MakeBrick(id: "B2", x: 9, y: 17, z: 7, height: 3, width: 2);
            HashSet<string> supportingBricksIds = pile.PlaceBrickAndReturnSupportingBrickIds(secondBrick);
            Assert.Equal(new HashSet<string> { "B1" }, supportingBricksIds);
            Assert.True(secondBrick.TopCoordinates().All(c => c.Z == 8));
        }

        [Fact]
        public void TestBrickLayedOnTopOfTwoBricksIsSupportedByBoth()
        {
            BrickPile pile = new();
            SandBrick baseBrickOne = MakeBrick(id: "B1", x: 0, y: 0, z: 0, height: 5);
            pile.PlaceBrickAndReturnSupportingBrickIds(baseBrickOne);
            SandBrick baseBrickTwo = MakeBrick(id: "B2", x: 0, y: 3, z: 0, height: 3);
            pile.PlaceBrickAndReturnSupportingBrickIds(baseBrickTwo);
            SandBrick baseBrickThree = MakeBrick(id: "B3", x: 0, y: 5, z: 0, height: 5);
            pile.PlaceBrickAndReturnSupportingBrickIds(baseBrickThree);

            SandBrick brick = MakeBrick(id: "B4", x: 0, y: 0, z: 5, depth: 10, height: 2);
            HashSet<string> supportingBricksIds = pile.PlaceBrickAndReturnSupportingBrickIds(brick);
            Assert.Equal(new HashSet<string> { "B1", "B3" }, supportingBricksIds);
            Assert.True(brick.TopCoordinates().All(c => c.Z == 7));
        }

        private static SandSlabs ExampleSlabs()
        {
            List<SandBrick> bricksSnapshot = new()
            {
                MakeBrick(id: "A", x: 1, y: 0, z: 1, depth: 3),
                MakeBrick(id: "B", x: 0, y: 0, z: 2, width: 3),
                MakeBrick(id: "C", x: 0, y: 2, z: 3, width: 3),
                MakeBrick(id: "D", x: 0, y: 0, z: 4, depth: 3),
                MakeBrick(id: "E", x: 2, y: 0, z: 5, depth: 3),
                MakeBrick(id: "F", x: 0, y: 1, z: 6, width: 3),
                MakeBrick(id: "G", x: 1, y: 1, z: 8, height: 2),
            };
            return new SandSlabs(bricksSnapshot);
        }

        [Fact]
        public void TestSandSlabsKeepsTrackOfWhichBricksSupportWhich()
        {
            SandSlabs slabs = ExampleSlabs();
            slabs.DropBricks();
            Assert.Equal(new HashSet<string> { "B", "C" }, slabs.BricksSupportedBy("A").ToHashSet());
            Assert.Equal(new HashSet<string> { "D", "E" }, slabs.BricksSupportedBy("B").ToHashSet());
            Assert.Equal(new HashSet<string> { "D", "E" }, slabs.BricksSupportedBy("C").ToHashSet());
            Assert.Equal(new HashSet<string> { "F" }, slabs.BricksSupportedBy("D").ToHashSet());
            Assert.Equal(new HashSet<string> { "F" }, slabs.BricksSupportedBy("E").ToHashSet());
            Assert.Equal(new HashSet<string> { "G" }, slabs.BricksSupportedBy("F").ToHashSet());
            Assert.Empty(slabs.BricksSupportedBy("G"));
        }

        [Fact]
        public void TestSandSlabsKeepsTrackOfWhichBricksAreSupportedByWhich()
        {
            SandSlabs slabs = ExampleSlabs();
            slabs.DropBricks();
            Assert.Empty(slabs.BricksSupporting("A"));
            Assert.Equal(new HashSet<string> { "A" }, slabs.BricksSupporting("B").ToHashSet());
            Assert.Equal(new HashSet<string> { "A" }, slabs.BricksSupporting("C").ToHashSet());
            Assert.Equal(new HashSet<string> { "B", "C" }, slabs.BricksSupporting("D").ToHashSet());
            Assert.Equal(new HashSet<string> { "B", "C" }, slabs.BricksSupporting("E").ToHashSet());
            Assert.Equal(new HashSet<string> { "D", "E" }, slabs.BricksSupporting("F").ToHashSet());
            Assert.Equal(new HashSet<string> { "F" }, slabs.BricksSupporting("G").ToHashSet());

        }

        [Fact]
        public void TestSandSlabsInformsBrickWhichCanBeDisintegratedSafely()
        {
            SandSlabs slabs = ExampleSlabs();
            slabs.DropBricks();
            IEnumerable<string> safeBricksIds = slabs.SafeToDisintegrateBrickIds();
            Assert.Equal(new HashSet<string> { "B", "C", "D", "E", "G" }, safeBricksIds.ToHashSet());
        }
    }
}