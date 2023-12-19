namespace AdventOfCode.Tests
{
    public class TestDay19Aplenty
    {
        [Fact]
        public void TestMachinePartCanBeAcceptedOrRejectedDependingOnRule()
        {
            RatingInequality ratingInequality = new(attribute: "X", isLessThan: true, threshold: 1, nextRuleId: "A");
            MachinePartRule rule = new(id: "in", inequalities: new[] { ratingInequality }, defaultNextRule: "R");
            var aplenty = new Aplenty(new List<MachinePartRule>() { rule });
            MachinePartRating acceptRating = new(0, 0, 0, 0);
            MachinePartRating rejectRating = new(1, 0, 0, 0);
            Assert.True(aplenty.MachinePartIsAccepted(acceptRating, initialRule: "in"));
            Assert.False(aplenty.MachinePartIsAccepted(rejectRating, initialRule: "in"));
        }

        [Fact]
        public void TestRuleAreChainApplied()
        {
            RatingInequality ratingInequality = new(attribute: "X", isLessThan: false, threshold: 0, nextRuleId: "rule accept");
            MachinePartRule ruleInit = new(id: "init", inequalities: new[] { ratingInequality }, defaultNextRule: "rule reject");
            MachinePartRule ruleAccept = new(id: "rule accept", inequalities: Array.Empty<RatingInequality>(), defaultNextRule: "A");
            MachinePartRule ruleReject = new(id: "rule reject", inequalities: Array.Empty<RatingInequality>(), defaultNextRule: "R");

            Aplenty aplenty = new(new List<MachinePartRule>() { ruleInit, ruleAccept, ruleReject });

            MachinePartRating acceptRating = new(1, 0, 0, 0);
            MachinePartRating rejectRating = new(0, 0, 0, 0);
            Assert.True(aplenty.MachinePartIsAccepted(acceptRating, initialRule: "init"));
            Assert.False(aplenty.MachinePartIsAccepted(rejectRating, initialRule: "init"));
        }
    }
}