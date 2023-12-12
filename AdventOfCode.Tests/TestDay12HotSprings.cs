namespace AdventOfCode.Tests
{
    public class TestDay12HotSprings
    {
        [Fact]
        public void TestImpossibleArrangementRetunsZeroArrangements()
        {
            string conditionRecords = ".";
            int[] contiguousGroups = new int[] { 1, };
            DamagedSprings damagedSprings = new(conditionRecords, contiguousGroups);
            HotSprings hotSprings = new(damagedSprings);
            Assert.Equal(0, hotSprings.NumArrangements());
        }

        [Fact]
        public void TestSinglePossibleArrangementReturnsOne()
        {
            string conditionRecords = "###";
            int[] contiguousGroups = new int[] { 3, };
            DamagedSprings damagedSprings = new(conditionRecords, contiguousGroups);
            HotSprings hotSprings = new(damagedSprings);
            Assert.Equal(1, hotSprings.NumArrangements());
        }

        [Fact]
        public void TestContiguousGroupIsPlacedInEveryPossiblePosition()
        {
            string conditionRecords = "???#??";
            int[] contiguousGroups = new int[] { 3, };
            DamagedSprings damagedSprings = new(conditionRecords, contiguousGroups);
            HotSprings hotSprings = new(damagedSprings);
            Assert.Equal(3, hotSprings.NumArrangements());
        }

        [Fact]
        public void TestContiguousGroupsIncreaseArrangementsInCombinatoricFaship()
        {
            string conditionRecords = "??.??";
            int[] contiguousGroups = new int[] { 1, 1 };
            DamagedSprings damagedSprings = new(conditionRecords, contiguousGroups);
            HotSprings hotSprings = new(damagedSprings);
            Assert.Equal(4, hotSprings.NumArrangements());
        }

        [Theory]
        [InlineData("???.###", new int[] { 1, 1, 3 }, 1)]
        [InlineData(".??..??...?##.", new int[] { 1, 1, 3 }, 4)]
        [InlineData("?#?#?#?#?#?#?#?", new int[] { 1, 3, 1, 6 }, 1)]
        [InlineData("????.#...#...", new int[] { 4, 1, 1 }, 1)]
        [InlineData("????.######..#####.", new int[] { 1, 6, 5 }, 4)]
        [InlineData("?###????????", new int[] { 3, 2, 1 }, 10)]
        public void TestGivenCases(string conditionRecords, int[] contiguousGroups, long expectedNumArrangements)
        {
            DamagedSprings damagedSprings = new(conditionRecords, contiguousGroups);
            HotSprings hotSprings = new(damagedSprings);
            Assert.Equal(expectedNumArrangements, hotSprings.NumArrangements());
        }

        [Fact]
        public void TestNumArrangementsRunsEfficently()
        {
            int numGroups = 20;
            string conditionRecords = string.Concat(Enumerable.Repeat("??.", numGroups));
            int[] contiguousGroups = Enumerable.Repeat(1, numGroups).ToArray();
            DamagedSprings damagedSprings = new(conditionRecords, contiguousGroups);
            HotSprings hotSprings = new(damagedSprings);
            Assert.Equal((int)Math.Pow(2, numGroups), hotSprings.NumArrangements());
        }
    }
}