namespace AdventOfCode.Console.Models
{
    public record PartNumber(int Value, int LineIdx, int CharIdx)
    {
        public int NumDigits => Value.ToString().Length;
    }

    public class GearRatios
    {
        private readonly List<string> Lines;

        public GearRatios(List<string> lines)
        {
            Lines = lines;
        }

        private PartNumber? NumberAt(int lineIdx, int charIdx)
        {
            if (lineIdx < 0 || lineIdx >= Lines.Count || charIdx < 0 || charIdx >= Lines[lineIdx].Length)
                return null;
            var currentChar = Lines[lineIdx][charIdx];
            if (!char.IsDigit(currentChar))
                return null;
            var startIdx = charIdx;
            while (startIdx > 0 && char.IsDigit(Lines[lineIdx][startIdx - 1]))
                startIdx--;
            var endIdx = charIdx;
            while (endIdx < Lines[lineIdx].Length - 1 && char.IsDigit(Lines[lineIdx][endIdx + 1]))
                endIdx++;
            var numberStr = Lines[lineIdx].Substring(startIdx, endIdx - startIdx + 1);
            return new PartNumber(int.Parse(numberStr), lineIdx, startIdx);
        }

        private IEnumerable<char> SurroundingCharacters(int lineIdx, int numberStartIdx, int numberEndIdx)
        {
            for (var surroundingLineIdx = lineIdx - 1; surroundingLineIdx <= lineIdx + 1; surroundingLineIdx += 2)
            {
                if (surroundingLineIdx < 0 || surroundingLineIdx >= Lines.Count)
                    continue;
                for (var i = numberStartIdx - 1; i <= numberEndIdx + 1; i++)
                    if (i >= 0 && i < Lines[surroundingLineIdx].Length)
                        yield return Lines[surroundingLineIdx][i];
            }

            if (numberStartIdx - 1 >= 0)
                yield return Lines[lineIdx][numberStartIdx - 1];

            if (numberEndIdx + 1 < Lines[lineIdx].Length)
                yield return Lines[lineIdx][numberEndIdx + 1];
        }

        private IEnumerable<int> SurroundingPartNumbers(int lineIdx, int charIdx)
        {
            for (var currentLineIdx = lineIdx - 1; currentLineIdx <= lineIdx + 1; currentLineIdx++)
            {
                var numberMiddle = NumberAt(currentLineIdx, charIdx);
                if (numberMiddle is not null)
                    yield return numberMiddle.Value;
                else
                {
                    var numberLeft = NumberAt(currentLineIdx, charIdx - 1);
                    if (numberLeft is not null)
                        yield return numberLeft.Value;

                    var numberRight = NumberAt(currentLineIdx, charIdx + 1);
                    if (numberRight is not null)
                        yield return numberRight.Value;
                }
            }
        }

        public IEnumerable<int> PartNumbers()
        {
            for (var lineIdx = 0; lineIdx < Lines.Count; lineIdx++)
            {
                foreach (var partNumber in PartNumbersInLine(lineIdx))
                    yield return partNumber;
            }
        }

        public IEnumerable<(int, int)> Gears()
        {
            for (var lineIdx = 0; lineIdx < Lines.Count; lineIdx++)
            {
                for (var charIdx = 0; charIdx < Lines[lineIdx].Length; charIdx++)
                {
                    if (Lines[lineIdx][charIdx] != '*')
                        continue;
                    var surroundingNumbers = SurroundingPartNumbers(lineIdx, charIdx);
                    if (surroundingNumbers.Count() == 2)
                        yield return (surroundingNumbers.First(), surroundingNumbers.Last());
                }
            }
        }

        private IEnumerable<int> PartNumbersInLine(int lineIdx)
        {
            var currentCharPointer = 0;
            while (currentCharPointer < Lines[lineIdx].Length)
            {
                var number = NumberAt(lineIdx, currentCharPointer);
                if (number is null)
                {
                    currentCharPointer++;
                    continue;
                }
                var numberEndIdx = number.CharIdx + number.NumDigits - 1;
                var surroundingChars = SurroundingCharacters(number.LineIdx, number.CharIdx, numberEndIdx);
                if (surroundingChars.Any(c => c != '.' && !char.IsDigit(c)))
                    yield return number.Value;
                currentCharPointer = numberEndIdx + 1;
            }
        }
    }
}