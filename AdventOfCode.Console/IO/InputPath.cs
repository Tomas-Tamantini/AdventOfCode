namespace AdventOfCode.Console.IO
{
    internal class InputPath
    {
        private readonly string inputFolder;

        public InputPath()
        {
            string defaultFolder = "C:/Users/EQ67/source/repos/Tomas-Tamantini/AdventOfCode/AdventOfCode.Console/InputFiles";
            inputFolder = Environment.GetEnvironmentVariable("INPUT_FOLDER") ?? defaultFolder;
        }

        public string GetPath(int day)
        {
            List<string> inputFiles = new()
            {
                "Trebuchet",
                "CubeConundrum",
                "GearRatios",
                "Scratchcards",
                "Fertilizer",
                "BoatRace",
                "CamelCards",
                "HauntedWasteland",
                "MirageMaintenance",
                "PipeMaze",
                "CosmicExpansion",
                "HotSprings",
                "PointOfIncidence",
                "ParabolicReflectorDish",
                "LensLibrary",
                "LavaFloor",
                "ClumsyCrucible",
                "LavaductLagoon",
                "Aplenty",
                "PulsePropagation",
                "StepCounter",
                "SandSlabs",
                "LongWalk",
                "Hailstones",
                "Snowverload",
            };
            return Path.Combine(inputFolder, $"{inputFiles[day - 1]}Input.txt");
        }
    }
}