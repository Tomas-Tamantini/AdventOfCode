namespace AdventOfCode.Tests
{
    public class TestDay3GearRatios
    {
        [Fact]
        public void TestNumberByItselfIsNotPartNumber()
        {
            var lines = new List<string> { "123" };
            var gearRatios = new GearRatios(lines);
            Assert.Empty(gearRatios.PartNumbers());
        }

        [Fact]
        public void TestNumberNotAdjacentToSymbolIsNotPartNumber()
        {
            var lines = new List<string> { "1...", "..2.", "..3." };
            var gearRatios = new GearRatios(lines);
            Assert.Empty(gearRatios.PartNumbers());
        }

        [Fact]
        public void TestNumberAdjacentToSymbolIsPartNumber()
        {
            var lines = new List<string> {
                "467..114..",
                "...*......",
                "..35..633.",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598.."
            };
            var gearRatios = new GearRatios(lines);
            Assert.Equal(new List<int> { 467, 35, 633, 617, 592, 755, 664, 598 }, gearRatios.PartNumbers());
        }

        [Fact]
        public void TestGearsAreAsteriscsAdjacentToTwoPartNumbers()
        {
            var lines = new List<string> {
                "467..114..",
                "...*......",
                "..35..633.",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598.."
            };
            var gearRatios = new GearRatios(lines);
            Assert.Equal(new List<(int, int)> { (467, 35), (755, 598) }, gearRatios.Gears());
        }
    }
}