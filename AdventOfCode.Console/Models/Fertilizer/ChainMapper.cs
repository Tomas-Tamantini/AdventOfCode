namespace AdventOfCode.Console.Models
{
    public class ChainMapper : IIntervalMapper
    {
        private readonly List<IIntervalMapper> mappers;

        public ChainMapper(List<IIntervalMapper> mappers)
        {
            this.mappers = mappers;
        }

        public ChainMapper(string sourceName, string destinationName, List<SourceDestinationMapper> mappers)
        {
            Dictionary<string, SourceDestinationMapper> mapperDictionary = mappers.ToDictionary(x => x.SourceName);
            this.mappers = new List<IIntervalMapper>();
            while (sourceName != destinationName && mapperDictionary.ContainsKey(sourceName))
            {
                var mapper = mapperDictionary[sourceName];
                this.mappers.Add(mapper);
                sourceName = mapper.DestinationName;
            }
        }

        public IIntervalSet Map(IIntervalSet intervalSet)
        {
            IIntervalSet result = intervalSet;
            foreach (var mapper in mappers)
            {
                result = mapper.Map(result);
            }
            return result;
        }
    }
}