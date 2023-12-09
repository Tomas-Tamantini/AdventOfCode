namespace AdventOfCode.Tests
{
    public class TestDay9MirageMaintenance
    {
        [Fact]
        public void TestNextTermOfConstantSequenceIsThatConstant()
        {
            List<long> constantSequence = new() { 1, 1, 1, 1, 1 };
            Assert.Equal(1, MirageMaintenance.NextTerm(constantSequence));
        }

        [Fact]
        public void TestNextTermOfLinearSequenceMaintainsOffset()
        {
            List<long> linearSequence = new() { 10, 7, 4, 1, -2 };
            Assert.Equal(-5, MirageMaintenance.NextTerm(linearSequence));
        }

        [Fact]
        public void TestNextTermObeysSimplestPolynomialRelationship()
        {
            List<long> polynomialSequence = new() { 10, 13, 16, 21, 30, 45 };
            Assert.Equal(68, MirageMaintenance.NextTerm(polynomialSequence));
        }
    }
}