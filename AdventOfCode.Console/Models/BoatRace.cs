namespace AdventOfCode.Console.Models
{
    public record RaceSpecification(long RaceTime, long PreviousRecord);
    public class BoatRace
    {
        public static long NumWaysToBreakRecord(RaceSpecification raceSpecification)
        {
            var raceTime = raceSpecification.RaceTime;

            var delta = raceTime * raceTime - 4 * raceSpecification.PreviousRecord;
            if (delta < 0) return 0;
            var sqrt = Math.Sqrt(delta);
            var lowerBound = (raceTime - sqrt) / 2;
            var upperBound = (raceTime + sqrt) / 2;

            var intLowerBound = (long)Math.Floor(lowerBound) + 1;
            var intUpperBound = (long)Math.Ceiling(upperBound) - 1;



            return Math.Max(intUpperBound - intLowerBound + 1, 0);

        }
    }
}