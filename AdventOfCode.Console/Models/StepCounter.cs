namespace AdventOfCode.Console.Models
{
    enum GardenTile
    {
        GardenPlot,
        Rock
    }
    public class Garden
    {
        private readonly int _width;
        private readonly int _height;
        private readonly GardenTile[,] _garden;
        private readonly (int, int) _startPosition;

        public Garden(string garden)
        {
            string[] rows = garden.Trim().Split("\n");
            _width = rows[0].Trim().Length;
            _height = rows.Length;
            _garden = new GardenTile[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                string row = rows[y].Trim();
                for (int x = 0; x < _width; x++)
                {
                    _garden[x, y] = row[x] == '#' ? GardenTile.Rock : GardenTile.GardenPlot;
                    if (row[x] == 'S') _startPosition = (x, y);
                }
            }
        }

        public (int, int) StartPosition => _startPosition;

        public IEnumerable<(int, int)> Neighbors((int, int) position)
        {
            foreach (CardinalDirection direction in Enum.GetValues(typeof(CardinalDirection)))
            {
                (int x, int y) = position;
                switch (direction)
                {
                    case CardinalDirection.North:
                        y--;
                        break;
                    case CardinalDirection.East:
                        x++;
                        break;
                    case CardinalDirection.South:
                        y++;
                        break;
                    case CardinalDirection.West:
                        x--;
                        break;
                }
                if (x >= 0 && x < _width && y >= 0 && y < _height && _garden[x, y] == GardenTile.GardenPlot)
                {
                    yield return (x, y);
                }
            }
        }
    }

    public class StepCounter
    {
        private readonly Garden _garden;

        public StepCounter(Garden garden)
        {
            _garden = garden;
        }

        public HashSet<(int, int)> PossiblePositionsAfterNSteps(int numSteps)
        {
            HashSet<(int, int)> currentPositions = new() { _garden.StartPosition };
            for (int i = 0; i < numSteps; i++)
            {
                HashSet<(int, int)> nextPositions = new();
                foreach ((int, int) position in currentPositions)
                {
                    foreach ((int, int) neighbor in _garden.Neighbors(position))
                    {
                        nextPositions.Add(neighbor);
                    }
                }
                currentPositions = nextPositions;
            }
            return currentPositions;
        }

    }
}