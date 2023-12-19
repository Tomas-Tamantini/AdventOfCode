using AdventOfCode.Console.Models;
using System.Text;

namespace AdventOfCode.Console.IO
{
    public interface IFileReader
    {
        string[] ReadAllLines(string path);
        string ReadAllText(string path);
    }

    public class FileReader : IFileReader
    {
        public string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path, Encoding.UTF8);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }
    }

    public class TextParser
    {
        private readonly IFileReader fileReader;

        public TextParser(IFileReader fileReader)
        {
            this.fileReader = fileReader;
        }

        public List<CubeGame> ParseCubeGamesFromTextFile(string fileName)
        {
            var games = new List<CubeGame>();
            var lines = fileReader.ReadAllLines(fileName);
            foreach (var line in lines)
                games.Add(ParseCubeGame(line));
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

        public List<ScratchcardGame> ParseScratchcardsFromTextFile(string fileName)
        {
            var games = new List<ScratchcardGame>();
            var lines = fileReader.ReadAllLines(fileName);
            foreach (var line in lines)
                games.Add(ParseScratchcard(line));
            return games;
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

        public static Fertilizer ParseFertilizer(string fertilizerText)
        {
            string[] lines = fertilizerText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            List<long> seeds = new();
            List<SourceDestinationMapper> mappers = new();
            List<string> lineAccumulator = new();
            foreach (var line in lines)
            {
                if (line.Contains("seeds:")) seeds = ParseFertilizerSeeds(line);
                else if (line.Contains("map:"))
                {
                    if (lineAccumulator.Count > 0) mappers.Add(ParseSourceDestinationMapper(lineAccumulator));
                    lineAccumulator.Clear();
                    lineAccumulator.Add(line);
                }
                else lineAccumulator.Add(line);
            }
            if (lineAccumulator.Count > 0) mappers.Add(ParseSourceDestinationMapper(lineAccumulator));
            return new Fertilizer(seeds, new ChainMapper("seed", "location", mappers));
        }

        public static List<long> ParseFertilizerSeeds(string line)
        {
            var seedsText = line.Split(":")[1];
            var seedsAsStr = SplitBySpace(seedsText);
            return seedsAsStr.Select(long.Parse).ToList();
        }

        public static SourceDestinationMapper ParseSourceDestinationMapper(List<string> lines)
        {
            var sourceName = "";
            var destinationName = "";
            var intervalOffsets = new List<IntervalOffset>();
            foreach (var line in lines)
            {
                if (line.Contains("map"))
                {
                    string[] sourceDestination = SplitBySpace(line)[0].Split("-to-");
                    (sourceName, destinationName) = (sourceDestination[0], sourceDestination[1]);
                }
                else
                {
                    var rangeDef = SplitBySpace(line);
                    if (rangeDef.Length == 3)
                    {
                        var range = rangeDef.Select(r => long.Parse(r)).ToList();
                        Interval interval = new() { Start = range[1], End = range[1] + range[2] - 1 };
                        var offset = range[0] - range[1];
                        IntervalOffset intervalOffset = new(interval, offset);
                        intervalOffsets.Add(intervalOffset);
                    }
                }
            }
            return new SourceDestinationMapper(sourceName, destinationName, intervalOffsets);
        }

        public List<RaceSpecification> ParseBoatRace(string fileName, bool ignoreSpaces)
        {
            List<long> times = new();
            List<long> distances = new();
            foreach (var line in fileReader.ReadAllLines(fileName))
            {
                var lineParts = line.Split(":");
                string numbers = ignoreSpaces ? lineParts[1].Replace(" ", "") : lineParts[1];
                if (lineParts[0].Contains("Time"))
                    times = SplitBySpace(numbers).Select(long.Parse).ToList();
                else if (lineParts[0].Contains("Distance"))
                    distances = SplitBySpace(numbers).Select(long.Parse).ToList();
            }
            return times.Zip(distances, (time, distance) => new RaceSpecification(RaceTime: time, PreviousRecord: distance)).ToList();
        }

        public List<CamelBid> ParseCamelBids(string fileName)
        {
            var lines = fileReader.ReadAllLines(fileName);
            return lines.Select(line => ParseCamelBid(line)).ToList();
        }

        public static CamelBid ParseCamelBid(string line)
        {
            var bidParts = line.Split(" ");
            var hand = bidParts[0];
            var bid = int.Parse(bidParts[1]);
            return new CamelBid(hand, bid);
        }

        public HauntedWasteland ParseHauntedWasteland(string fileName)
        {
            var lines = fileReader.ReadAllLines(fileName);
            string path = "";
            Dictionary<string, (string, string)> network = new();
            foreach (string line in lines)
            {
                if (line.Length == 0) continue;
                if (!line.Contains('=')) path = line;
                else
                {
                    var dictParts = line.Replace("(", "").Replace(")", "").Split("=");
                    var dictKey = dictParts[0].Trim();
                    var dictValues = dictParts[1].Split(",");
                    string leftNode = dictValues[0].Trim();
                    string rightNode = dictValues[1].Trim();
                    network.Add(dictKey, (leftNode, rightNode));
                }
            }
            return new HauntedWasteland() { Network = network, Path = path };
        }

        public CosmicExpansion ParseCosmicExpansion(string fileName)
        {
            var lines = fileReader.ReadAllLines(fileName);
            var universeWidth = lines[0].Trim().Length;
            var universeHeight = lines.Length;
            var galaxies = new List<(int, int)>();
            for (int y = 0; y < universeHeight; y++)
            {
                var line = lines[y].Trim();
                for (int x = 0; x < universeWidth; x++)
                {
                    if (line[x] == '#') galaxies.Add((x, y));
                }
            }
            return new CosmicExpansion(universeWidth, universeHeight, galaxies);
        }

        public static DamagedSprings ParseDamagedSprings(string line, int foldNumber = 1)
        {
            string[] lineParts = line.Split(" ");
            string records = lineParts[0].Trim();
            int[] groups = lineParts[1].Split(',')
                                     .Select(int.Parse)
                                     .ToArray();

            string repeatedRecords = string.Join("?", Enumerable.Repeat(records, foldNumber).ToArray());

            int[] repeatedGroups = Enumerable.Repeat(groups, foldNumber).SelectMany(x => x).ToArray();

            return new DamagedSprings(ConditionRecords: repeatedRecords, ContiguousGroups: repeatedGroups);
        }

        public List<PointOfIncidence> ParsePointsOfIncidence(string filename)
        {
            List<string> patterns = new();
            StringBuilder currentPattern = new();
            string currentStr;
            foreach (string line in fileReader.ReadAllLines(filename))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    currentStr = currentPattern.ToString().Trim();
                    if (currentStr.Length > 0) patterns.Add(currentPattern.ToString());
                    currentPattern = new StringBuilder();
                }
                else
                {
                    currentPattern.Append('\n');
                    currentPattern.Append(line);
                }
            }
            currentStr = currentPattern.ToString().Trim();
            if (currentStr.Length > 0) patterns.Add(currentPattern.ToString());
            return patterns.Select(p => new PointOfIncidence(p.Trim())).ToList();
        }

        public LensLibrary ParseLensLibrary(string filename)
        {
            LensLibrary lensLibrary = new();
            string rawCommands = fileReader.ReadAllText(filename);
            string[] strCommands = rawCommands.Split(',');
            foreach (string strCommand in strCommands)
            {
                RunLensLibraryCommand(lensLibrary, strCommand);
            }
            return lensLibrary;
        }

        private static void RunLensLibraryCommand(LensLibrary lensLibrary, string strCommand)
        {
            if (strCommand.Contains('='))
            {
                string[] commandParts = strCommand.Split('=');
                string label = commandParts[0].Trim();
                int focalLenght = int.Parse(commandParts[1].Trim());
                lensLibrary.Upsert(new Lens { Label = label, FocalLenght = focalLenght });
            }
            else if (strCommand.Contains('-'))
            {
                string[] commandParts = strCommand.Split('-');
                string label = commandParts[0].Trim();
                lensLibrary.Remove(label);
            }
        }

        public static DigCommand ParseDigCommand(string digCommandStr)
        {
            string[] digCommandParts = digCommandStr.Trim().Split(" ");
            CardinalDirection direction = digCommandParts[0] switch
            {
                "U" => CardinalDirection.North,
                "R" => CardinalDirection.East,
                "D" => CardinalDirection.South,
                _ => CardinalDirection.West,
            };
            long numSteps = long.Parse(digCommandParts[1]);
            return new DigCommand(direction, numSteps);
        }

        public static DigCommand ParseDigCommandFromHexCode(string digCommandStr)
        {
            string[] digCommandParts = digCommandStr.Trim().Split(" ");
            string hexCode = digCommandParts[2].Replace("(", "").Replace(")", "");
            long numSteps = long.Parse(hexCode[1..], System.Globalization.NumberStyles.HexNumber);
            CardinalDirection direction = hexCode[^1] switch
            {
                '0' => CardinalDirection.East,
                '1' => CardinalDirection.South,
                '2' => CardinalDirection.West,
                _ => CardinalDirection.North,
            };
            return new DigCommand(direction, numSteps / 16);
        }

        public LavaductLagoon ParseLavaductLagoon(string filename, bool useHexCode = false)
        {
            IEnumerable<DigCommand> digPlan = fileReader.ReadAllLines(filename)
                                                         .Select(line => useHexCode ? ParseDigCommandFromHexCode(line) : ParseDigCommand(line));
            return new LavaductLagoon(digPlan);
        }

        public static MachinePartRating ParseMachinePartRating(string line)
        {
            var parts = line.Replace("{", "").Replace("}", "").Split(',');
            Dictionary<string, int> ratingParts = new();
            foreach (var part in parts)
            {
                var keyValue = part.Split('=');
                ratingParts.Add(keyValue[0], int.Parse(keyValue[1]));
            }
            return new MachinePartRating(ratingParts["x"], ratingParts["m"], ratingParts["a"], ratingParts["s"]);
        }

        private static RatingInequality ParseRatingInequalitiy(string condition)
        {
            var parts = condition.Split('<', '>', ':');
            string attribute = parts[0].ToUpper();
            bool isLessThan = condition.Contains('<');
            int threshold = int.Parse(parts[1]);
            string nextRuleId = parts[2];
            return new RatingInequality(attribute, isLessThan, threshold, nextRuleId);
        }

        public static MachinePartRule ParseMachinePartRule(string line)
        {
            var parts = line.Replace("}", "").Split('{');
            string ruleId = parts[0];
            List<RatingInequality> inequalities = new();
            string defaultNextRuleId = "";
            foreach (var part in parts[1].Split(','))
            {
                if (part.Contains(':'))
                {
                    inequalities.Add(ParseRatingInequalitiy(part));
                }
                else
                {
                    defaultNextRuleId = part;
                }
            }
            return new MachinePartRule(id: ruleId, inequalities: inequalities.ToArray(), defaultNextRule: defaultNextRuleId);
        }

        public (Aplenty, IEnumerable<MachinePartRating>) ParseAplenty(string filename, string initialRule = "in")
        {
            var lines = fileReader.ReadAllLines(filename);
            var rules = new List<MachinePartRule>();
            List<MachinePartRating> ratings = new();
            bool parsingRules = true;
            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    parsingRules = false;
                    continue;
                }
                if (parsingRules)
                {
                    var rule = ParseMachinePartRule(trimmedLine);
                    rules.Add(rule);
                }
                else
                {
                    ratings.Add(ParseMachinePartRating(trimmedLine));
                }
            }
            return (new Aplenty(rules, initialRule), ratings);
        }
    }
}
