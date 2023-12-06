namespace AdventOfCode.Tests
{
    public class TestDay6BoatRace
    {
        [Fact]
        public void TestCannotBreakRecordIfItIsOptimal()
        {
            RaceSpecification raceSpec = new(RaceTime: 6, PreviousRecord: 9);
            Assert.Equal(0, BoatRace.NumWaysToBreakRecord(raceSpec));
        }

        [Fact]
        public void TestCalculatesNumberOfWaysToBeatPreviousRecord()
        {
            RaceSpecification raceSpec = new(RaceTime: 30, PreviousRecord: 200);
            Assert.Equal(9, BoatRace.NumWaysToBreakRecord(raceSpec));
        }
    }
}