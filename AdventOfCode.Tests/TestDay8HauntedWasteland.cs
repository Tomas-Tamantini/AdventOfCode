namespace AdventOfCode.Tests
{
    public class TestDay8HauntedWasteland
    {
        [Fact]
        public void TestCanFindOutHowManyStepsItTakesToGoFromSourceToDestinationFollowingPath()
        {
            string path = "RL";
            Dictionary<string, (string, string)> network = new()
            {
                { "AAA", ("BBB", "CCC") },
                { "BBB", ("DDD", "EEE") },
                { "CCC", ("ZZZ", "GGG") },
                { "DDD", ("DDD", "DDD") },
                { "EEE", ("EEE", "EEE") },
                { "GGG", ("GGG", "GGG") },
                { "ZZZ", ("ZZZ", "ZZZ") },
            };
            HauntedWasteland pathFinder = new() { Network = network, Path = path };
            Assert.Equal(2, pathFinder.NumSteps(origin: "AAA", destination: "ZZZ"));
        }

        [Fact]
        public void TestPathLoopsAroundUntilDestinationIsReached()
        {
            string path = "LLR";
            Dictionary<string, (string, string)> network = new()
            {
                { "AAA", ("BBB", "BBB") },
                { "BBB", ("AAA", "ZZZ") },
                { "ZZZ", ("ZZZ", "ZZZ") },
            };
            HauntedWasteland pathFinder = new() { Network = network, Path = path };
            Assert.Equal(6, pathFinder.NumSteps(origin: "AAA", destination: "ZZZ"));

        }
    }
}