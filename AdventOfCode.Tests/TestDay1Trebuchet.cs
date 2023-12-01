

namespace AdventOfCode.Tests
{
    public class TestDay1Trebuchet
    {
        [Fact]
        public void TestStringWithNoDigitsAddsToZero()
        {
            var stringWithNoDigits = "abc";
            var numericValue = Trebuchet.ExtractNumericValue(stringWithNoDigits);
            Assert.Equal(0, numericValue);
        }

        [Fact]
        public void TestStringWithSingleDigitCorrespondsToThatDigitTwice()
        {
            var stringWithDigits = "sdfds4asdf";
            var numericValue = Trebuchet.ExtractNumericValue(stringWithDigits);
            Assert.Equal(44, numericValue);
        }

        [Fact]
        public void TestStringWithTwoDigitsCorrespondToThoseTwoDigitsConcatenated()
        {
            var stringWithDigits = "sd1fd3sa";
            var numericValue = Trebuchet.ExtractNumericValue(stringWithDigits);
            Assert.Equal(13, numericValue);
        }

        [Fact]
        public void TestListOfStringsHaveTheirValuesAddedUp()
        {
            var listOfStrings = new List<string> { "", "1", "sd1fd3sa", "0sr78", "4321" };
            var numericValue = Trebuchet.AddUpNumericValues(listOfStrings);
            Assert.Equal(73, numericValue);
        }
    }
}