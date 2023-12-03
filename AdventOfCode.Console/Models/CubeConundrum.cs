namespace AdventOfCode.Console.Models
{
    public record CubeCollection(int Red, int Green, int Blue)
    {
        public int Power => Red * Green * Blue;
    };
    public record CubeGame(int Id, List<CubeCollection> Handfuls);
    public class CubeConundrum
    {
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

        public static CubeCollection MinimumBag(CubeGame game)
        {
            if (game.Handfuls.Count == 0)
                return new CubeCollection(Red: 0, Green: 0, Blue: 0);

            var maxRed = game.Handfuls.Max(handful => handful.Red);
            var maxGreen = game.Handfuls.Max(handful => handful.Green);
            var maxBlue = game.Handfuls.Max(handful => handful.Blue);

            return new CubeCollection(maxRed, maxGreen, maxBlue);
        }

        public static int SumOfPowersOfMinBags(List<CubeGame> games)
        {
            var sum = 0;
            foreach (var game in games)
                sum += MinimumBag(game).Power;
            return sum;
        }
    }
}
