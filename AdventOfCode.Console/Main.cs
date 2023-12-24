using AdventOfCode.Console.Models;
using AdventOfCode.Console.IO;

var input = new InputPath();
var parser = new TextParser(new FileReader());


#region Day 24 - Hailstones

string hailstonesFile = input.GetPath(day: 24);
List<Hailstone> hailstones = File.ReadAllLines(hailstonesFile).Select(TextParser.ParseHailstone).ToList();
BoundingBox boundingBox = new(Min: (200000000000000, 200000000000000), Max: (400000000000000, 400000000000000));
Hailstones hailstonesSimulator = new(hailstones, boundingBox);
int numXYIntersections = hailstonesSimulator.NumFutureXYIntersections();
Console.WriteLine($"Day 24 - Hailstones - Number of future XY intersections: {numXYIntersections}");

#endregion