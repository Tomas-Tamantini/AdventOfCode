using System.Security.Cryptography.X509Certificates;
using System.Text;
using AdventOfCode.Console.IO;
using Moq;

namespace AdventOfCode.Tests
{
    public class TestParser
    {
        static TextParser MockedParser(string fileContent)
        {
            var fileReaderMock = new Mock<IFileReader>();
            fileReaderMock.Setup(fr => fr.ReadAllLines(It.IsAny<string>())).Returns(fileContent.Split('\n'));
            fileReaderMock.Setup(fr => fr.ReadAllText(It.IsAny<string>())).Returns(fileContent);
            return new TextParser(fileReaderMock.Object);
        }

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
            var parser = MockedParser(fileContent);
            var cosmicExpansion = parser.ParseCosmicExpansion("input.txt");
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
            var parser = MockedParser(fileContent);
            List<PointOfIncidence> pointOfIncidences = parser.ParsePointsOfIncidence("input.txt");
            Assert.Equal(2, pointOfIncidences.Count);
        }

        [Fact]
        public void TestCanParseLensLibrary()
        {
            var fileContent = "rn=1,cm=2,rn-";
            var parser = MockedParser(fileContent);
            LensLibrary lensLibrary = parser.ParseLensLibrary("input.txt");
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
            var parser = MockedParser(fileContent);
            LavaductLagoon lavaductLagoonWithoutHex = parser.ParseLavaductLagoon("LavaductLagoonInput.txt");
            Assert.Equal(62, lavaductLagoonWithoutHex.Volume());
            LavaductLagoon lavaductLagoonWithHex = parser.ParseLavaductLagoon("LavaductLagoonInput.txt", useHexCode: true);
            Assert.Equal(952408144115, lavaductLagoonWithHex.Volume());
        }

        [Fact]
        public void TestCanParseMachinePartRating()
        {
            string ratingStr = "{x=787,m=2655,a=1222,s=2876}";
            MachinePartRating rating = TextParser.ParseMachinePartRating(ratingStr);
            Assert.Equal(787, rating.X);
            Assert.Equal(2655, rating.M);
            Assert.Equal(1222, rating.A);
            Assert.Equal(2876, rating.S);
        }

        [Fact]
        public void TestCanParseMachinePartRule()
        {
            string ruleStr = "ruleX{a<2006:qkq,m>2090:A,rfg}";
            var rule = TextParser.ParseMachinePartRule(ruleStr);
            Assert.Equal("ruleX", rule.Id);
            Assert.Equal("qkq", rule.Invoke(new MachinePartRating(X: 0, M: 2091, A: 2005, S: 0)));
            Assert.Equal("A", rule.Invoke(new MachinePartRating(X: 0, M: 2091, A: 2006, S: 0)));
            Assert.Equal("rfg", rule.Invoke(new MachinePartRating(X: 0, M: 2090, A: 2006, S: 0)));
        }

        [Fact]
        public void TestCanParseRulesAndMachinePartRatingsFromInputFile()
        {
            string fileContent = @"px{a<2006:qkq,m>2090:A,rfg}
                                   pv{a>1716:R,A}
                                   lnx{m>1548:A,A}
                                   rfg{s<537:gd,x>2440:R,A}
                                   qs{s>3448:A,lnx}
                                   qkq{x<1416:A,crn}
                                   crn{x>2662:A,R}
                                   in{s<1351:px,qqz}
                                   qqz{s>2770:qs,m<1801:hdj,R}
                                   gd{a>3333:R,R}
                                   hdj{m>838:A,pv}

                                   {x=787,m=2655,a=1222,s=2876}
                                   {x=1679,m=44,a=2067,s=496}
                                   {x=2036,m=264,a=79,s=2244}
                                   {x=2461,m=1339,a=466,s=291}
                                   {x=2127,m=1623,a=2188,s=1013}";

            var parser = MockedParser(fileContent);
            (Aplenty aplenty, IEnumerable<MachinePartRating> ratings) = parser.ParseAplenty("MachinePartRulesInput.txt", initialRule: "in");
            Assert.Equal(5, ratings.Count());
            Assert.Equal(3, ratings.Count(r => aplenty.MachinePartIsAccepted(r)));
        }

        [Fact]
        public void TestCanParsePulseCircuit()
        {
            string fileContent = @"broadcaster -> a
                                   %a -> inv, con
                                   &inv -> b
                                   %b -> con
                                   &con -> output";
            var parser = MockedParser(fileContent);
            PulseCircuit circuit = parser.ParsePulseCircuit("PulsePropagationInput.txt");
            circuit.SendInitialPulse(PulseIntensity.Low);
            Assert.Equal(4, circuit.NumLowPulses);
            Assert.Equal(4, circuit.NumHighPulses);
        }

        [Fact]
        public void TestCanParseSandBrick()
        {
            SandBrick brick = TextParser.ParseSandBrick("Brick X", "9,9,176~9,5,176");
            IEnumerable<Coordinates> bottomCoordinates = brick.BottomCoordinates();
            Assert.Equal(5, bottomCoordinates.Count());
            Assert.Equal(new Coordinates(9, 5, 176), bottomCoordinates.First());
            Assert.Equal(new Coordinates(9, 9, 176), bottomCoordinates.Last());
            Assert.Equal("Brick X", brick.Id);
        }
    }
}
