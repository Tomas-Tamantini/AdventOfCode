namespace AdventOfCode.Tests
{
    public class TestDay15LensLibrary
    {
        [Fact]
        public void TestHashValueIsCalculatedProperly()
        {
            Assert.Equal(52, LensLibrary.GetHash("HASH"));
        }
    }
}