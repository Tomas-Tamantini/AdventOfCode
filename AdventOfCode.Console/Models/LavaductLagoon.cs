namespace AdventOfCode.Console.Models
{
    public record DigCommand(CardinalDirection Direction, int NumSteps);
    public class LavaductLagoon
    {
        private readonly IEnumerable<DigCommand> _digPlan;

        public LavaductLagoon(IEnumerable<DigCommand> digPlan)
        {
            _digPlan = digPlan;
        }

        private static (int, int) NewCorner((int, int) previousCorner, CardinalDirection direction, int numSteps)
        {
            return direction switch
            {
                CardinalDirection.North => (previousCorner.Item1, previousCorner.Item2 + numSteps),
                CardinalDirection.East => (previousCorner.Item1 + numSteps, previousCorner.Item2),
                CardinalDirection.South => (previousCorner.Item1, previousCorner.Item2 - numSteps),
                _ => (previousCorner.Item1 - numSteps, previousCorner.Item2),
            };
        }

        public int Volume()
        {
            List<(int, int)> corners = new() { (0, 0) };
            int numPointsOnEdge = 1;
            foreach (DigCommand digCommand in _digPlan)
            {
                (int, int) lastCorner = corners[^1];
                (int, int) newCorner = NewCorner(lastCorner, digCommand.Direction, digCommand.NumSteps);
                corners.Add(newCorner);
                numPointsOnEdge += digCommand.NumSteps;
            }

            // Shoelace formula
            int twiceArea = 0;
            for (int i = 0; i < corners.Count - 1; i++)
            {
                (int x1, int y1) = corners[i];
                (int x2, int y2) = corners[i + 1];
                twiceArea += x1 * y2 - x2 * y1;
            }

            double twicePoints = Math.Abs(twiceArea) + 1 + numPointsOnEdge;
            return (int)Math.Ceiling(twicePoints / 2);
        }
    }
}