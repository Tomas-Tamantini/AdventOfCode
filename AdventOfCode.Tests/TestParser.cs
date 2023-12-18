using System.Text;
using AdventOfCode.Console.IO;
using Moq;

namespace AdventOfCode.Tests
{
    public class TestParser
    {
        [Fact]
        public void TestCanParseTextIntoCubeGame()
        {
            var game = TextParser.ParseCubeGame("Game  2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue; 2 green");
            Assert.Equal(2, game.Id);
            Assert.Equal(4, game.Handfuls.Count);
            Assert.Equal(new CubeCollection(0, 2, 1), game.Handfuls[0]);
            Assert.Equal(new CubeCollection(1, 3, 4), game.Handfuls[1]);
            Assert.Equal(new CubeCollection(0, 1, 1), game.Handfuls[2]);
            Assert.Equal(new CubeCollection(0, 2, 0), game.Handfuls[3]);
        }

        [Fact]
        public void TestCanParseTextIntoScratchcard()
        {
            var scratchCard = TextParser.ParseScratchcard("Card  4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83");
            Assert.Equal(4, scratchCard.Id);
            Assert.Equal(new HashSet<int> { 41, 92, 73, 84, 69 }, scratchCard.WinningNumbers);
            Assert.Equal(new HashSet<int> { 59, 84, 76, 51, 58, 5, 54, 83 }, scratchCard.MyNumbers);
        }

        [Fact]
        public void TestCanParseTextIntoFertilizerSeeds()
        {
            var seedsText = "seeds: 79 14 55 13";
            Assert.Equal(new List<long>() { 79, 14, 55, 13 }, TextParser.ParseFertilizerSeeds(seedsText));
        }

        [Fact]
        public void TestCanParseTextIntoSourceDestinationMap()
        {
            List<string> mapText = new() { "seed-to-soil map:", "50 98 2", "52 50 48" };
            SourceDestinationMapper parsedMap = TextParser.ParseSourceDestinationMapper(mapText);
            Assert.Equal("seed", parsedMap.SourceName);
            Assert.Equal("soil", parsedMap.DestinationName);
            IntervalSet intervalSet = new(new List<Interval> { new() { Start = 53, End = 53 } });
            Assert.Equal(55, parsedMap.Map(intervalSet).LowestNumber());
        }

        [Fact]
        public void TestCanParseTextIntoFertilizer()
        {
            var fertilizerText = "seeds: 79 14 55 13\n" +
                                "\n" +
                                "seed-to-soil map:\n" +
                                "50 98 2\n" +
                                "52 50 48\n" +
                                "\n" +
                                "soil-to-fertilizer map:\n" +
                                "0 15 37\n" +
                                "37 52 2\n" +
                                "39 0 15\n" +
                                "\n" +
                                "fertilizer-to-water map:\n" +
                                "49 53 8\n" +
                                "0 11 42\n" +
                                "42 0 7\n" +
                                "57 7 4\n" +
                                "\n" +
                                "water-to-light map:\n" +
                                "88 18 7\n" +
                                "18 25 70\n" +
                                "\n" +
                                "light-to-temperature map:\n" +
                                "45 77 23\n" +
                                "81 45 19\n" +
                                "68 64 13\n" +
                                "\n" +
                                "temperature-to-humidity map:\n" +
                                "0 69 1\n" +
                                "1 0 69\n" +
                                "\n" +
                                "humidity-to-location map:\n" +
                                "60 56 37\n" +
                                "56 93 4\n";

            Fertilizer fertilizer = TextParser.ParseFertilizer(fertilizerText);
            Assert.Equal(35, fertilizer.LowestOutputWithStandaloneSeeds());
        }

        [Fact]
        public void TestCanParseCamelBid()
        {
            var bidText = "A23A4 1";
            var bid = TextParser.ParseCamelBid(bidText);
            Assert.Equal("A23A4", bid.Hand);
            Assert.Equal(1, bid.Bid);
        }

        [Fact]
        public void TestCanParseCosmicExpansion()
        {
            var filename = "CosmicExpansionInput.txt";
            var fileContent = @"...#......
                                .......#..
                                #.........
                                ..........
                                ......#...
                                .#........
                                .........#
                                ..........
                                .......#..
                                #...#.....";
            var fileReaderMock = new Mock<IFileReader>();
            fileReaderMock.Setup(fr => fr.ReadAllLines(filename)).Returns(fileContent.Split('\n'));
            var parser = new TextParser(fileReaderMock.Object);
            var cosmicExpansion = parser.ParseCosmicExpansion(filename);
            Assert.Equal(374, cosmicExpansion.SumDistancesBetweenAllPairsOfGalaxies());
        }

