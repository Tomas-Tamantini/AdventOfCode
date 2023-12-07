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

        [Fact]
        public void TestHighCardhandsAreProperlyClassified()
        {
            var hand = new CamelHand("23456");
            Assert.Equal(HandType.HighCard, hand.HandType);
        }

        [Fact]
        public void TestOnePairhandsAreProperlyClassified()
        {
            var hand = new CamelHand("A23A4");
            Assert.Equal(HandType.OnePair, hand.HandType);
        }

        [Fact]
        public void TestTwoPairhandsAreProperlyClassified()
        {
            var hand = new CamelHand("23432");
            Assert.Equal(HandType.TwoPair, hand.HandType);
        }

        [Fact]
        public void TestThreeOfAKindhandsAreProperlyClassified()
        {
            var hand = new CamelHand("TTT98");
            Assert.Equal(HandType.ThreeOfAKind, hand.HandType);
        }

        [Fact]
        public void TestFullHousehandsAreProperlyClassified()
        {
            var hand = new CamelHand("23332");
            Assert.Equal(HandType.FullHouse, hand.HandType);
        }

        [Fact]
        public void TestFourOfAKindhandsAreProperlyClassified()
        {
            var hand = new CamelHand("AA8AA");
            Assert.Equal(HandType.FourOfAKind, hand.HandType);
        }

        [Fact]
        public void TestFiveOfAKindhandsAreProperlyClassified()
        {
            var hand = new CamelHand("AAAAA");
            Assert.Equal(HandType.FiveOfAKind, hand.HandType);
        }

        [Fact]
        public void TestHandOfHigherTypeBeatsHandOfLowerType()
        {
            var bestHand = new CamelHand("KKKJJ");
            var worstHand = new CamelHand("AATT2");
            Assert.True(worstHand < bestHand);
        }

        [Fact]
        public void TestHandsOfEqualTypesAreComparedCardByCardFromFirstToLast()
        {
            var bestHand = new CamelHand("33332");
            var worstHand = new CamelHand("2AAAA");
            Assert.True(worstHand < bestHand);
        }

        private readonly List<CamelBid> testBids = new()
        {
            new CamelBid(new CamelHand("32T3K"), 765),
            new CamelBid(new CamelHand("T55J5"), 684),
            new CamelBid(new CamelHand("KK677"), 28),
            new CamelBid(new CamelHand("KTJJT"), 220),
            new CamelBid(new CamelHand("QQQJA"), 483),
        };

        [Fact]
        public void TestBidsCanBeSortedByHandValue()
        {
            CamelCards camelCards = new(testBids);
            int[] expectedSortedBids = { 765, 220, 28, 684, 483 };
            Assert.Equal(expectedSortedBids, camelCards.SortedBidValues());
        }

        [Fact]
        public void TestTotalWinningsAreSumOfBidsTimeTheirRanks()
        {
            CamelCards camelCards = new(testBids);
            Assert.Equal(6440, camelCards.TotalWinnings());
        }
    }
}