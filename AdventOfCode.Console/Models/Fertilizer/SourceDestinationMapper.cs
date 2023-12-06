namespace AdventOfCode.Console.Models
{
    public record IntervalOffset(Interval Interval, long Offset);

    public class SourceDestinationMapper : IIntervalMapper
    {
        public string SourceName { get; init; }
        public string DestinationName { get; init; }
        private readonly List<IntervalOffset> intervalOffsets;

        public SourceDestinationMapper(string sourceName, string destinationName, List<IntervalOffset> intervalOffsets)
        {
            SourceName = sourceName;
            DestinationName = destinationName;
            this.intervalOffsets = intervalOffsets;
        }

        public IIntervalSet Map(IIntervalSet intervalSet)
        {
            IIntervalSet inputSet = intervalSet;
            IIntervalSet outputSet = new IntervalSet(new List<Interval>());
            foreach (var intervalOffset in intervalOffsets)
            {
                var bottomSlice = inputSet.Slice(end: intervalOffset.Interval.Start - 1);
                var middleSlice = inputSet.Slice(start: intervalOffset.Interval.Start, end: intervalOffset.Interval.End);
                var topSlice = inputSet.Slice(start: intervalOffset.Interval.End + 1);

                inputSet = bottomSlice.Merge(topSlice);
                outputSet = outputSet.Merge(middleSlice.Offset(intervalOffset.Offset));
            }
            return outputSet.Merge(inputSet);
        }
    }
}