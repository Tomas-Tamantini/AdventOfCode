namespace AdventOfCode.Console
{
    public class Trebuchet
    {
        static public int ExtractNumericValue(string value)
        {
            var digits = value.Where(char.IsDigit).Select(digit => (int)char.GetNumericValue(digit));
            int firstDigit = digits.FirstOrDefault();
            int lastDigit = digits.LastOrDefault();
            return firstDigit * 10 + lastDigit;
        }

        static public int AddUpNumericValues(IEnumerable<string> values)
        {
            return values.Select(ExtractNumericValue).Sum();
        }

        static public int AddUpNumericValuesFromTextFile(string path)
        {
            var values = File.ReadAllLines(path);
            return AddUpNumericValues(values);
        }
    }
}
