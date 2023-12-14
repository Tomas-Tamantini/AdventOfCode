using System.Text;

namespace AdventOfCode.Console.Models
{
    enum GroundTile
    {
        Ground = '.',
        CubeRock = '#',
        RoundRock = 'O',
    }

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

        public void RollNorth()
        {
            // Loop through columns:
            for (int colIdx = 0; colIdx < _width; colIdx++)
            {
                RollColumnNorth(colIdx);
            }
        }

        public string ToString()
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
    }
}