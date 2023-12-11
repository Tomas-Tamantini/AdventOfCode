namespace AdventOfCode.Console.Models
{
    enum MazeTile
    {
        NSPipe = '|',
        EWPipe = '-',
        NEPipe = 'L',
        NWPipe = 'J',
        SWPipe = '7',
        SEPipe = 'F',
        Ground = '.',
        Start = 'S',
    }

    enum Direction { North, East, South, West };

    class MazeStructure
    {
        private readonly MazeTile[,] _maze;
        private readonly int _width;
        private readonly int _height;
        public int StartX { get; }
        public int StartY { get; }

        public (int, int) Start => (StartX, StartY);

        public MazeStructure(string input)
        {
            string[] lines = input.Split('\n');
            _width = lines[0].Trim().Length;
            _height = lines.Length;
            _maze = new MazeTile[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                string line = lines[y].Trim();
                for (int x = 0; x < _width; x++)
                {
                    _maze[x, y] = (MazeTile)line[x];
                    if (_maze[x, y] == MazeTile.Start)
                    {
                        StartX = x;
                        StartY = y;
                    }
                }
            }
        }

        private Direction? NextDirection(Direction incomingDirection, MazeTile nextTile)
        {
            return incomingDirection switch
            {
                Direction.North => nextTile switch
                {
                    MazeTile.NSPipe => Direction.North,
                    MazeTile.SWPipe => Direction.West,
                    MazeTile.SEPipe => Direction.East,
                    _ => null,
                },
                Direction.East => nextTile switch
                {
                    MazeTile.EWPipe => Direction.East,
                    MazeTile.NWPipe => Direction.North,
                    MazeTile.SWPipe => Direction.South,
                    _ => null,
                },
                Direction.South => nextTile switch
                {
                    MazeTile.NSPipe => Direction.South,
                    MazeTile.NEPipe => Direction.East,
                    MazeTile.NWPipe => Direction.West,
                    _ => null,
                },
                Direction.West => nextTile switch
                {
                    MazeTile.EWPipe => Direction.West,
                    MazeTile.NEPipe => Direction.North,
                    MazeTile.SEPipe => Direction.South,
                    _ => null,
                },
                _ => throw new Exception("Invalid direction"),
            };
        }

        public (int, int, Direction)? Neighbor(int x, int y, Direction direction)
        {
            (int newX, int newY) = direction switch
            {
                Direction.North => (x, y - 1),
                Direction.East => (x + 1, y),
                Direction.South => (x, y + 1),
                Direction.West => (x - 1, y),
                _ => throw new Exception("Invalid direction")
            };

            if (newX < 0 || newX >= _width || newY < 0 || newY >= _height) return null;
            MazeTile newTile = _maze[newX, newY];
            if (newTile == MazeTile.Ground) return null;
            if (newTile == MazeTile.Start) return (newX, newY, direction);

            Direction? nextDirection = NextDirection(direction, newTile);
            if (nextDirection == null) return null;
            return (newX, newY, nextDirection.Value);
        }
    }

    public class PipeMaze
    {
        private readonly MazeStructure _maze;

        public PipeMaze(string input)
        {
            // TODO: Inject dependency
            _maze = new MazeStructure(input);
        }

        private List<(int, int)>? LoopFromStartingPoint(Direction startingDirection)
        {
            List<(int, int)> loop = new() { _maze.Start };
            Direction directionNextStep = startingDirection;
            (int, int) currentPos = _maze.Start;
            while (true)
            {
                var feasibleNeighbor = _maze.Neighbor(currentPos.Item1, currentPos.Item2, directionNextStep);
                if (feasibleNeighbor == null) return null;
                (int neighborX, int neighborY, Direction nextDirection) = feasibleNeighbor.Value;
                if ((neighborX, neighborY) == _maze.Start) return loop;
                loop.Add((neighborX, neighborY));
                currentPos = (neighborX, neighborY);
                directionNextStep = nextDirection;
            }
        }

        public List<(int, int)>? Loop()
        {
            return Enum.GetValues(typeof(Direction))
                       .Cast<Direction>()
                       .Select(direction => LoopFromStartingPoint(direction))
                       .FirstOrDefault(loop => loop != null);
        }

        public int LoopLength()
        {
            List<(int, int)> loop = Loop() ?? throw new Exception("No loop found");
            return loop.Count;
        }

        public int LoopArea()
        {
            List<(int, int)> loop = Loop() ?? throw new Exception("No loop found");
            int twiceArea = 0;
            // Calculate area using shoelace theorem
            for (int i = 0; i < loop.Count; i++)
            {
                (int x1, int y1) = loop[i];
                (int x2, int y2) = i + 1 < loop.Count ? loop[i + 1] : loop[0];
                twiceArea += x1 * y2 - x2 * y1;
            }

            return Math.Abs(twiceArea / 2);
        }

        public int NumPointsInsideLoop()
        {
            // Find number of points inside loop by Pick's theorem
            return LoopArea() + 1 - LoopLength() / 2;
        }
    }
}