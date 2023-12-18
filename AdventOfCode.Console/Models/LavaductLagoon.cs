namespace AdventOfCode.Console.Models
{
    public record DigCommand(CardinalDirection Direction, long NumSteps);
    public class LavaductLagoon
    {
        private readonly IEnumerable<DigCommand> _digPlan;

        public LavaductLagoon(IEnumerable<DigCommand> digPlan)
        {
            _digPlan = digPlan;
        }

        private static (long, long) NewCorner((long, long) previousCorner, CardinalDirection direction, long numSteps)
        {
            return direction switch
            {
                CardinalDirection.North => (previousCorner.Item1, previousCorner.Item2 + numSteps),
                CardinalDirection.East => (previousCorner.Item1 + numSteps, previousCorner.Item2),
                CardinalDirection.South => (previousCorner.Item1, previousCorner.Item2 - numSteps),
                _ => (previousCorner.Item1 - numSteps, previousCorner.Item2),
            };
        }

        public long Volume()
        {
            List<(long, long)> corners = new() { (0, 0) };
            long numPointsOnEdge = 1;
            foreach (DigCommand digCommand in _digPlan)
            {
                (long, long) lastCorner = corners[^1];
                (long, long) newCorner = NewCorner(lastCorner, digCommand.Direction, digCommand.NumSteps);
                corners.Add(newCorner);
                numPointsOnEdge += digCommand.NumSteps;
            }

            // Shoelace formula
            long twiceArea = 0;
            for (int i = 0; i < corners.Count - 1; i++)
            {
                (long x1, long y1) = corners[i];
                (long x2, long y2) = corners[i + 1];
                twiceArea += x1 * y2 - x2 * y1;
            }

            double twicePoints = Math.Abs(twiceArea) + 1 + numPointsOnEdge;

            return (long)Math.Ceiling(twicePoints / 2);
        }
    }
}