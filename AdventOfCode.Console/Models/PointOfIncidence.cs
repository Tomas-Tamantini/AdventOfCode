namespace AdventOfCode.Console.Models
{
    public class PointOfIncidence
    {
        private readonly char[,] _tiles;
        private readonly int _width;
        private readonly int _height;

        public PointOfIncidence(string pattern)
        {
            string[] lines = pattern.Split('\n');
            _width = lines[0].Trim().Length;
            _height = lines.Length;
            _tiles = new char[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                string line = lines[y].Trim();
                for (int x = 0; x < _width; x++)
                {
                    _tiles[x, y] = line[x];
                }
            }
        }

        private bool IsSymmetricAboutColumn(int columnIdx, int numMismatches)
        {
            int totalMismatches = 0;
            for (int offset = 0; offset <= columnIdx; offset++)
            {
                int reflectedColIdx = columnIdx + offset + 1;
                if (reflectedColIdx >= _width) break;
                for (int row = 0; row < _height; row++)
                {
                    if (_tiles[columnIdx - offset, row] != _tiles[reflectedColIdx, row])
                    {
                        totalMismatches += 1;
                        if (totalMismatches > numMismatches) return false;
                    }
                }
            }
            return totalMismatches == numMismatches;
        }

        public int ColumnMirrorIdx(int numMismatches = 0)
        {
            for (int colIdx = 0; colIdx < _width - 1; colIdx++)
            {
                if (IsSymmetricAboutColumn(colIdx, numMismatches)) return colIdx;
            }
            return -1;
        }


        private bool IsSymmetricAboutRow(int rowIdx, int numMismatches)
        {
            int totalMismatches = 0;
            for (int offset = 0; offset <= rowIdx; offset++)
            {
                int reflectedRowIdx = rowIdx + offset + 1;
                if (reflectedRowIdx >= _height) break;
                for (int col = 0; col < _width; col++)
                {
                    if (_tiles[col, rowIdx - offset] != _tiles[col, reflectedRowIdx])
                    {
                        totalMismatches += 1;
                        if (totalMismatches > numMismatches) return false;
                    }
                }
            }
            return totalMismatches == numMismatches;
        }

        public int RowMirrorIdx(int numMismatches = 0)
        {
            for (int rowIdx = 0; rowIdx < _height - 1; rowIdx++)
            {
                if (IsSymmetricAboutRow(rowIdx, numMismatches)) return rowIdx;
            }
            return -1;
        }
    }
}