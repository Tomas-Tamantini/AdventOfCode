using System.Data.Common;

namespace AdventOfCode.Console.Models
{
    public class RatingInequality
    {
        private readonly string attribute;
        private readonly bool isLessThan;
        private readonly int threshold;
        private readonly string nextRuleId;

        public RatingInequality(string attribute, bool isLessThan, int threshold, string nextRuleId)
        {
            this.isLessThan = isLessThan;
            this.threshold = threshold;
            this.attribute = attribute;
            this.nextRuleId = nextRuleId;
        }

        public bool IsTrueFor(MachinePartRating rating)
        {
            int ratingValue = (int)rating.GetType().GetProperty(attribute).GetValue(rating);
            return isLessThan ? ratingValue < threshold : ratingValue > threshold;
        }

        public string NextRuleId => nextRuleId;
    }

    public class MachinePartRule
    {
        private string id;
        private readonly RatingInequality[] inequalities;
        private readonly string defaultNextRuleId;

        public MachinePartRule(string id, RatingInequality[] inequalities, string defaultNextRule)
        {
            this.id = id;
            this.inequalities = inequalities;
            this.defaultNextRuleId = defaultNextRule;
        }

        public string Id => id;

        public string Invoke(MachinePartRating rating)
        {
            foreach (RatingInequality inequality in inequalities)
            {
                if (inequality.IsTrueFor(rating))
                {
                    return inequality.NextRuleId;
                }
            }
            return defaultNextRuleId;
        }
    }

    enum TerminalStates
    {
        Accepted = 'A',
        Rejected = 'R',
    };

    public record MachinePartRating(int X, int M, int A, int S)
    {
        public int TotalRating => X + M + A + S;
    }
    public class Aplenty
    {
        private readonly Dictionary<string, MachinePartRule> rules;

        public Aplenty(IEnumerable<MachinePartRule> rules)
        {
            this.rules = rules.ToDictionary(rule => rule.Id);
        }

        public bool MachinePartIsAccepted(MachinePartRating rating, string initialRule = "in")
        {
            MachinePartRule currentRule = rules[initialRule];
            while (currentRule != null)
            {
                string nextRule = currentRule.Invoke(rating);
                if (nextRule == ((char)TerminalStates.Accepted).ToString())
                {
                    return true;
                }
                if (nextRule == ((char)TerminalStates.Rejected).ToString())
                {
                    return false;
                }
                currentRule = rules[nextRule];
            }
            return false;
        }
    }
}