namespace AdventOfCode.Tests
{
    public class TestDay4Scratchcards
    {
        private static ScratchcardGame BuildGame(int numMatches, int id = 0)
        {
            var winningNumbers = Enumerable.Range(1, numMatches).ToHashSet();
            var myNumbers = Enumerable.Range(1, numMatches).ToHashSet();
            for (var i = numMatches + 1; i <= numMatches + 10; i += 2)
            {
                winningNumbers.Add(i);
                myNumbers.Add(i + 1);
            }
            return new ScratchcardGame(id, winningNumbers, myNumbers);
        }

        [Fact]
        public void TestNoWinningNumbersResultInNoPoints()
        {
            var scratchcard = BuildGame(numMatches: 0);
            Assert.Equal(0, Scratchcards.NumPoints(scratchcard));
        }

        [Fact]
        public void TestWinningNumbersResultInPreviousPowerOfTwoPoints()
        {
            Assert.Equal(1, Scratchcards.NumPoints(BuildGame(numMatches: 1)));
            Assert.Equal(2, Scratchcards.NumPoints(BuildGame(numMatches: 2)));
            Assert.Equal(4, Scratchcards.NumPoints(BuildGame(numMatches: 3)));
        }

        [Fact]
        public void TestCanCalculateAccumulatedPoints()
        {
            var games = new List<ScratchcardGame>
            {
                new(1,new HashSet<int>{41, 48, 83, 86, 17},new HashSet<int>{83, 86,  6, 31, 17, 9, 48, 53}),
                new(1,new HashSet<int>{13, 32, 20, 16, 61}, new HashSet<int>{61, 30, 68, 82, 17, 32, 24, 19}),
                new(1,new HashSet<int>{1, 21, 53, 59, 44}, new HashSet<int>{69, 82, 63, 72, 16, 21, 14, 1}),
                new(1,new HashSet<int>{41, 92, 73, 84, 69}, new HashSet<int>{59, 84, 76, 51, 58, 5, 54, 83}),
                new(1,new HashSet<int>{87, 83, 26, 28, 32}, new HashSet<int>{88, 30, 70, 12, 93, 22, 82, 36}),
                new(1,new HashSet<int>{31, 18, 13, 56, 72}, new HashSet<int>{74, 77, 10, 23, 35, 67, 36, 11})
            };
            var scratchcards = new Scratchcards(games);
            Assert.Equal(13, scratchcards.TotalPoints());
        }
    }
}