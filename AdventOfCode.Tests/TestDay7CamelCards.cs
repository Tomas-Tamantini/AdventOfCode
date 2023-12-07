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
    }
}