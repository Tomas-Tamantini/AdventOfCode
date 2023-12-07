namespace AdventOfCode.Tests
{
    public class TestDay7CamelCards
    {
        [Fact]
        public void TestHandTypesAreProperlyOrdered()
        {
            Assert.True(HandType.HighCard < HandType.OnePair);
            Assert.True(HandType.OnePair < HandType.TwoPair);
            Assert.True(HandType.TwoPair < HandType.ThreeOfAKind);
            Assert.True(HandType.ThreeOfAKind < HandType.FullHouse);
            Assert.True(HandType.FullHouse < HandType.FourOfAKind);
            Assert.True(HandType.FourOfAKind < HandType.FiveOfAKind);
        }

        private readonly IHandRanker defaultRanker = new DefaultCamelRanker();
        private readonly IHandRanker jokerRanker = new JokerCamelRanker();

        [Fact]
        public void TestHandsAreProperlyClassifiedByDefaultRanker()
        {
            Assert.Equal(HandType.HighCard, defaultRanker.RankHand("23456"));
            Assert.Equal(HandType.OnePair, defaultRanker.RankHand("A23A4"));
            Assert.Equal(HandType.TwoPair, defaultRanker.RankHand("23432"));
            Assert.Equal(HandType.ThreeOfAKind, defaultRanker.RankHand("TTT98"));
            Assert.Equal(HandType.FullHouse, defaultRanker.RankHand("23332"));
            Assert.Equal(HandType.FourOfAKind, defaultRanker.RankHand("AA8AA"));
            Assert.Equal(HandType.FiveOfAKind, defaultRanker.RankHand("AAAAA"));
        }

        [Fact]
        public void TestHandsAreProperlyClassifiedByJokerRanker()
        {
            Assert.Equal(HandType.HighCard, jokerRanker.RankHand("23456"));
            Assert.Equal(HandType.OnePair, jokerRanker.RankHand("23J56"));
            Assert.Equal(HandType.TwoPair, jokerRanker.RankHand("KKQQ6"));
            Assert.Equal(HandType.ThreeOfAKind, jokerRanker.RankHand("7J9TJ"));
            Assert.Equal(HandType.FullHouse, jokerRanker.RankHand("7788J"));
            Assert.Equal(HandType.FourOfAKind, jokerRanker.RankHand("QJJQ2"));
            Assert.Equal(HandType.FiveOfAKind, jokerRanker.RankHand("AAAJJ"));
        }

        [Fact]
        public void TestHandOfHigherTypeBeatsHandOfLowerType()
        {
            var bestHand = new CamelHand { HandType = HandType.FourOfAKind, CardValues = new int[] { 1, 2, 3 } };
            var worstHand = new CamelHand { HandType = HandType.ThreeOfAKind, CardValues = new int[] { 1, 2, 3 } };
            Assert.True(worstHand < bestHand);
        }

        [Fact]
        public void TestHandsOfEqualTypesAreComparedCardByCardFromFirstToLast()
        {
            var bestHand = new CamelHand { HandType = HandType.FourOfAKind, CardValues = new int[] { 1, 2, 4 } };
            var worstHand = new CamelHand { HandType = HandType.ThreeOfAKind, CardValues = new int[] { 1, 2, 3 } };
            Assert.True(worstHand < bestHand);
        }

        private static List<CamelBid> GetTestBids()
        {
            return new()
            {
                new CamelBid("32T3K", 765),
                new CamelBid("T55J5", 684),
                new CamelBid("KK677", 28),
                new CamelBid("KTJJT", 220),
                new CamelBid("QQQJA", 483)
            };
        }

        [Fact]
        public void TestBidsCanBeSortedByHandValue()
        {
            CamelCards camelCards = new(GetTestBids(), defaultRanker);
            int[] expectedSortedBids = { 765, 220, 28, 684, 483 };
            Assert.Equal(expectedSortedBids, camelCards.SortedBidValues());

            camelCards = new(GetTestBids(), jokerRanker);
            expectedSortedBids = new[] { 765, 28, 684, 483, 220 };
            Assert.Equal(expectedSortedBids, camelCards.SortedBidValues());
        }

        [Fact]
        public void TestTotalWinningsAreSumOfBidsTimeTheirRanks()
        {
            CamelCards camelCards = new(GetTestBids(), defaultRanker);
            Assert.Equal(6440, camelCards.TotalWinnings());

            camelCards = new(GetTestBids(), jokerRanker);
            Assert.Equal(5905, camelCards.TotalWinnings());
        }
    }
}