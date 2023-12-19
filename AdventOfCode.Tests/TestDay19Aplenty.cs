using AdventOfCode.Console.IO;
using Moq;

namespace AdventOfCode.Tests
{
    public class TestDay19Aplenty
    {
        [Fact]
        public void TestMachinePartCanBeAcceptedOrRejectedDependingOnRule()
        {
            RatingInequality ratingInequality = new(attribute: "X", isLessThan: true, threshold: 1, nextRuleId: "A");
            MachinePartRule rule = new(id: "in", inequalities: new[] { ratingInequality }, defaultNextRule: "R");
            var aplenty = new Aplenty(new List<MachinePartRule>() { rule }, initialRule: "in");
            MachinePartRating acceptRating = new(0, 0, 0, 0);
            MachinePartRating rejectRating = new(1, 0, 0, 0);
            Assert.True(aplenty.MachinePartIsAccepted(acceptRating));
            Assert.False(aplenty.MachinePartIsAccepted(rejectRating));
        }

        [Fact]
        public void TestRuleAreChainApplied()
        {
            RatingInequality ratingInequality = new(attribute: "X", isLessThan: false, threshold: 0, nextRuleId: "rule accept");
            MachinePartRule ruleInit = new(id: "init", inequalities: new[] { ratingInequality }, defaultNextRule: "rule reject");
            MachinePartRule ruleAccept = new(id: "rule accept", inequalities: Array.Empty<RatingInequality>(), defaultNextRule: "A");
            MachinePartRule ruleReject = new(id: "rule reject", inequalities: Array.Empty<RatingInequality>(), defaultNextRule: "R");

            Aplenty aplenty = new(new List<MachinePartRule>() { ruleInit, ruleAccept, ruleReject }, initialRule: "init");

            MachinePartRating acceptRating = new(1, 0, 0, 0);
            MachinePartRating rejectRating = new(0, 0, 0, 0);
            Assert.True(aplenty.MachinePartIsAccepted(acceptRating));
            Assert.False(aplenty.MachinePartIsAccepted(rejectRating));
        }

        [Fact]
        public void TestCanCountTotalNumberOfAcceptedStates()
        {
            RatingInequality ratingInequality = new(attribute: "X", isLessThan: true, threshold: 5, nextRuleId: "A");
            MachinePartRule rule = new(id: "in", inequalities: new[] { ratingInequality }, defaultNextRule: "R");
            var aplenty = new Aplenty(new List<MachinePartRule>() { rule }, initialRule: "in");
            RatingRange xRange = new(1, 10);
            RatingRange mRange = new(11, 13);
            RatingRange aRange = new(14, 15);
            RatingRange sRange = new(16, 20);
            RatingsRange attributeRanges = new(xRange, mRange, aRange, sRange);
            Assert.Equal(120, aplenty.NumAcceptedStates(attributeRanges));
        }

        [Fact]
        public void TestNumberOfAcceptedStatesRunsEfficiently()
        {
            string fileContent = @"px{a<2006:qkq,m>2090:A,rfg}
                                   pv{a>1716:R,A}
                                   lnx{m>1548:A,A}
                                   rfg{s<537:gd,x>2440:R,A}
                                   qs{s>3448:A,lnx}
                                   qkq{x<1416:A,crn}
                                   crn{x>2662:A,R}
                                   in{s<1351:px,qqz}
                                   qqz{s>2770:qs,m<1801:hdj,R}
                                   gd{a>3333:R,R}
                                   hdj{m>838:A,pv}";

            var fileReaderMock = new Mock<IFileReader>();
            fileReaderMock.Setup(fr => fr.ReadAllLines("MachinePartRulesInput.txt")).Returns(fileContent.Split('\n'));
            var parser = new TextParser(fileReaderMock.Object);
            (Aplenty aplenty, IEnumerable<MachinePartRating> _) = parser.ParseAplenty("MachinePartRulesInput.txt", initialRule: "in");
            RatingRange attributeRange = new(1, 4000);
            RatingsRange attributeRanges = new(attributeRange, attributeRange, attributeRange, attributeRange);
            Assert.Equal(167409079868000, aplenty.NumAcceptedStates(attributeRanges));
        }
    }
}