        [Fact]
        public void TestCanParseDamagedSpring()
        {
            string springStr = ".#...#....###. 1,1,3";
            DamagedSprings damagedSprings = TextParser.ParseDamagedSprings(springStr);
            Assert.Equal(".#...#....###.", damagedSprings.ConditionRecords);
            Assert.Equal(3, damagedSprings.ContiguousGroups.Length);
        }

        [Fact]
        public void TestCanParseDamagedSpringUnfoldedNTimes()
        {
            string springStr = ".# 1";
            DamagedSprings damagedSprings = TextParser.ParseDamagedSprings(springStr, foldNumber: 3);
            Assert.Equal(".#?.#?.#", damagedSprings.ConditionRecords);
            Assert.Equal(3, damagedSprings.ContiguousGroups.Length);
        }

        [Fact]
        public void TestCanParsePointsOfIncidence()
        {
            var filename = "PointsOfIncidenceInput.txt";
            var fileContent = @"#.##..##.
                                ..#.##.#.
                                ##......#
                                ##......#
                                ..#.##.#.
                                ..##..##.
                                #.#.##.#.

                                #...##..#
                                #....#..#
                                ..##..###
                                #####.##.
                                #####.##.
                                ..##..###
                                #....#..#";
            var fileReaderMock = new Mock<IFileReader>();
            fileReaderMock.Setup(fr => fr.ReadAllLines(filename)).Returns(fileContent.Split('\n'));
            var parser = new TextParser(fileReaderMock.Object);
            List<PointOfIncidence> pointOfIncidences = parser.ParsePointsOfIncidence(filename);
            Assert.Equal(2, pointOfIncidences.Count);
        }

        [Fact]
        public void TestCanParseLensLibrary()
        {
            var filename = "LensLibraryInput.txt";
            var fileContent = "rn=1,cm=2,rn-";
            var fileReaderMock = new Mock<IFileReader>();
            fileReaderMock.Setup(fr => fr.ReadAllText(filename)).Returns(fileContent);
            var parser = new TextParser(fileReaderMock.Object);
            LensLibrary lensLibrary = parser.ParseLensLibrary(filename);
            Assert.Single(lensLibrary.Boxes[0]);
        }

        [Fact]
        public void TestCanParseDigCommand()
        {
            string digCommandStr = "R 7 (#3137a2)";
            DigCommand digCommand = TextParser.ParseDigCommand(digCommandStr);
            Assert.Equal(CardinalDirection.East, digCommand.Direction);
            Assert.Equal(7, digCommand.NumSteps);
        }

        [Fact]
        public void TestCanParseDigCommandFromHexCode()
        {
            string digCommandStr = "R 2 (#caa173)";
            DigCommand digCommand = TextParser.ParseDigCommandFromHexCode(digCommandStr);
            Assert.Equal(CardinalDirection.North, digCommand.Direction);
            Assert.Equal(829975, digCommand.NumSteps);
        }

        [Fact]
        public void TestCanParseLavaductLagoon()
        {
            string fileContent = @"R 6 (#70c710)
                                   D 5 (#0dc571)
                                   L 2 (#5713f0)
                                   D 2 (#d2c081)
                                   R 2 (#59c680)
                                   D 2 (#411b91)
                                   L 5 (#8ceee2)
                                   U 2 (#caa173)
                                   L 1 (#1b58a2)
                                   U 2 (#caa171)
                                   R 2 (#7807d2)
                                   U 3 (#a77fa3)
                                   L 2 (#015232)
                                   U 2 (#7a21e3)";
            var fileReaderMock = new Mock<IFileReader>();
            fileReaderMock.Setup(fr => fr.ReadAllLines("LavaductLagoonInput.txt")).Returns(fileContent.Split('\n'));
            var parser = new TextParser(fileReaderMock.Object);
            LavaductLagoon lavaductLagoonWithoutHex = parser.ParseLavaductLagoon("LavaductLagoonInput.txt");
            Assert.Equal(62, lavaductLagoonWithoutHex.Volume());
            LavaductLagoon lavaductLagoonWithHex = parser.ParseLavaductLagoon("LavaductLagoonInput.txt", useHexCode: true);
            Assert.Equal(952408144115, lavaductLagoonWithHex.Volume());
        }
    }
}
