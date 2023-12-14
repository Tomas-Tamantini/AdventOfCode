using AdventOfCode.Console.Models;
using AdventOfCode.Console.IO;

var input = new InputPath();
var parser = new TextParser(new FileReader());

#region Day 1 - Trebuchet

var lines = File.ReadAllLines(input.GetPath(day: 1));
var valueWithoutSpelledOutDigits = Trebuchet.AddUpNumericValues(lines, considerSpelledOutDigits: false);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/o spelled out digits: {valueWithoutSpelledOutDigits}");
var valueWithSpelledOutDigits = Trebuchet.AddUpNumericValues(lines, considerSpelledOutDigits: true);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/ spelled out digits: {valueWithSpelledOutDigits}");

#endregion

#region Day 2 - Cube Conundrum

var bag = new CubeCollection(Red: 12, Green: 13, Blue: 14);
var games = parser.ParseCubeGamesFromTextFile(input.GetPath(day: 2));
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

var scratchcardGames = parser.ParseScratchcardsFromTextFile(input.GetPath(day: 4));
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

var raceSpecsWithSpaces = parser.ParseBoatRace(input.GetPath(day: 6), ignoreSpaces: false);
long product = raceSpecsWithSpaces.Aggregate(1L, (currentProduct, raceSpec) => currentProduct * BoatRace.NumWaysToBreakRecord(raceSpec));
Console.WriteLine($"Day 6 - Boat Race - Product of number of ways to beat records with spaces: {product}");

var raceSpecsWithoutSpaces = parser.ParseBoatRace(input.GetPath(day: 6), ignoreSpaces: true);
product = raceSpecsWithoutSpaces.Aggregate(1L, (currentProduct, raceSpec) => currentProduct * BoatRace.NumWaysToBreakRecord(raceSpec));
Console.WriteLine($"Day 6 - Boat Race - Product of number of ways to beat records ignoring spaces: {product}");

#endregion

#region Day 7 - Camel Cards

var camelBids = parser.ParseCamelBids(input.GetPath(day: 7));
var defaultGame = new CamelCards(camelBids, new DefaultCamelRanker());
Console.WriteLine($"Day 7 - Camel Cards - Total winnings: {defaultGame.TotalWinnings()}");

var jokerGame = new CamelCards(camelBids, new JokerCamelRanker());
Console.WriteLine($"Day 7 - Camel Cards - Total winnings with jokers: {jokerGame.TotalWinnings()}");

#endregion

#region Day 8 - Haunted Wasteland

var wasteland = parser.ParseHauntedWasteland(input.GetPath(day: 8));
int numStepsSinglePath = wasteland.NumStepsSinglePath("AAA", "ZZZ");
Console.WriteLine($"Day 8 - Haunted Wasteland - Number of steps with single path: {numStepsSinglePath}");
long numStepsSimultaneousPaths = wasteland.NumStepsSimultaneousPaths('A', 'Z');
Console.WriteLine($"Day 8 - Haunted Wasteland - Number of steps with simultaneous paths: {numStepsSimultaneousPaths}");

#endregion

#region Day 9 - Mirage Maintenance

var sequencesFile = input.GetPath(day: 9);
List<long> nextTerms = new();
List<long> previousTerms = new();
foreach (string line in File.ReadAllLines(sequencesFile))
{
    List<long> sequence = line.Split(" ").Select(long.Parse).ToList();
    long nextTerm = MirageMaintenance.NextTerm(sequence);
    long previousTerm = MirageMaintenance.PreviousTerm(sequence);
    nextTerms.Add(nextTerm);
    previousTerms.Add(previousTerm);
}
Console.WriteLine($"Day 9 - Mirage Maintenance - Sum of all next terms: {nextTerms.Sum()}");
Console.WriteLine($"Day 9 - Mirage Maintenance - Sum of all previous terms: {previousTerms.Sum()}");

#endregion

#region Day 10 - Pipe Maze

string pipeMazeFile = input.GetPath(day: 10);
string pipeMazeText = File.ReadAllText(pipeMazeFile);
PipeMaze pipeMaze = new(pipeMazeText);
Console.WriteLine($"Day 10 - Pipe Maze - Furthest point in loop: {pipeMaze.LoopLength() / 2}");
Console.WriteLine($"Day 10 - Pipe Maze - Number of points inside loop: {pipeMaze.NumPointsInsideLoop()}");

