namespace AdventOfCode.Console.Models
{
    public record RaceSpecification(int RaceTime, int PreviousRecord);
    public class BoatRace
    {
        public static int NumWaysToBreakRecord(RaceSpecification raceSpecification)
        {
            var raceTime = raceSpecification.RaceTime;

            var delta = raceTime * raceTime - 4 * raceSpecification.PreviousRecord;
            if (delta < 0) return 0;
            var sqrt = Math.Sqrt(delta);
            var lowerBound = (raceTime - sqrt) / 2;
            var upperBound = (raceTime + sqrt) / 2;

            var intLowerBound = (int)Math.Floor(lowerBound) + 1;
            var intUpperBound = (int)Math.Ceiling(upperBound) - 1;



            return Math.Max(intUpperBound - intLowerBound + 1, 0);

        }
    }
}