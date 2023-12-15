namespace AdventOfCode.Tests
{
    public class TestDay15LensLibrary
    {
        static Lens BuildLens(string label, int focalLenght = 1)
        {
            return new Lens { Label = label, FocalLenght = focalLenght };
        }

        [Fact]
        public void TestHashValueIsCalculatedProperly()
        {
            Assert.Equal(52, LensLibrary.GetHash("HASH"));
        }

        [Fact]
        public void TestLibraryStartsWithAllBoxesEmpty()
        {
            LensLibrary lensLibrary = new();
            foreach (var box in lensLibrary.Boxes)
            {
                Assert.Empty(box);
            }
        }

        [Fact]
        public void TestLensGetsAddedToProperBox()
        {
            Lens lens = BuildLens("HASH");
            LensLibrary lensLibrary = new();
            lensLibrary.Upsert(lens);
            Assert.Single(lensLibrary.Boxes[52]);
        }

        [Fact]
        public void TestLensGetsAddedToEndOfItsBox()
        {
            Lens lens1 = BuildLens("rn");
            Lens lens2 = BuildLens("cm");
            LensLibrary lensLibrary = new();
            lensLibrary.Upsert(lens1);
            lensLibrary.Upsert(lens2);
            Assert.Equal(2, lensLibrary.Boxes[0].Count);
            Assert.Equal(lens1, lensLibrary.Boxes[0].First.Value);
            Assert.Equal(lens2, lensLibrary.Boxes[0].Last.Value);
        }

        [Fact]
        public void TestUpsertingExistingLensUpdatesItsFocalLenght()
        {
            Lens lens1 = BuildLens("rn", focalLenght: 10);
            Lens lens2 = BuildLens("cm");
            Lens lens3 = BuildLens("rn", focalLenght: 20);
            LensLibrary lensLibrary = new();
            lensLibrary.Upsert(lens1);
            lensLibrary.Upsert(lens2);
            lensLibrary.Upsert(lens3);
            Assert.Equal(2, lensLibrary.Boxes[0].Count);
            Assert.Equal(lens3, lensLibrary.Boxes[0].First.Value);
            Assert.Equal(lens2, lensLibrary.Boxes[0].Last.Value);
        }

        [Fact]
        public void TestRemovingNonExistingLensDoesNothing()
        {
            LensLibrary lensLibrary = new();
            lensLibrary.Remove("HASH");
            Assert.Empty(lensLibrary.Boxes[52]);
        }

        [Fact]
        public void TestRemovingExistingLensDeletesItFromItsBox()
        {
            Lens lens1 = BuildLens("rn");
            Lens lens2 = BuildLens("cm");
            LensLibrary lensLibrary = new();
            lensLibrary.Upsert(lens1);
            lensLibrary.Upsert(lens2);
            lensLibrary.Remove("rn");
            Assert.Single(lensLibrary.Boxes[0]);
            Assert.Equal(lens2, lensLibrary.Boxes[0].First.Value);
        }

        [Fact]
        public void TestFocusingPowerOfEachBoxIsSumOfSlotNumberTimesFocalLengthForEachLens()
        {
            LensLibrary lensLibrary = new();
            lensLibrary.Upsert(BuildLens("ot", 7));
            lensLibrary.Upsert(BuildLens("ab", 5));
            lensLibrary.Upsert(BuildLens("pc", 6));
            int expectedFocusingPower = 35; // 1*7 + 2*5 + 3*6
            Assert.Equal(expectedFocusingPower, lensLibrary.BoxFocusingPower(boxNumber: 3));
        }
    }
}