#endregion

#region Day 11 - Cosmic Expansion

string cosmicExpansionFile = input.GetPath(day: 11);
CosmicExpansion cosmicExpansion = parser.ParseCosmicExpansion(cosmicExpansionFile);
long distancesExpansionRate2 = cosmicExpansion.SumDistancesBetweenAllPairsOfGalaxies(expansionRate: 2);
Console.WriteLine($"Day 11 - Cosmic Expansion - Total distance between all pairs of galaxies w/ expansion rate 2: {distancesExpansionRate2}");
long distancesExpansionRate1M = cosmicExpansion.SumDistancesBetweenAllPairsOfGalaxies(expansionRate: 1000000);
Console.WriteLine($"Day 11 - Cosmic Expansion - Total distance between all pairs of galaxies w/ expansion rate 1,000,000: {distancesExpansionRate1M}");

#endregion

#region Day 12 - Hot Springs

string hotSpringsFile = input.GetPath(day: 12);
long sumArrangementsOneFold = 0;
foreach (string line in File.ReadAllLines(hotSpringsFile))
{
    DamagedSprings damagedSprings = TextParser.ParseDamagedSprings(line);
    HotSprings hotSprings = new(damagedSprings);
    sumArrangementsOneFold += hotSprings.NumArrangements();
}

long sumArrangementsFiveFold = 0;
foreach (string line in File.ReadAllLines(hotSpringsFile))
{
    DamagedSprings damagedSprings = TextParser.ParseDamagedSprings(line, foldNumber: 5);
    HotSprings hotSprings = new(damagedSprings);
    sumArrangementsFiveFold += hotSprings.NumArrangements();
}
Console.WriteLine($"Day 12 - Hot Springs - Sum of all arrangements with one fold: {sumArrangementsOneFold}");
Console.WriteLine($"Day 12 - Hot Springs - Sum of all arrangements with five folds: {sumArrangementsFiveFold}");

#endregion

#region Day 13 - Point of Incidence

string poiFile = input.GetPath(day: 13);
List<PointOfIncidence> pointOfIncidences = parser.ParsePointsOfIncidence(poiFile);
int sumColsNoMismatch = 0;
int sumRowsNoMismatch = 0;
int sumColsOneMismatch = 0;
int sumRowsOneMismatch = 0;
foreach (var poi in pointOfIncidences)
{
    sumColsNoMismatch += poi.ColumnMirrorIdx() + 1;
    sumRowsNoMismatch += poi.RowMirrorIdx() + 1;
    sumColsOneMismatch += poi.ColumnMirrorIdx(numMismatches: 1) + 1;
    sumRowsOneMismatch += poi.RowMirrorIdx(numMismatches: 1) + 1;
}

int sumNoMismatch = sumRowsNoMismatch * 100 + sumColsNoMismatch;
int sumOneMismatch = sumRowsOneMismatch * 100 + sumColsOneMismatch;
Console.WriteLine($"Day 13 - Point of Incidence - Summary of reflection lines with no mismatches: {sumNoMismatch}");
Console.WriteLine($"Day 13 - Point of Incidence - Summary of reflection lines with one mismatch: {sumOneMismatch}");

#endregion

#region Day 14 - Parabolic Reflector Dish

string dishInput = File.ReadAllText(input.GetPath(day: 14));
ParabolicReflectorDish dish = new(dishInput);
dish.Roll(CardinalDirection.North);
Console.WriteLine($"Day 14 - Parabolic Reflector Dish - Torque on south hinge: {dish.TorqueOnSouthHinge()}");
dish = new(dishInput);
List<CardinalDirection> directions = new() { CardinalDirection.North, CardinalDirection.West, CardinalDirection.South, CardinalDirection.East, };
dish.RunCycles(directions, numCycles: 1000000000);
Console.WriteLine($"Day 14 - Parabolic Reflector Dish - Torque on south hinge after 1 billion cycles: {dish.TorqueOnSouthHinge()}");

#endregion