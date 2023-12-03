namespace AdventOfCode.Console.Models
{
    public class GearRatios
    {
        private readonly List<string> Lines;

        public GearRatios(List<string> lines)
        {
            Lines = lines;
        }

        private IEnumerable<char> SurroundingCharacters(int lineIdx, int leftIdx, int rightIdx)
        {
            for (var surroundingLineIdx = lineIdx - 1; surroundingLineIdx <= lineIdx + 1; surroundingLineIdx += 2)
            {
                if (surroundingLineIdx < 0 || surroundingLineIdx >= Lines.Count)
                    continue;
                for (var i = leftIdx; i <= rightIdx; i++)
                    if (i >= 0 && i < Lines[surroundingLineIdx].Length)
                        yield return Lines[surroundingLineIdx][i];
            }

            if (leftIdx - 1 >= 0)
                yield return Lines[lineIdx][leftIdx - 1];

            if (rightIdx + 1 < Lines[lineIdx].Length)
                yield return Lines[lineIdx][rightIdx + 1];
        }

        public IEnumerable<int> PartNumbers()
        {
            for (var lineIdx = 0; lineIdx < Lines.Count; lineIdx++)
            {
                foreach (var partNumber in PartNumbersInLine(lineIdx))
                    yield return partNumber;
            }
        }

        private IEnumerable<int> PartNumbersInLine(int lineIdx)
        {
            var currentCharPointer = 0;
            while (currentCharPointer < Lines[lineIdx].Length)
            {
                var currentChar = Lines[lineIdx][currentCharPointer];
                if (!char.IsDigit(currentChar))
                {
                    currentCharPointer++;
                    continue;
                }
                var currentNumber = currentChar - '0';
                var nextCharPointer = currentCharPointer + 1;
                while (nextCharPointer < Lines[lineIdx].Length && char.IsDigit(Lines[lineIdx][nextCharPointer]))
                {
                    currentNumber = currentNumber * 10 + (Lines[lineIdx][nextCharPointer] - '0');
                    nextCharPointer++;
                }
                var surroundingChars = SurroundingCharacters(lineIdx, currentCharPointer, nextCharPointer - 1);
                if (surroundingChars.Any(c => c != '.' && !char.IsDigit(c)))
                    yield return currentNumber;
                currentCharPointer = nextCharPointer;
            }
        }
    }
}