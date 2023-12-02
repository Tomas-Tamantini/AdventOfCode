using System.Text;
using AdventOfCode.Console;

var inputFolder = Environment.GetEnvironmentVariable("INPUT_FOLDER");

#region Day 1 - Trebuchet

var trebuchetInputPath = $"{inputFolder}/TrebuchetInput.txt";
var valueWithoutSpelledOutDigits = Trebuchet.AddUpNumericValuesFromTextFile(trebuchetInputPath, considerSpelledOutDigits: false);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/o spelled out digits: {valueWithoutSpelledOutDigits}");
var valueWithSpelledOutDigits = Trebuchet.AddUpNumericValuesFromTextFile(trebuchetInputPath, considerSpelledOutDigits: true);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/ spelled out digits: {valueWithSpelledOutDigits}");

#endregion

#region Day 2 - Cube Conundrum

var cubeConundrumInputPath = $"{inputFolder}/CubeConundrumInput.txt";
var bag = new CubeCollection(Red: 12, Green: 13, Blue: 14);

var games = new List<CubeGame>();
var lines = File.ReadAllLines(cubeConundrumInputPath, Encoding.UTF8);
foreach (var line in lines)
    games.Add(CubeConundrum.ParseGame(line));

var sumOfIdsOfAllPossibleGames = CubeConundrum.SumOfIdsOfAllPossibleGames(bag, games);
Console.WriteLine($"Day 2 - Cube Conundrum - Sum of ids of all possible games: {sumOfIdsOfAllPossibleGames}");
var sumOfAllPowers = CubeConundrum.SumOfPowersOfMinBags(games);
Console.WriteLine($"Day 2 - Cube Conundrum - Sum of powers of all minimum bags: {sumOfAllPowers}");

#endregion

