using System.Data;

namespace AdventOfCode.Console.Models
{
    public class TerminalSteps
    {
        public List<int> AperiodicTerminalSteps { get; init; } = new();
        public List<int> PeriodicTerminalSteps { get; init; } = new();
        public int Period { get; init; } = 1;
    }

    public class HauntedWasteland
    {
        public Dictionary<string, (string, string)> Network { get; init; }
        public string Path { get; init; }

        private IEnumerable<char> PathCycle()
        {
            while (true)
                foreach (char step in Path) yield return step;
        }
        private string NextNode(string currentNode, char step)
        {
            return (step == 'L') ? Network[currentNode].Item1 : Network[currentNode].Item2;
        }

        public int NumStepsSinglePath(string origin, string destination)
        {
            int numSteps = 0;
            string currentNode = origin;
            foreach (var step in PathCycle())
            {
                if (currentNode == destination) return numSteps;
                currentNode = NextNode(currentNode, step);
                numSteps += 1;
            }
            return numSteps;
        }

        private (List<string>, int) Periodicity(string node)
        {
            List<(string, int)> sequentialStates = new() { (node, 0) };
            HashSet<(string, int)> visitedStates = new() { (node, 0) };
            int currentStepIdx = 0;
            string currentNode = node;
            while (true)
            {
                char step = Path[currentStepIdx];
                currentNode = NextNode(currentNode, step);
                currentStepIdx = (currentStepIdx + 1) % Path.Length;
                var currentState = (currentNode, currentStepIdx);
                if (!visitedStates.Contains(currentState))
                {
                    sequentialStates.Add(currentState);
                    visitedStates.Add(currentState);
                }
                else
                {
                    int matchingIndex = sequentialStates.IndexOf(currentState);
                    int period = visitedStates.Count - matchingIndex;
                    List<string> nodes = sequentialStates.Select(x => x.Item1).ToList();
                    return (nodes, period);
                }
            }
        }

        public long NumStepsSimultaneousPaths(char lastCharOriginNodes, char lastCharDestinationNodes)
        {
            var startingNodes = Network.Keys.Where(key => key.EndsWith(lastCharOriginNodes));
            List<TerminalSteps> terminalStepsForStartingNodes = startingNodes.
            Select(node => FindTerminalSteps(node, node => node.EndsWith(lastCharDestinationNodes))).ToList();
            // TODO: Properly implement the algorithm. Line below only works if there are no aperiodic terms and
            // if there is only one periodic term, whose value is equal to the period
            // Luckily, such is the case for the input
            return LeastCommonMultiple(terminalStepsForStartingNodes.Select(t => (long)t.Period).ToArray());
        }

        public static long LeastCommonMultiple(params long[] numbers)
        {
            long FindGCD(long a, long b)
            {
                while (b != 0)
                {
                    long temp = b;
                    b = a % b;
                    a = temp;
                }
                return a;
            }

            long FindLCM(long a, long b)
            {
                return Math.Abs(a * b) / FindGCD(a, b);
            }

            return numbers.Aggregate((currentLCM, nextNumber) => FindLCM(currentLCM, nextNumber));
        }

        private TerminalSteps FindTerminalSteps(string node, Func<string, bool> isTerminal)
        {
            (List<string> visitedNodes, int period) = Periodicity(node);
            List<int> aperiodicSteps = new();
            List<int> periodicSteps = new();
            for (int i = 0; i < visitedNodes.Count; i++)
            {
                string currentNode = visitedNodes[i];
                if (!isTerminal(currentNode)) continue;
                bool isInsidePeriodicPhase = i >= visitedNodes.Count - period;
                if (isInsidePeriodicPhase) periodicSteps.Add(i);
                else aperiodicSteps.Add(i);
            }
            TerminalSteps newTerminalStep = new()
            {
                AperiodicTerminalSteps = aperiodicSteps,
                PeriodicTerminalSteps = periodicSteps,
                Period = period
            };
            return newTerminalStep;
        }
    }
}