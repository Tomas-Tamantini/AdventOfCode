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
    }
}