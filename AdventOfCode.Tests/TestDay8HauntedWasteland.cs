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
            Assert.Equal(2, pathFinder.NumStepsSinglePath(origin: "AAA", destination: "ZZZ"));
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
            Assert.Equal(6, pathFinder.NumStepsSinglePath(origin: "AAA", destination: "ZZZ"));

        }

        [Fact]
        public void TestCanExploreSimultaneousPathsAccordingToLastCharacter()
        {
            string path = "LR";
            Dictionary<string, (string, string)> network = new()
            {
                { "11A", ("11B", "XXX") },
                { "11B", ("XXX", "11Z") },
                { "11Z", ("11B", "XXX") },
                { "22A", ("22B", "XXX") },
                { "22B", ("22C", "22C") },
                { "22C", ("22Z", "22Z") },
                { "22Z", ("22B", "22B") },
                { "XXX", ("XXX", "XXX") },
            };
            HauntedWasteland pathFinder = new() { Network = network, Path = path };
            Assert.Equal(6, pathFinder.NumStepsSimultaneousPaths('A', 'Z'));
        }
    }
}