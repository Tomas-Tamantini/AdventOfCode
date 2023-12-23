namespace AdventOfCode.Console.Models
{
    enum HikeTile
    {
        Path = '.',
        Forest = '#',
        SlopeNorth = '^',
        SlopeSouth = 'v',
        SlopeEast = '>',
        SlopeWest = '<'
    }

    public class Forest
    {
        private readonly HikeTile[,] _tiles;
        private readonly int _width;
        private readonly int _height;
        private readonly (int, int) _startPosition;
        private readonly (int, int) _endPosition;

        public (int, int) StartPosition => _startPosition;
        public (int, int) EndPosition => _endPosition;

        public Forest(string input)
        {
            string[] lines = input.Split('\n');
            _width = lines[0].Trim().Length;
            _height = lines.Length;
            _tiles = new HikeTile[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                string line = lines[y].Trim();
                for (int x = 0; x < _width; x++)
                {
                    _tiles[x, y] = (HikeTile)line[x];
                    if (_tiles[x, y] == HikeTile.Path)
                    {
                        if (y == 0) { _startPosition = (x, y); }
                        if (y == _height - 1) { _endPosition = (x, y); }
                    }
                }
            }
        }

        private static IEnumerable<(int, int)> NeighboringCoordinates((int, int) currentCoordinates)
        {
            (int x, int y) = currentCoordinates;
            yield return (x + 1, y);
            yield return (x - 1, y);
            yield return (x, y + 1);
            yield return (x, y - 1);
        }

        private bool IsOutOfBounds((int, int) coordinates)
        {
            (int x, int y) = coordinates;
            return x < 0 || y < 0 || x >= _width || y >= _height;
        }

        private bool ViolatesSlope((int, int) currentPosition, (int, int) nextPosition)
        {
            var currentTile = _tiles[currentPosition.Item1, currentPosition.Item2];
            int dx = nextPosition.Item1 - currentPosition.Item1;
            int dy = nextPosition.Item2 - currentPosition.Item2;
            return currentTile switch
            {
                HikeTile.SlopeEast => dx != 1,
                HikeTile.SlopeWest => dx != -1,
                HikeTile.SlopeSouth => dy != 1,
                HikeTile.SlopeNorth => dy != -1,
                _ => false,
            };
        }

        public IEnumerable<(int, int)> ValidNeighbors((int, int) currentPosition)
        {
            foreach (var neighbor in NeighboringCoordinates(currentPosition))
            {
                if (IsOutOfBounds(neighbor) || _tiles[neighbor.Item1, neighbor.Item2] == HikeTile.Forest) continue;
                if (ViolatesSlope(currentPosition, neighbor)) continue;
                yield return neighbor;
            }
        }

    }
    public class LongWalk
    {
        private readonly Forest _forest;
        public LongWalk(Forest forest)
        {
            _forest = forest;
        }

        public int LengthLongestPath()
        {
            HashSet<(int, int)> visitedTiles = new();
            return LengthLongestPathRecursive(_forest.StartPosition, visitedTiles);

        }

        private int LengthLongestPathRecursive((int, int) currentPosition, HashSet<(int, int)> visitedPositions)
        {
            if (currentPosition == _forest.EndPosition) return 1;
            visitedPositions.Add(currentPosition);
            int maxLength = -1;
            foreach (var neighbor in _forest.ValidNeighbors(currentPosition))
            {
                if (visitedPositions.Contains(neighbor)) continue;
                int pathLength = LengthLongestPathRecursive(neighbor, visitedPositions);
                maxLength = Math.Max(maxLength, pathLength);
            }
            visitedPositions.Remove(currentPosition);
            return maxLength >= 0 ? maxLength + 1 : -1;
        }
    }
}