using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Console.Models
{
    enum LavaTile
    {
        Ash = '.',
        Rock = '#'
    }

    public class PointOfIncidence
    {
        private readonly LavaTile[,] _tiles;
        private readonly int _width;
        private readonly int _height;

        public PointOfIncidence(string pattern)
        {
            string[] lines = pattern.Split('\n');
            _width = lines[0].Trim().Length;
            _height = lines.Length;
            _tiles = new LavaTile[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                string line = lines[y].Trim();
                for (int x = 0; x < _width; x++)
                {
                    _tiles[x, y] = (LavaTile)line[x];
                }
            }
        }

        private bool ColumnsAreEqual(int columnIdxA, int columnIdxB)
        {
            for (int row = 0; row < _height; row++)
            {
                if (_tiles[columnIdxA, row] != _tiles[columnIdxB, row]) return false;
            }
            return true;
        }

        private bool IsSymmetricAboutColumn(int columnIdx)
        {
            for (int offset = 0; offset <= columnIdx; offset++)
            {
                int reflectedIdx = columnIdx + offset + 1;
                if (reflectedIdx >= _width) break;
                if (!ColumnsAreEqual(columnIdx - offset, reflectedIdx)) return false;
            }
            return true;
        }

        public int ColumnMirrorIdx()
        {
            for (int colIdx = 0; colIdx < _width - 1; colIdx++)
            {
                if (IsSymmetricAboutColumn(colIdx)) return colIdx;
            }
            return -1;
        }

        private bool RowsAreEqual(int rowIdxA, int rowIdxB)
        {
            for (int col = 0; col < _width; col++)
            {
                if (_tiles[col, rowIdxA] != _tiles[col, rowIdxB]) return false;
            }
            return true;
        }

        private bool IsSymmetricAboutRow(int rowIdx)
        {
            for (int offset = 0; offset <= rowIdx; offset++)
            {
                int reflectedIdx = rowIdx + offset + 1;
                if (reflectedIdx >= _height) break;
                if (!RowsAreEqual(rowIdx - offset, reflectedIdx)) return false;
            }
            return true;
        }

        public int RowMirrorIdx()
        {
            for (int rowIdx = 0; rowIdx < _height - 1; rowIdx++)
            {
                if (IsSymmetricAboutRow(rowIdx)) return rowIdx;
            }
            return -1;
        }
    }
}