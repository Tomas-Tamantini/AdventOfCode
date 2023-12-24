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


#region Day 15 - Lens Library

string lensLibraryFile = input.GetPath(day: 15);
string lensLibraryText = File.ReadAllText(lensLibraryFile);
int totalHash = lensLibraryText.Split(",").Select(element => element.Trim()).Sum(element => LensLibrary.GetHash(element));
Console.WriteLine($"Day 15 - Lens Library - Total hash: {totalHash}");
LensLibrary lensLibrary = parser.ParseLensLibrary(lensLibraryFile);
int totalFocusingPower = lensLibrary.Boxes.Select((box, i) => (i + 1) * lensLibrary.BoxFocusingPower(i)).Sum();
Console.WriteLine($"Day 15 - Lens Library - Total focusing power: {totalFocusingPower}");

#endregion

#region Day 16 - Lava Floor

string lavaFloorFile = input.GetPath(day: 16);
string lavaFloorText = File.ReadAllText(lavaFloorFile);
LavaContraption lavaContraption = new(lavaFloorText);
LavaFloor lavaFloor = new(lavaContraption);
PhotonState initialBeam = new(0, 0, CardinalDirection.East);
lavaFloor.RunBeam(initialBeam);
Console.WriteLine($"Day 16 - Lava Floor - Number of energized tiles: {lavaFloor.NumEnergizedTiles()}");
Console.WriteLine($"Day 16 - Lava Floor - Maximum number of energized tiles: {lavaFloor.MaxNumEnergizedTiles()}");

#endregion

#region Day 17 - Clumsy Crucible

string clumsyCrucibleFile = input.GetPath(day: 17);
string clumsyCrucibleText = File.ReadAllText(clumsyCrucibleFile);

CrucibleInertia inertiaSmallCrucible = new(MinStepsSameDirection: null, MaxStepsSameDirection: 3);
ClumsyCrucible smallCrucible = new(clumsyCrucibleText, inertiaSmallCrucible);
Console.WriteLine($"Day 17 - Clumsy Crucible - Minimum heat loss for small crucible: {smallCrucible.MinimumHeatLoss()}");

CrucibleInertia inertiaUltraCrucible = new(MinStepsSameDirection: 4, MaxStepsSameDirection: 10);
ClumsyCrucible ultraCrucible = new(clumsyCrucibleText, inertiaUltraCrucible);
Console.WriteLine($"Day 17 - Clumsy Crucible - Minimum heat loss for ultra crucible: {ultraCrucible.MinimumHeatLoss()}");

#endregion

#region Day 18 - Lavaduct Lagoon

string lavaductLagoonFile = input.GetPath(day: 18);
LavaductLagoon lavaductLagoonNoHex = parser.ParseLavaductLagoon(lavaductLagoonFile);
Console.WriteLine($"Day 18 - Lavaduct Lagoon - Volume of lagoon not using hex code: {lavaductLagoonNoHex.Volume()}");
LavaductLagoon lavaductLagoonHex = parser.ParseLavaductLagoon(lavaductLagoonFile, useHexCode: true);
Console.WriteLine($"Day 18 - Lavaduct Lagoon - Volume of lagoon using hex code: {lavaductLagoonHex.Volume()}");

#endregion

#region Day 19 - Aplenty

string aplentyFile = input.GetPath(day: 19);
(Aplenty aplenty, IEnumerable<MachinePartRating> ratings) = parser.ParseAplenty(aplentyFile, initialRule: "in");
IEnumerable<MachinePartRating> acceptedRatings = ratings.Where(rating => aplenty.MachinePartIsAccepted(rating));
Console.WriteLine($"Day 19 - Aplenty - Sum of all accepted ratings: {acceptedRatings.Sum(rating => rating.TotalRating)}");
RatingRange attributeRange = new(1, 4000);
RatingsRange attributeRanges = new(attributeRange, attributeRange, attributeRange, attributeRange);
long numAcceptedStates = aplenty.NumAcceptedStates(attributeRanges);
Console.WriteLine($"Day 19 - Aplenty - Number of accepted states: {numAcceptedStates}");

#endregion

#region Day 20 - Pulse Propagation

string pulsePropagationFile = input.GetPath(day: 20);
PulseCircuit pulseCircuit = parser.ParsePulseCircuit(pulsePropagationFile);
PulsePropagation pulsePropagation = new(pulseCircuit);
(int numLowPulses, int numHighPulses) = pulsePropagation.RunCircuitAndCountPulses(numTimes: 1000, initialPulseIntensity: PulseIntensity.Low);
int pulseProduct = numLowPulses * numHighPulses;
Console.WriteLine($"Day 20 - Pulse Propagation - Product of number of low pulses and number of high pulses: {pulseProduct}");

