namespace AdventOfCode.Console.Models
{
    public class CosmicExpansion
    {
        private readonly List<(int, int)> galaxies;
        private readonly int universeWidth;
        private readonly int universeHeight;
        private readonly int[] numEmptyRowsAccumulated;
        private readonly int[] numEmptyColumnsAccumulated;
        private readonly List<int> emptyRowIndices = new();
        private readonly List<int> emptyColumnIndices = new();

        public CosmicExpansion(int universeWidth, int universeHeight, List<(int, int)> galaxies)
        {
            this.galaxies = galaxies;
            this.universeWidth = universeWidth;
            this.universeHeight = universeHeight;
            numEmptyRowsAccumulated = AccumulatedEmptyRows();
            numEmptyColumnsAccumulated = AccumulatedEmptyColumns();
        }

        private int[] AccumulatedEmptyRows()
        {
            HashSet<int> emptyRows = Enumerable.Range(0, universeHeight).ToHashSet();
            foreach ((_, int y) in galaxies) emptyRows.Remove(y);
            List<int> emptyRowIndices = emptyRows.ToList();
            emptyRowIndices.Sort();
            var emptyRowsAccumulated = new int[universeHeight];
            foreach (int y in emptyRowIndices)
            {
                for (int i = y; i < universeHeight; i++) emptyRowsAccumulated[i]++;
            }
            return emptyRowsAccumulated;
        }

        private int[] AccumulatedEmptyColumns()
        {
            HashSet<int> emptyColumns = Enumerable.Range(0, universeWidth).ToHashSet();
            foreach ((int x, _) in galaxies) emptyColumns.Remove(x);
            List<int> emptyColumnIndices = emptyColumns.ToList();
            emptyColumnIndices.Sort();
            var emptyColumnsAccumulated = new int[universeWidth];
            foreach (int x in emptyColumnIndices)
            {
                for (int i = x; i < universeWidth; i++) emptyColumnsAccumulated[i]++;
            }
            return emptyColumnsAccumulated;
        }

        private int EmptyRowsBetween(int y1, int y2)
        {
            return Math.Abs(numEmptyRowsAccumulated[y1] - numEmptyRowsAccumulated[y2]);
        }

        private int EmptyColumnsBetween(int x1, int x2)
        {
            return Math.Abs(numEmptyColumnsAccumulated[x1] - numEmptyColumnsAccumulated[x2]);
        }

        public long DistanceBetweenGalaxies(int galaxy1Idx, int galaxy2Idx, int expansionRate = 2)
        {
            (int x1, int y1) = galaxies[galaxy1Idx];
            (int x2, int y2) = galaxies[galaxy2Idx];
            int expandedSpace = EmptyColumnsBetween(x1, x2) + EmptyRowsBetween(y1, y2);
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2) + expandedSpace * (expansionRate - 1);
        }

        public long SumDistancesBetweenAllPairsOfGalaxies(int expansionRate = 2)
        {
            long sum = 0;
            for (int i = 0; i < galaxies.Count - 1; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    sum += DistanceBetweenGalaxies(i, j, expansionRate);
                }
            }
            return sum;
        }
    }
}