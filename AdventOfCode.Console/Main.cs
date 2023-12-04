using AdventOfCode.Console.Models;
using AdventOfCode.Console.IO;

var inputFolder = Environment.GetEnvironmentVariable("INPUT_FOLDER");

#region Day 1 - Trebuchet

var trebuchetInputPath = $"{inputFolder}/TrebuchetInput.txt";
var lines = File.ReadAllLines(trebuchetInputPath);
var valueWithoutSpelledOutDigits = Trebuchet.AddUpNumericValues(lines, considerSpelledOutDigits: false);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/o spelled out digits: {valueWithoutSpelledOutDigits}");
var valueWithSpelledOutDigits = Trebuchet.AddUpNumericValues(lines, considerSpelledOutDigits: true);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/ spelled out digits: {valueWithSpelledOutDigits}");

#endregion

#region Day 2 - Cube Conundrum

var cubeConundrumInputPath = $"{inputFolder}/CubeConundrumInput.txt";
var bag = new CubeCollection(Red: 12, Green: 13, Blue: 14);

var games = TextParser.ParseCubeGamesFromTextFile(cubeConundrumInputPath);
var sumOfIdsOfAllPossibleGames = CubeConundrum.SumOfIdsOfAllPossibleGames(bag, games);
Console.WriteLine($"Day 2 - Cube Conundrum - Sum of ids of all possible games: {sumOfIdsOfAllPossibleGames}");
var sumOfAllPowers = CubeConundrum.SumOfPowersOfMinBags(games);
Console.WriteLine($"Day 2 - Cube Conundrum - Sum of powers of all minimum bags: {sumOfAllPowers}");

#endregion

#region Day 3 - Gear Ratios

var gearRatiosInputPath = $"{inputFolder}/GearRatiosInput.txt";
lines = File.ReadAllLines(gearRatiosInputPath);
var gearRatios = new GearRatios(lines.ToList());
var sumPartNumbers = gearRatios.PartNumbers().Sum();
Console.WriteLine($"Day 3 - Gear Ratios - Sum of part numbers: {sumPartNumbers}");
var gears = gearRatios.Gears();
var sumOfGearRatios = gears.Sum(gear => gear.Item1 * gear.Item2);
Console.WriteLine($"Day 3 - Gear Ratios - Sum ofproduct of gears: {sumOfGearRatios}");

#endregion

#region Day 4 - Scratchcards

var scratchcardsInputPath = $"{inputFolder}/ScratchcardsInput.txt";
var scratchcardGames = TextParser.ParseScratchcardsFromTextFile(scratchcardsInputPath);
var scratchcards = new Scratchcards(scratchcardGames);
Console.WriteLine($"Day 4 - Scratchcards - Total points: {scratchcards.TotalPoints()}");
Console.WriteLine($"Day 4 - Scratchcards - Total cards: {scratchcards.CardsMultiplicity().Sum()}");


#endregion