using AdventOfCode.Console.IO;

namespace AdventOfCode.Tests
{
    public class TestParser
    {
        [Fact]
        public void TestCanParseTextIntoCubeGame()
        {
            var game = TextParser.ParseCubeGame("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue; 2 green");
            Assert.Equal(2, game.Id);
            Assert.Equal(4, game.Handfuls.Count);
            Assert.Equal(new CubeCollection(0, 2, 1), game.Handfuls[0]);
            Assert.Equal(new CubeCollection(1, 3, 4), game.Handfuls[1]);
            Assert.Equal(new CubeCollection(0, 1, 1), game.Handfuls[2]);
            Assert.Equal(new CubeCollection(0, 2, 0), game.Handfuls[3]);
        }
    }
}
