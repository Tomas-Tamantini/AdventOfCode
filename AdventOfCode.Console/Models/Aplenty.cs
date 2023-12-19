namespace AdventOfCode.Console.Models
{
    public record RatingRange(int InclusiveLowerBound, int InclusiveUpperBound)
    {
        public long NumStates => Math.Max(InclusiveUpperBound - InclusiveLowerBound + 1, 0);
    };
    public record RatingsRange(RatingRange X, RatingRange M, RatingRange A, RatingRange S)
    {
        public long NumStates => X.NumStates * M.NumStates * A.NumStates * S.NumStates;
    }
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

        public (RatingsRange, RatingsRange) SliceRange(RatingsRange range)
        {
            Dictionary<string, RatingRange> attributeRanges = new()
            {
                { "X", range.X },
                { "M", range.M },
                { "A", range.A },
                { "S", range.S },
            };
            RatingRange previousRange = attributeRanges[attribute];
            RatingRange newRange, leftoverRange;
            if (isLessThan)
            {
                newRange = new RatingRange(previousRange.InclusiveLowerBound, Math.Min(previousRange.InclusiveUpperBound, threshold - 1));
                leftoverRange = new RatingRange(Math.Max(previousRange.InclusiveLowerBound, threshold), previousRange.InclusiveUpperBound);
            }
            else
            {
                newRange = new RatingRange(Math.Max(previousRange.InclusiveLowerBound, threshold + 1), previousRange.InclusiveUpperBound);
                leftoverRange = new RatingRange(previousRange.InclusiveLowerBound, Math.Min(previousRange.InclusiveUpperBound, threshold));
            }

            Dictionary<string, RatingRange> newRanges = new();
            Dictionary<string, RatingRange> leftoverRanges = new();

            foreach (string key in attributeRanges.Keys)
            {
                if (key == attribute)
                {
                    newRanges[key] = newRange;
                    leftoverRanges[key] = leftoverRange;
                }
                else
                {
                    newRanges[key] = attributeRanges[key];
                    leftoverRanges[key] = attributeRanges[key];
                }
            }

            return (new RatingsRange(newRanges["X"], newRanges["M"], newRanges["A"], newRanges["S"]),
                    new RatingsRange(leftoverRanges["X"], leftoverRanges["M"], leftoverRanges["A"], leftoverRanges["S"]));
        }
    }

    public class MachinePartRule
    {
        private readonly string id;
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

        public IEnumerable<(string, RatingsRange)> MapRange(RatingsRange currentRange)
        {
            RatingsRange leftoverRange = currentRange;
            foreach (RatingInequality inequality in inequalities)
            {
                (RatingsRange range, RatingsRange leftover) = inequality.SliceRange(leftoverRange);
                if (range.NumStates != 0) yield return (inequality.NextRuleId, range);
                if (leftover.NumStates == 0) yield break;
                leftoverRange = leftover;
            }
            yield return (defaultNextRuleId, leftoverRange);
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
        private readonly string initialRule;

        public Aplenty(IEnumerable<MachinePartRule> rules, string initialRule = "in")
        {
            this.rules = rules.ToDictionary(rule => rule.Id);
            this.initialRule = initialRule;
        }

        public bool MachinePartIsAccepted(MachinePartRating rating)
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

        private IEnumerable<RatingsRange> GetAcceptedRanges(RatingsRange attributeRanges)
        {
            IEnumerable<RatingsRange> acceptedRanges = new List<RatingsRange>() { attributeRanges };
            List<(string, RatingsRange)> rulesStack = new() { (initialRule, attributeRanges) };
            while (rulesStack.Count > 0)
            {
                (string currentRuleId, RatingsRange currentRange) = rulesStack[^1];
                rulesStack.RemoveAt(rulesStack.Count - 1);
                MachinePartRule currentRule = rules[currentRuleId];
                foreach ((string nextRuleId, RatingsRange mappedRange) in currentRule.MapRange(currentRange))
                {
                    if (nextRuleId == ((char)TerminalStates.Accepted).ToString())
                    {
                        yield return mappedRange;
                    }
                    else if (nextRuleId != ((char)TerminalStates.Rejected).ToString())
                    {
                        rulesStack.Add((nextRuleId, mappedRange));
                    }
                }
            }
        }

        public long NumAcceptedStates(RatingsRange attributeRanges)
        {
            IEnumerable<RatingsRange> acceptedRanges = GetAcceptedRanges(attributeRanges);
            return acceptedRanges.Sum(range => range.NumStates);

        }
    }
}