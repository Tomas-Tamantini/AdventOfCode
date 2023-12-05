namespace AdventOfCode.Console.Models
{
    public class UlongInterval : IComparable<UlongInterval>
    {
        public ulong Start { get; init; }
        public ulong End { get; init; }

        public UlongInterval? Intersection(UlongInterval other)
        {
            if (Start > other.End || other.Start > End) return null;
            return new UlongInterval { Start = Math.Max(Start, other.Start), End = Math.Min(End, other.End) };
        }

        public List<UlongInterval> Difference(UlongInterval other)
        {
            var intersection = Intersection(other);
            if (intersection == null) return new List<UlongInterval> { this };
            var difference = new List<UlongInterval>();
            if (intersection.Start > this.Start) difference.Add(new UlongInterval { Start = this.Start, End = intersection.Start - 1 });
            if (intersection.End < this.End) difference.Add(new UlongInterval { Start = intersection.End + 1, End = this.End });
            return difference;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            UlongInterval other = (UlongInterval)obj;
            return Start == other.Start && End == other.End;
        }

        public int CompareTo(UlongInterval other)
        {
            return Start.CompareTo(other.Start);
        }
    }

    public class RangeMapper
    {
        private readonly ulong sourceStart;
        private readonly ulong destinationStart;
        private readonly ulong rangeLength;

        public RangeMapper(ulong sourceStart, ulong destinationStart, ulong rangeLength)
        {
            this.sourceStart = sourceStart;
            this.destinationStart = destinationStart;
            this.rangeLength = rangeLength;
        }

        public ulong? Map(ulong source)
        {
            if (source < sourceStart || source >= sourceStart + rangeLength) return null;
            return destinationStart + (source - sourceStart);
        }

        public (UlongInterval?, List<UlongInterval>) MapInterval(UlongInterval interval)
        {
            var sourceInterval = new UlongInterval { Start = sourceStart, End = sourceStart + rangeLength - 1 };
            var intersection = interval.Intersection(sourceInterval);
            if (intersection == null) return (null, new List<UlongInterval> { interval });
            var difference = interval.Difference(sourceInterval);
            var outputInterval = new UlongInterval
            {
                Start = intersection.Start + destinationStart - sourceStart,
                End = intersection.End + destinationStart - sourceStart
            };
            return (outputInterval, difference);
        }
    }

    public class SourceDestinationMapper
    {
        public string SourceName { get; }
        public string DestinationName { get; }
        private readonly List<RangeMapper> rangeMappers;

        public SourceDestinationMapper(string sourceName, string destinationName, List<RangeMapper> rangeMappers)
        {
            SourceName = sourceName;
            DestinationName = destinationName;
            this.rangeMappers = rangeMappers;
        }

        public ulong Map(ulong source)
        {
            return rangeMappers.Select(rangeMapper => rangeMapper.Map(source))
                               .FirstOrDefault(result => result != null) ?? source;
        }

        public List<UlongInterval> MapIntervals(List<UlongInterval> sourceIntervals)
        {
            var outputIntervals = new List<UlongInterval>();
            foreach (var rangeMapper in rangeMappers)
            {
                var newSourceIntervals = new List<UlongInterval>();
                foreach (var sourceInterval in sourceIntervals)
                {
                    (UlongInterval? outputInterval, List<UlongInterval> leftoverInputIntervals) = rangeMapper.MapInterval(sourceInterval);
                    if (outputInterval != null) outputIntervals.Add(outputInterval);
                    foreach (var newInputInterval in leftoverInputIntervals) newSourceIntervals.Add(newInputInterval);
                }
                sourceIntervals = newSourceIntervals.ToList();
            }
            var allIntervals = outputIntervals.Concat(sourceIntervals).ToList();
            // TODO: Merge and sort intervals (maybe merge them in the loop still. Create a class to encapsulate the list of intervals, and optimize it)
            return allIntervals;
        }
    }

    public class ChainMapper
    {
        private readonly Dictionary<string, SourceDestinationMapper> mappers;

        public ChainMapper()
        {
            mappers = new Dictionary<string, SourceDestinationMapper>();
        }

        public ChainMapper(List<SourceDestinationMapper> sourceDestinationMappers)
        {
            mappers = sourceDestinationMappers.ToDictionary(mapper => mapper.SourceName);
        }

        public virtual ulong Map(string sourceName, string destinationName, ulong value)
        {
            if (sourceName == destinationName) return value;
            string currentSource = sourceName;
            ulong currentValue = value;
            while (true)
            {
                if (!mappers.ContainsKey(currentSource)) throw new ArgumentException("Invalid source-destination");
                var map = this.mappers[currentSource];
                currentValue = map.Map(currentValue);
                if (map.DestinationName == destinationName) return currentValue;
                currentSource = map.DestinationName;
            }
        }

        public virtual List<UlongInterval> MapIntervals(string sourceName, string destinationName, List<UlongInterval> sourceIntervals)
        {
            if (sourceName == destinationName) return sourceIntervals;
            string currentSource = sourceName;
            List<UlongInterval> currentIntervals = sourceIntervals.ToList();
            while (true)
            {
                if (!mappers.ContainsKey(currentSource)) throw new ArgumentException("Invalid source-destination");
                var map = this.mappers[currentSource];
                currentIntervals = map.MapIntervals(currentIntervals);
                if (map.DestinationName == destinationName) return currentIntervals;
                currentSource = map.DestinationName;
            }
        }
    }

    public class Fertilizer
    {
        private readonly ChainMapper chainMapper;
        private readonly List<ulong> seeds;


        public Fertilizer(List<ulong> seeds, ChainMapper chainMapper)
        {
            this.seeds = seeds;
            this.chainMapper = chainMapper;
        }

        public ulong LowestLocationNumber()
        {
            return seeds.Min(seed => chainMapper.Map("seed", "location", seed));
        }

        public ulong LowestLocationNumberWithSeedsAsRange()
        {
            List<UlongInterval> seedIntervals = BuildSeedIntervals();
            List<UlongInterval> locationIntervals = chainMapper.MapIntervals("seed", "location", seedIntervals);
            return locationIntervals.Min(interval => interval.Start);
        }

        private List<UlongInterval> BuildSeedIntervals()
        {
            List<UlongInterval> seedIntervals = new();
            for (var i = 0; i < seeds.Count; i += 2)
                seedIntervals.Add(new UlongInterval { Start = seeds[i], End = seeds[i] + seeds[i + 1] - 1 });
            return seedIntervals;
        }
    }
}