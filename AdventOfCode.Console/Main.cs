using AdventOfCode.Console.Models;
using AdventOfCode.Console.IO;

var input = new InputPath();

#region Day 1 - Trebuchet

var lines = File.ReadAllLines(input.GetPath(day: 1));
var valueWithoutSpelledOutDigits = Trebuchet.AddUpNumericValues(lines, considerSpelledOutDigits: false);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/o spelled out digits: {valueWithoutSpelledOutDigits}");
var valueWithSpelledOutDigits = Trebuchet.AddUpNumericValues(lines, considerSpelledOutDigits: true);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/ spelled out digits: {valueWithSpelledOutDigits}");

#endregion

#region Day 2 - Cube Conundrum

var bag = new CubeCollection(Red: 12, Green: 13, Blue: 14);
var games = TextParser.ParseCubeGamesFromTextFile(input.GetPath(day: 2));
var sumOfIdsOfAllPossibleGames = CubeConundrum.SumOfIdsOfAllPossibleGames(bag, games);
Console.WriteLine($"Day 2 - Cube Conundrum - Sum of ids of all possible games: {sumOfIdsOfAllPossibleGames}");
var sumOfAllPowers = CubeConundrum.SumOfPowersOfMinBags(games);
Console.WriteLine($"Day 2 - Cube Conundrum - Sum of powers of all minimum bags: {sumOfAllPowers}");

#endregion

#region Day 3 - Gear Ratios

lines = File.ReadAllLines(input.GetPath(day: 3));
var gearRatios = new GearRatios(lines.ToList());
var sumPartNumbers = gearRatios.PartNumbers().Sum();
Console.WriteLine($"Day 3 - Gear Ratios - Sum of part numbers: {sumPartNumbers}");
var gears = gearRatios.Gears();
var sumOfGearRatios = gears.Sum(gear => gear.Item1 * gear.Item2);
Console.WriteLine($"Day 3 - Gear Ratios - Sum ofproduct of gears: {sumOfGearRatios}");

#endregion

#region Day 4 - Scratchcards

var scratchcardGames = TextParser.ParseScratchcardsFromTextFile(input.GetPath(day: 4));
var scratchcards = new Scratchcards(scratchcardGames);
Console.WriteLine($"Day 4 - Scratchcards - Total points: {scratchcards.TotalPoints()}");
Console.WriteLine($"Day 4 - Scratchcards - Total cards: {scratchcards.CardsMultiplicity().Sum()}");

#endregion

#region Day 5 - Fertilizer

var fileText = File.ReadAllText(input.GetPath(day: 5));
var fertilizer = TextParser.ParseFertilizer(fileText);
Console.WriteLine($"Day 5 - Fertilizer - Lowest location number with individual seeds: {fertilizer.LowestOutputWithStandaloneSeeds()}");
Console.WriteLine($"Day 5 - Fertilizer - Lowest location number with seeds as ranges: {fertilizer.LowestOutputWithSeedsAsRanges()}");

#endregion

#region Day 6 - Boat race

var raceSpecsWithSpaces = TextParser.ParseBoatRace(input.GetPath(day: 6), ignoreSpaces: false);
long product = raceSpecsWithSpaces.Aggregate(1L, (currentProduct, raceSpec) => currentProduct * BoatRace.NumWaysToBreakRecord(raceSpec));
Console.WriteLine($"Day 6 - Boat Race - Product of number of ways to beat records with spaces: {product}");

var raceSpecsWithoutSpaces = TextParser.ParseBoatRace(input.GetPath(day: 6), ignoreSpaces: true);
product = raceSpecsWithoutSpaces.Aggregate(1L, (currentProduct, raceSpec) => currentProduct * BoatRace.NumWaysToBreakRecord(raceSpec));
Console.WriteLine($"Day 6 - Boat Race - Product of number of ways to beat records ignoring spaces: {product}");

#endregion

#region Day 7 - Camel Cards

var camelBids = TextParser.ParseCamelBids(input.GetPath(day: 7));
var defaultGame = new CamelCards(camelBids, new DefaultCamelRanker());
Console.WriteLine($"Day 7 - Camel Cards - Total winnings: {defaultGame.TotalWinnings()}");

var jokerGame = new CamelCards(camelBids, new JokerCamelRanker());
Console.WriteLine($"Day 7 - Camel Cards - Total winnings with jokers: {jokerGame.TotalWinnings()}");

#endregion

#region Day 8 - Haunted Wasteland

var wasteland = TextParser.ParseHauntedWasteland(input.GetPath(day: 8));
int numStepsSinglePath = wasteland.NumStepsSinglePath("AAA", "ZZZ");
Console.WriteLine($"Day 8 - Haunted Wasteland - Number of steps with single path: {numStepsSinglePath}");
long numStepsSimultaneousPaths = wasteland.NumStepsSimultaneousPaths('A', 'Z');
Console.WriteLine($"Day 8 - Haunted Wasteland - Number of steps with simultaneous paths: {numStepsSimultaneousPaths}");

#endregion

#region Day 9 - Mirage Maintenance

var sequencesFile = input.GetPath(day: 9);
List<long> nextTerms = new();
foreach (string line in File.ReadAllLines(sequencesFile))
{
    List<long> sequence = line.Split(" ").Select(long.Parse).ToList();
    long nextTerm = MirageMaintenance.NextTerm(sequence);
    nextTerms.Add(nextTerm);
}
Console.WriteLine($"Day 9 - Mirage Maintenance - Sum of all next terms: {nextTerms.Sum()}");

#endregion