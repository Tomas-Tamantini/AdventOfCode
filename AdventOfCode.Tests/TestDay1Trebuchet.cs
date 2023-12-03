namespace AdventOfCode.Tests
{
    public class TestDay1Trebuchet
    {
        [Fact]
        public void TestStringWithNoDigitsAddsToZeroWhenNotConsideringSpelledOutDigits()
        {
            var stringWithNoDigits = "abc";
            var numericValue = Trebuchet.ExtractNumericValue(stringWithNoDigits);
            Assert.Equal(0, numericValue);
        }

        [Fact]
        public void TestStringWithSingleDigitCorrespondsToThatDigitTwiceWhenNotConsideringSpelledOutDigits()
        {
            var stringWithDigits = "sdfds4asdf";
            var numericValue = Trebuchet.ExtractNumericValue(stringWithDigits);
            Assert.Equal(44, numericValue);
        }

        [Fact]
        public void TestStringWithTwoDigitsCorrespondToThoseTwoDigitsConcatenatedWhenNotConsideringSpelledOutDigits()
        {
            var stringWithDigits = "sd1fd3sa";
            var numericValue = Trebuchet.ExtractNumericValue(stringWithDigits);
            Assert.Equal(13, numericValue);
        }

        [Fact]
        public void TestDigitsCanBeSpelledOut()
        {
            var spelledOutDigits = "asdfouras7dfiveight";
            var numericValue = Trebuchet.ExtractNumericValue(spelledOutDigits, considerSpelledOutDigits: true);
            Assert.Equal(48, numericValue);
        }

        [Fact]
        public void TestListOfStringsHaveTheirValuesAddedUp()
        {
            var listOfStrings = new List<string> { "", "1", "sd1fd3sa", "0sr78", "4321", "twfiveight" };
            var numericValueWithoutSpelledOutDigits = Trebuchet.AddUpNumericValues(listOfStrings);
            Assert.Equal(73, numericValueWithoutSpelledOutDigits);
            var numericValueWithSpelledOutDigits = Trebuchet.AddUpNumericValues(listOfStrings, considerSpelledOutDigits: true);
            Assert.Equal(131, numericValueWithSpelledOutDigits);
        }
    }
}