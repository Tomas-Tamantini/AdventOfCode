namespace AdventOfCode.Console.Models
{
    public static class MirageMaintenance
    {
        // TODO: Refactor this class to use a more efficient algorithm
        public static List<List<long>> Differences(List<long> sequence)
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
            return differences;
        }

        public static long NextTerm(List<long> sequence)
        {
            List<List<long>> differences = Differences(sequence);

            for (int i = differences.Count - 1; i >= 0; i--)
            {
                long nextTerm = differences[i][^1];
                if (i < differences.Count - 1) nextTerm += differences[i + 1][^1];
                differences[i].Add(nextTerm);
            }
            return differences[0][^1];
        }

        public static long PreviousTerm(List<long> sequence)
        {
            List<List<long>> differences = Differences(sequence);

            for (int i = differences.Count - 1; i >= 0; i--)
            {
                long previousTerm = differences[i][0];
                if (i < differences.Count - 1) previousTerm -= differences[i + 1][0];
                differences[i].Insert(0, previousTerm);
            }
            return differences[0][0];
        }

    }
}