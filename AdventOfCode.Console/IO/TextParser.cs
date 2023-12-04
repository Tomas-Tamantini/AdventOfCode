using AdventOfCode.Console.Models;
using System.Text;

namespace AdventOfCode.Console.IO
{
    public class TextParser
    {
        public static List<CubeGame> ParseCubeGamesFromTextFile(string fileName)
        {
            var games = new List<CubeGame>();
            var lines = File.ReadAllLines(fileName, Encoding.UTF8);
            foreach (var line in lines)
                games.Add(TextParser.ParseCubeGame(line));
            return games;
        }

        public static CubeGame ParseCubeGame(string line)
        {
            var gameParts = line.Split(":");
            var gameId = int.Parse(SplitBySpace(gameParts[0])[1]);
            var handfuls = gameParts[1].Split(";")
                                      .Select(handfulStr => ParseHandful(handfulStr))
                                      .ToList();
            return new CubeGame(gameId, handfuls);
        }

        private static CubeCollection ParseHandful(string handfulStr)
        {
            var handfulParts = handfulStr.Trim().Split(",");
            var colorCounts = handfulParts
                .Select(part => part.Trim().Split(" "))
                .ToDictionary(
                    colorAndCount => colorAndCount[1],
                    colorAndCount => int.Parse(colorAndCount[0]));

            var red = colorCounts.GetValueOrDefault("red", 0);
            var green = colorCounts.GetValueOrDefault("green", 0);
            var blue = colorCounts.GetValueOrDefault("blue", 0);

            return new CubeCollection(red, green, blue);
        }

        public static ScratchcardGame ParseScratchcard(string scratchcardStr)
        {
            var gameParts = scratchcardStr.Split(":");
            var gameId = int.Parse(SplitBySpace(gameParts[0])[1]);
            var numbers = gameParts[1].Split("|");
            var winningNumbers = ParseHashset(numbers[0]);
            var myNumbers = ParseHashset(numbers[1]);
            return new ScratchcardGame(gameId, winningNumbers, myNumbers);
        }

        private static HashSet<int> ParseHashset(string numbersSeparatedBySpace)
        {
            return new HashSet<int>(SplitBySpace(numbersSeparatedBySpace).Select(int.Parse));
        }

        private static string[] SplitBySpace(string text)
        {
            return text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
