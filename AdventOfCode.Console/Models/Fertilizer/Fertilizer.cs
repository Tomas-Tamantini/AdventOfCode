namespace AdventOfCode.Console.Models
{
    public class Fertilizer
    {
        private readonly List<long> seeds;
        private readonly IIntervalMapper intervalMapper;

        public Fertilizer(List<long> seeds, IIntervalMapper intervalMapper)
        {
            this.seeds = seeds;
            this.intervalMapper = intervalMapper;
        }

        public long LowestOutputWithStandaloneSeeds()
        {
            List<Interval> intervals = seeds.Select(x => new Interval { Start = x, End = x }).ToList();
            IIntervalSet intervalSet = new IntervalSet(intervals);
            return intervalMapper.Map(intervalSet).LowestNumber();
        }

        public long LowestOutputWithSeedsAsRanges()
        {
            List<Interval> intervals = new();
            for (int i = 0; i < seeds.Count; i += 2)
            {
                intervals.Add(new Interval { Start = seeds[i], End = seeds[i] + seeds[i + 1] - 1 });
            }
            IIntervalSet intervalSet = new IntervalSet(intervals);
            return intervalMapper.Map(intervalSet).LowestNumber();
        }
    }
}