namespace AdventOfCode.Tests
{
    public class TestDay2Cube
    {
        readonly CubeCollection smallBag = new(Red: 1, Green: 2, Blue: 3);

        private static CubeGame BuildGame(List<CubeCollection> handfuls, int id = 0)
        {
            return new CubeGame(id, handfuls);
        }

        [Fact]
        public void TestGameWithNoHandfulsIsPossible()
        {
            var handfuls = new List<CubeCollection> { };
            var game = BuildGame(handfuls);
            Assert.True(CubeConundrum.GameIsPossible(smallBag, game));
        }

        [Fact]
        public void TestHandfulWithAllColorsLEQBagIsPossible()
        {
            var handfuls = new List<CubeCollection> { new(Red: 0, Green: 2, Blue: 2) };
            var game = BuildGame(handfuls);
            Assert.True(CubeConundrum.GameIsPossible(smallBag, game));
        }

        [Fact]
        public void TestHandfulSomeColorGreaterThanBagIsImpossible()
        {
            var handfuls = new List<CubeCollection> { new(Red: 0, Green: 3, Blue: 0) };
            var game = BuildGame(handfuls);
            Assert.False(CubeConundrum.GameIsPossible(smallBag, game));
        }

        [Fact]
        public void TestIfSomeHandfulIsImpossibleGameIsImpossible()
        {
            var handfuls = new List<CubeCollection> { new(Red: 0, Green: 1, Blue: 0), new(Red: 0, Green: 2, Blue: 4) };
            var game = BuildGame(handfuls);
            Assert.False(CubeConundrum.GameIsPossible(smallBag, game));
        }

        [Fact]
        public void TestIfAllHandfulsArePossibleGameIsPossible()
        {
            var handfuls = new List<CubeCollection> { new(Red: 0, Green: 1, Blue: 0), new(Red: 0, Green: 2, Blue: 2) };
            var game = BuildGame(handfuls);
            Assert.True(CubeConundrum.GameIsPossible(smallBag, game));
        }

        [Fact]
        public void TestCanGetSumOfIdsOfAllPossibleGames()
        {
            var bag = new CubeCollection(Red: 12, Green: 13, Blue: 14);
            var games = new List<CubeGame> {
                BuildGame(new List<CubeCollection> { new(4, 0, 3), new(1, 2, 6), new(0, 2, 0) }, 1),
                BuildGame(new List<CubeCollection> { new(0, 2, 1), new(1, 3, 4), new(0, 1, 0) , new(0, 0, 1) }, 2),
                BuildGame(new List<CubeCollection> { new(20, 8, 6), new(4, 0, 5), new(0, 13, 0) , new(1, 5, 0) }, 3),
                BuildGame(new List<CubeCollection> { new(3, 1, 6), new(6, 3, 0), new(14, 3, 15) }, 4),
                BuildGame(new List<CubeCollection> { new(6, 3, 1), new(1, 2, 2) }, 5)
            };
            Assert.Equal(8, CubeConundrum.SumOfIdsOfAllPossibleGames(bag, games));
        }

        [Fact]
        public void TestCanParseTextIntoGame()
        {
            var game = CubeConundrum.ParseGame("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue; 2 green");
            Assert.Equal(2, game.Id);
            Assert.Equal(4, game.Handfuls.Count);
            Assert.Equal(new CubeCollection(0, 2, 1), game.Handfuls[0]);
            Assert.Equal(new CubeCollection(1, 3, 4), game.Handfuls[1]);
            Assert.Equal(new CubeCollection(0, 1, 1), game.Handfuls[2]);
            Assert.Equal(new CubeCollection(0, 2, 0), game.Handfuls[3]);
        }
    }
}
