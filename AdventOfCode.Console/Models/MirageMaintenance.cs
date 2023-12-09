namespace AdventOfCode.Console.Models
{
    public static class MirageMaintenance
    {
        public static long NextTerm(List<long> sequence)
        {
            List<List<long>> differences = new() { sequence };
            for (int i = 0; i < sequence.Count - 1; i++)
            {
                List<long> difference = new();
                for (int j = 0; j < differences[i].Count - 1; j++)
                {
                    difference.Add(differences[i][j + 1] - differences[i][j]);
                }
                if (difference.All(d => d == 0)) break;
                differences.Add(difference);
            }

            for (int i = differences.Count - 1; i >= 0; i--)
            {
                long nextTerm = differences[i][^1];
                if (i < differences.Count - 1) nextTerm += differences[i + 1][^1];
                differences[i].Add(nextTerm);
            }
            return differences[0][^1];
        }

    }
}