using System.Text;

namespace AdventOfCode.Console.Models
{
    enum GroundTile
    {
        Ground = '.',
        CubeRock = '#',
        RoundRock = 'O',
    }

    public enum CardinalDirection { North, East, South, West }

    public class ParabolicReflectorDish
    {
        private readonly GroundTile[,] _tiles;
        private readonly int _width;
        private readonly int _height;

        public ParabolicReflectorDish(string input)
        {
            string[] lines = input.Split('\n');
            _width = lines[0].Trim().Length;
            _height = lines.Length;
            _tiles = new GroundTile[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                string line = lines[y].Trim();
                for (int x = 0; x < _width; x++)
                {
                    _tiles[x, y] = (GroundTile)line[x];
                }
            }
        }

        private void RollColumnNorth(int columnIdx)
        {
            int obstaclePointer = -1;
            for (int probePointer = 0; probePointer < _height; probePointer++)
            {
                GroundTile currentTile = _tiles[columnIdx, probePointer];
                if (currentTile == GroundTile.CubeRock) obstaclePointer = probePointer;
                else if (currentTile == GroundTile.RoundRock)
                {
                    obstaclePointer += 1;
                    _tiles[columnIdx, probePointer] = GroundTile.Ground;
                    _tiles[columnIdx, obstaclePointer] = GroundTile.RoundRock;
                }
            }
        }

        private void RollNorth()
        {
            for (int colIdx = 0; colIdx < _width; colIdx++)
            {
                RollColumnNorth(colIdx);
            }
        }

        private void RollColumnSouth(int columnIdx)
        {
            int obstaclePointer = _height;
            for (int probePointer = _height - 1; probePointer >= 0; probePointer--)
            {
                GroundTile currentTile = _tiles[columnIdx, probePointer];
                if (currentTile == GroundTile.CubeRock) obstaclePointer = probePointer;
                else if (currentTile == GroundTile.RoundRock)
                {
                    obstaclePointer -= 1;
                    _tiles[columnIdx, probePointer] = GroundTile.Ground;
                    _tiles[columnIdx, obstaclePointer] = GroundTile.RoundRock;
                }
            }
        }

        private void RollSouth()
        {
            for (int colIdx = 0; colIdx < _width; colIdx++)
            {
                RollColumnSouth(colIdx);
            }
        }

        private void RollRowEast(int rowIdx)
        {
            int obstaclePointer = _width;
            for (int probePointer = _width - 1; probePointer >= 0; probePointer--)
            {
                GroundTile currentTile = _tiles[probePointer, rowIdx];
                if (currentTile == GroundTile.CubeRock) obstaclePointer = probePointer;
                else if (currentTile == GroundTile.RoundRock)
                {
                    obstaclePointer -= 1;
                    _tiles[probePointer, rowIdx] = GroundTile.Ground;
                    _tiles[obstaclePointer, rowIdx] = GroundTile.RoundRock;
                }
            }

        }

        private void RollEast()
        {
            for (int rowIdx = 0; rowIdx < _height; rowIdx++)
            {
                RollRowEast(rowIdx);
            }
        }

        private void RollRowWest(int rowIdx)
        {
            int obstaclePointer = -1;
            for (int probePointer = 0; probePointer < _width; probePointer++)
            {
                GroundTile currentTile = _tiles[probePointer, rowIdx];
                if (currentTile == GroundTile.CubeRock) obstaclePointer = probePointer;
                else if (currentTile == GroundTile.RoundRock)
                {
                    obstaclePointer += 1;
                    _tiles[probePointer, rowIdx] = GroundTile.Ground;
                    _tiles[obstaclePointer, rowIdx] = GroundTile.RoundRock;
                }
            }
        }

        private void RollWest()
        {
            for (int rowIdx = 0; rowIdx < _height; rowIdx++)
            {
                RollRowWest(rowIdx);
            }
        }

        public void Roll(CardinalDirection direction)
        {
            if (direction == CardinalDirection.North) RollNorth();
            else if (direction == CardinalDirection.South) RollSouth();
            else if (direction == CardinalDirection.East) RollEast();
            else if (direction == CardinalDirection.West) RollWest();
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            for (int y = 0; y < _height; y++)
            {
                StringBuilder line = new();
                for (int x = 0; x < _width; x++)
                {
                    line.Append((char)_tiles[x, y]);
                }
                sb.AppendLine(line.ToString());
            }
            return sb.ToString().Trim();
        }

        public IEnumerable<int> RoundRocksPerRow()
        {
            for (int y = 0; y < _height; y++)
            {
                int roundRocks = 0;
                for (int x = 0; x < _width; x++)
                {
                    if (_tiles[x, y] == GroundTile.RoundRock) roundRocks++;
                }
                yield return roundRocks;
            }
        }

        public int TorqueOnSouthHinge()
        {
            int[] distances = Enumerable.Range(1, _height).Reverse().ToArray();
            return RoundRocksPerRow().Zip(distances, (load, distance) => load * distance).Sum();
        }
    }
}