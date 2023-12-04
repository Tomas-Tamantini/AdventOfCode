namespace AdventOfCode.Console.Models
{
    public record ScratchcardGame(int Id, HashSet<int> WinningNumbers, HashSet<int> MyNumbers)
    {
        public int NumMatches => WinningNumbers.Intersect(MyNumbers).Count();
    }
    public class Scratchcards
    {
        private readonly List<ScratchcardGame> games;

        public Scratchcards(List<ScratchcardGame> scratchcardGames)
        {
            games = scratchcardGames;
        }

        public int TotalPoints()
        {
            return games.Sum(NumPoints);
        }

        public static int NumPoints(ScratchcardGame scratchcard)
        {
            var numMatches = scratchcard.NumMatches;
            return numMatches == 0 ? 0 : (int)Math.Pow(2, numMatches - 1);
        }
    }
}