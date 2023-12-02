using System.Text;

namespace AdventOfCode.Console
{
    public class Trebuchet
    {
        static public int ExtractNumericValue(string line, bool considerSpelledOutDigits = false)
        {
            int firstDigit = -1;
            int lastDigit = -1;

            for (int i = 0; i < line.Length; i++)
            {
                int digit = GetDigit(line, i, considerSpelledOutDigits);
                if (digit >= 0)
                {
                    firstDigit = (firstDigit < 0) ? digit : firstDigit;
                    lastDigit = digit;
                }
            }

            return (firstDigit < 0) ? 0 : firstDigit * 10 + lastDigit;
        }

        static public int AddUpNumericValues(IEnumerable<string> lines, bool considerSpelledOutDigits = false)
        {
            return lines.Select(line => ExtractNumericValue(line, considerSpelledOutDigits)).Sum();
        }

        static public int AddUpNumericValuesFromTextFile(string path, bool considerSpelledOutDigits = false)
        {
            var values = File.ReadAllLines(path);
            return AddUpNumericValues(values, considerSpelledOutDigits);
        }

        private static int GetDigit(string line, int currentIdx, bool considerSpelledOutDigits)
        {
            var character = line[currentIdx];
            if (char.IsDigit(character)) return character - '0';
            else if (considerSpelledOutDigits && char.IsLetter(character))
                return SpelledOutDigit(line, currentIdx);
            return -1;
        }

        static private int SpelledOutDigit(string line, int startIdx)
        {
            var spelledOutDigits = new Dictionary<string, int>
            {
                { "zero", 0 },
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 }
            };
            var spelledOutDigit = new StringBuilder();
            spelledOutDigit.Append(line[startIdx]);
            for (int j = startIdx + 1; j < line.Length; j++)
            {
                var nextCharacter = line[j];
                if (!char.IsLetter(nextCharacter) || spelledOutDigit.Length > 5) return -1;
                spelledOutDigit.Append(nextCharacter);
                if (spelledOutDigits.ContainsKey(spelledOutDigit.ToString()))
                    return spelledOutDigits[spelledOutDigit.ToString()];
            }
            return -1;
        }
    }
}
