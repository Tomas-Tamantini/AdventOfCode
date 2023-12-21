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

        private static IEnumerable<(int, int)> NeighborCoordinates((int, int) position)
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
                yield return (x, y);
            }
        }

        public IEnumerable<(int, int)> Neighbors((int, int) position)
        {
            foreach ((int x, int y) in NeighborCoordinates(position))
            {
                if (x >= 0 && x < _width && y >= 0 && y < _height && _garden[x, y] == GardenTile.GardenPlot)
                {
                    yield return (x, y);
                }
            }
        }

        public IEnumerable<(int, int)> PacmanNeighbors((int, int) position)
        {
            foreach ((int neighborX, int neighborY) in NeighborCoordinates(position))
            {
                int x = (neighborX % _width + _width) % _width;
                int y = (neighborY % _height + _height) % _height;
                if (_garden[x, y] == GardenTile.GardenPlot)
                {
                    yield return (neighborX, neighborY);
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

        private HashSet<(int, int)> NextPossiblePositions(HashSet<(int, int)> currentPositions, bool pacmanGarden = false)
        {
            HashSet<(int, int)> nextPositions = new();
            foreach ((int, int) position in currentPositions)
            {
                foreach ((int, int) neighbor in pacmanGarden ? _garden.PacmanNeighbors(position) : _garden.Neighbors(position))
                {
                    nextPositions.Add(neighbor);
                }
            }
            return nextPositions;
        }

        public HashSet<(int, int)> PossiblePositionsAfterNSteps(int numSteps)
        {
            HashSet<(int, int)> currentPositions = new() { _garden.StartPosition };
            for (int _ = 0; _ < numSteps; _++)
            {
                HashSet<(int, int)> nextPositions = NextPossiblePositions(currentPositions);
                currentPositions = nextPositions;
            }
            return currentPositions;
        }

        public IEnumerable<long> NumPossiblePositionsInPacmanGarden(int totalSteps)
        {
            HashSet<(int, int)> currentPositions = new() { _garden.StartPosition };
            yield return currentPositions.Count;
            for (int _ = 0; _ < totalSteps; _++)
            {
                HashSet<(int, int)> nextPositions = NextPossiblePositions(currentPositions, pacmanGarden: true);
                currentPositions = nextPositions;
                yield return currentPositions.Count;
            }
        }

        public static long ExtrapolateParabola(int x0, int stepSize, long[] y, int newX)
        {
            long a = (y[0] - 2 * y[1] + y[2]) / 2;
            long b = (-3 * y[0] + 4 * y[1] - y[2]) / 2;
            long c = y[0];

            long xTranslated = (newX - x0) / stepSize;
            return a * xTranslated * xTranslated + b * xTranslated + c;
        }

    }
}