// Part 2 is a bit (or a lot) of a cheat. Had to notice that the output rx will only go low when ph, vn, kt and hn receive low inputs

List<long> numIterationsForPrecedingModules = new();
foreach (string moduleId in new string[] { "ph", "vn", "kt", "hn" })
{
    pulseCircuit = parser.ParsePulseCircuit(pulsePropagationFile);
    pulsePropagation = new(pulseCircuit);
    int numIterationsForModule = pulsePropagation.RunCircuitUntilGivenPulse(PulseIntensity.Low, moduleId, PulseIntensity.Low);
    numIterationsForPrecedingModules.Add(numIterationsForModule);
}
long totalNumIterations = HauntedWasteland.LeastCommonMultiple(numIterationsForPrecedingModules.ToArray());
Console.WriteLine($"Day 20 - Pulse Propagation - Number of iterations until rx is low: {totalNumIterations}");

#endregion

#region Day 21 - Step Counter

string stepCounterFile = input.GetPath(day: 21);
Garden garden = new(File.ReadAllText(stepCounterFile));
StepCounter stepCounter = new(garden);
HashSet<(int, int)> possiblePositions = stepCounter.PossiblePositionsAfterNSteps(numSteps: 64);
Console.WriteLine($"Day 21 - Step Counter - Number of possible positions after 64 steps: {possiblePositions.Count}");

// Part 2 is a cheat again, noticed that the number of possible positions for positions k + 131 * n grows as a parabola, for every k, n
// 131 is the width and height of the garden. If the garden were not square, I don't know what periodicity there would be

int numSteps = 26501365;
int period = 131; // Garden width and height
int firstStepInParabola = numSteps % period;
int[] stepsInParabola = new int[] { firstStepInParabola, firstStepInParabola + period, firstStepInParabola + 2 * period };
List<long> numPossiblePositions = stepCounter.NumPossiblePositionsInPacmanGarden(stepsInParabola[^1]).ToList();
long[] possiblePositionsInParabola = stepsInParabola.Select(index => numPossiblePositions[index]).ToArray();
long numPossiblePositionsPacmanGarden = StepCounter.ExtrapolateParabola(firstStepInParabola, period, possiblePositionsInParabola, numSteps);
Console.WriteLine($"Day 21 - Step Counter - Number of possible positions in infinite garden after {numSteps} steps: {numPossiblePositionsPacmanGarden}");

#endregion

#region Day 22 - Sand Slabs

string sandSlabsFile = input.GetPath(day: 22);
List<SandBrick> bricks = File
    .ReadAllLines(sandSlabsFile)
    .Select((line, index) => TextParser.ParseSandBrick($"B{index + 1}", line))
    .ToList();

SandSlabs sandSlabs = new(bricks);
sandSlabs.DropBricks();
Console.WriteLine($"Day 22 - Sand Slabs - Number of bricks safe to disintegrate: {sandSlabs.SafeToDisintegrateBrickIds().Count()}");
int totalFallenBricks = bricks.Sum(brick => sandSlabs.CountBricksThatFallWhenDisintegrating(brick.Id));
Console.WriteLine($"Day 22 - Sand Slabs - Sum of number of bricks that would fall: {totalFallenBricks}");

#endregion

#region Day 23 - A Long Walk

string longWalkFile = input.GetPath(day: 23);
Forest forest = new(File.ReadAllText(longWalkFile));
LongWalk longWalk = new(forest);
int maxNumStepsConsideringSlopes = longWalk.LengthLongestPath() - 1;
Console.WriteLine($"Day 23 - A Long Walk - Maximum number of steps considering slopes: {maxNumStepsConsideringSlopes}");
int maxNumStepsIgnoringSlopes = longWalk.LengthLongestPathIgnoringSlopes() - 1;
Console.WriteLine($"Day 23 - A Long Walk - Maximum number of steps ignoring slopes: {maxNumStepsIgnoringSlopes}");

#endregion

#region Day 24 - Hailstones

string hailstonesFile = input.GetPath(day: 24);
List<Hailstone> hailstones = File.ReadAllLines(hailstonesFile).Select(TextParser.ParseHailstone).ToList();
BoundingBox boundingBox = new(Min: (200000000000000, 200000000000000), Max: (400000000000000, 400000000000000));
Hailstones hailstonesSimulator = new(hailstones, boundingBox);
int numXYIntersections = hailstonesSimulator.NumFutureXYIntersections();
Console.WriteLine($"Day 24 - Hailstones - Number of future XY intersections: {numXYIntersections}");

#endregion