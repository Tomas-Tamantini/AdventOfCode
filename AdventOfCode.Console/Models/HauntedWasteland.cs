namespace AdventOfCode.Console.Models
{
    public class HauntedWasteland
    {
        public Dictionary<string, (string, string)> Network { get; init; }
        public string Path { get; init; }

        private IEnumerable<char> PathCycle()
        {
            while (true)
                foreach (char step in Path) yield return step;
        }

        public int NumSteps(string origin, string destination)
        {
            int numSteps = 0;
            string currentNode = origin;
            foreach (var step in PathCycle())
            {
                if (currentNode == destination) return numSteps;
                currentNode = (step == 'L') ? Network[currentNode].Item1 : Network[currentNode].Item2;
                numSteps += 1;
            }
            return numSteps;
        }
    }
}