using System.Text;

namespace AdventOfCode.Console
{
    public record CubeCollection(int Red, int Green, int Blue);
    public record CubeGame(int Id, List<CubeCollection> Handfuls);
    public class CubeConundrum
    {
        public static int SumOfIdsOfAllPossibleGamesFromTextFile(CubeCollection bag, string filePath)
        {
            var games = new List<CubeGame>();
            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            foreach (var line in lines)
                games.Add(ParseGame(line));
            return SumOfIdsOfAllPossibleGames(bag, games);
        }

        public static bool GameIsPossible(CubeCollection bag, CubeGame game)
        {
            foreach (var handful in game.Handfuls)
                if (handful.Red > bag.Red || handful.Green > bag.Green || handful.Blue > bag.Blue)
                    return false;
            return true;
        }

        public static int SumOfIdsOfAllPossibleGames(CubeCollection bag, List<CubeGame> games)
        {
            var sum = 0;
            foreach (var game in games)
                if (GameIsPossible(bag, game))
                    sum += game.Id;
            return sum;
        }

        public static CubeGame ParseGame(string line)
        {
            var gameParts = line.Split(":");
            var gameId = int.Parse(gameParts[0].Split(" ")[1]);
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
    }
}
