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
        public void TestHandOfHigherTypeBeatsHandOfLowerType()
        {
            var bestHand = new CamelHand("KKKJJ", defaultRanker);
            var worstHand = new CamelHand("AATT2", defaultRanker);
            Assert.True(worstHand < bestHand);
        }

        [Fact]
        public void TestHandsOfEqualTypesAreComparedCardByCardFromFirstToLast()
        {
            var bestHand = new CamelHand("33332", defaultRanker);
            var worstHand = new CamelHand("2AAAA", defaultRanker);
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
        }

        [Fact]
        public void TestTotalWinningsAreSumOfBidsTimeTheirRanks()
        {
            CamelCards camelCards = new(GetTestBids(), defaultRanker);
            Assert.Equal(6440, camelCards.TotalWinnings());
        }
    }
}