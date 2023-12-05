namespace AdventOfCode.Console.Models
{
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
    }

    public class ChainMapper
    {
        private readonly Dictionary<string, SourceDestinationMapper> mappers;

        public ChainMapper(List<SourceDestinationMapper> sourceDestinationMappers)
        {
            mappers = sourceDestinationMappers.ToDictionary(mapper => mapper.SourceName);
        }

        public ulong Map(string sourceName, string destinationName, ulong value)
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
    }
}