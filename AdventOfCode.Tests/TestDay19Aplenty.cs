namespace AdventOfCode.Tests
{
    public class TestDay19Aplenty
    {
        [Fact]
        public void TestMachinePartCanBeAcceptedOrRejectedDependingOnRule()
        {
            Func<MachinePartRating, string> rule = (MachinePartRating rating) =>
            {
                return (rating.X == 0) ? "A" : "R";
            };
            var rules = new Dictionary<string, Func<MachinePartRating, string>>()
            {
                { "in", rule }
            };

            var aplenty = new Aplenty(rules);
            MachinePartRating acceptRating = new(0, 0, 0, 0);
            MachinePartRating rejectRating = new(1, 0, 0, 0);
            Assert.True(aplenty.MachinePartIsAccepted(acceptRating, initialRule: "in"));
            Assert.False(aplenty.MachinePartIsAccepted(rejectRating, initialRule: "in"));
        }

        [Fact]
        public void TestRuleAreChainApplied()
        {
            Func<MachinePartRating, string> ruleInit = (MachinePartRating rating) =>
            {
                if (rating.X == 0)
                {
                    return "rule accept";
                }
                return "rule reject";
            };
            static string ruleAccept(MachinePartRating _) => "A";
            static string ruleReject(MachinePartRating _) => "R";
            var rules = new Dictionary<string, Func<MachinePartRating, string>>()
            {
                { "init", ruleInit },
                { "rule accept", ruleAccept },
                { "rule reject", ruleReject },
            };

            var aplenty = new Aplenty(rules);
            MachinePartRating acceptRating = new(0, 0, 0, 0);
            MachinePartRating rejectRating = new(1, 0, 0, 0);
            Assert.True(aplenty.MachinePartIsAccepted(acceptRating, initialRule: "init"));
            Assert.False(aplenty.MachinePartIsAccepted(rejectRating, initialRule: "init"));
        }
    }
}