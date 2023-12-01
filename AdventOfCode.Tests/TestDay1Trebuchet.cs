

namespace AdventOfCode.Tests
{
    public class TestDay1Trebuchet
    {
        [Fact]
        public void TestStringWithNoDigitsAddsToZero()
        {
            var stringWithNoDigits = "abc";
            var numericValue = Trebuchet.extractNumericValue(stringWithNoDigits);
            Assert.Equal(0, numericValue);
        }
    }
}