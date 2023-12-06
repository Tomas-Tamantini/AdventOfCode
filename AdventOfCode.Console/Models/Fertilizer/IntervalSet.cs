namespace AdventOfCode.Console.Models
{
    public class Interval
    {
        public long Start { get; init; }
        public long End { get; init; }
        public bool Contains(long number)
        {
            return number >= Start && number <= End;
        }
    }

    public interface IIntervalSet
    {
        bool Contains(long number);
        bool ContainsAll(params long[] numbers);
        bool ContainsAny(params long[] numbers);
        long LowestNumber();
        bool IsEmpty();
        IIntervalSet Slice(long? start = null, long? end = null);
        IIntervalSet Merge(IIntervalSet other);
        IntervalSet Offset(long offset);
    }

    public class IntervalSet : IIntervalSet
    {
        private readonly List<Interval> intervals;

        public IntervalSet(List<Interval> intervals)
        {
            this.intervals = intervals;
        }

        public bool Contains(long number)
        {
            return intervals.Any(x => x.Contains(number));
        }

        public bool ContainsAll(params long[] numbers)
        {
            return numbers.All(x => Contains(x));
        }
        public bool ContainsAny(params long[] numbers)
        {
            return numbers.Any(x => Contains(x));
        }

        public long LowestNumber()
        {
            return intervals.Min(x => x.Start);
        }

        public bool IsEmpty()
        {
            return intervals.Count == 0;
        }

        public IIntervalSet Slice(long? start = null, long? end = null)
        {
            List<Interval> slicedIntervals = new();
            foreach (var interval in intervals)
            {
                long actualStart = start ?? interval.Start;
                long actualEnd = end ?? interval.End;
                if (interval.Start > actualEnd) continue;
                if (interval.End < actualStart) continue;
                var newStart = Math.Max(interval.Start, actualStart);
                var newEnd = Math.Min(interval.End, actualEnd);
                slicedIntervals.Add(new Interval { Start = newStart, End = newEnd });
            }
            return new IntervalSet(slicedIntervals);
        }

        public IIntervalSet Merge(IIntervalSet other)
        {
            var mergedIntervals = new List<Interval>();
            mergedIntervals.AddRange(intervals);
            mergedIntervals.AddRange((other as IntervalSet).intervals);
            return new IntervalSet(mergedIntervals);

        }

        public IntervalSet Offset(long offset)
        {
            var offsetIntervals = new List<Interval>();
            foreach (var interval in intervals)
            {
                offsetIntervals.Add(new Interval { Start = interval.Start + offset, End = interval.End + offset });
            }
            return new IntervalSet(offsetIntervals);
        }
    }

    public interface IIntervalMapper
    {
        IIntervalSet Map(IIntervalSet intervalSet);
    }
}