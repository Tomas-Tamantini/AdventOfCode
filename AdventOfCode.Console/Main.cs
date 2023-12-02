using AdventOfCode.Console;

var trebuchetInputPath = "TrebuchetInput.txt";
var valueWithoutSpelledOutDigits = Trebuchet.AddUpNumericValuesFromTextFile(trebuchetInputPath, considerSpelledOutDigits: false);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/o spelled out digits: {valueWithoutSpelledOutDigits}");
var valueWithSpelledOutDigits = Trebuchet.AddUpNumericValuesFromTextFile(trebuchetInputPath, considerSpelledOutDigits: true);
Console.WriteLine($"Day 1 - Trebuchet - Sum of all numeric values w/ spelled out digits: {valueWithSpelledOutDigits}");


