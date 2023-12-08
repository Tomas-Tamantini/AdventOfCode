namespace AdventOfCode.Console.Models
{
    public class HauntedWasteland
    {
        private readonly Dictionary<string, (string, string)> network;

        public HauntedWasteland(Dictionary<string, (string, string)> network)
        {
            this.network = network;
        }

        private static IEnumerable<char> PathCycle(string path)
        {
            while (true)
                foreach (char step in path) yield return step;
        }

        public int NumSteps(string origin, string destination, string path)
        {
            int numSteps = 0;
            string currentNode = origin;
            foreach (var step in PathCycle(path))
            {
                if (currentNode == destination) return numSteps;
                currentNode = (step == 'L') ? network[currentNode].Item1 : network[currentNode].Item2;
                numSteps += 1;
            }
            return numSteps;
        }
    }
}