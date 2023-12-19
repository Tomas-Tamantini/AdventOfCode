namespace AdventOfCode.Console.Models
{

    using MachinePartRule = Func<MachinePartRating, string>;

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

        public Aplenty(Dictionary<string, MachinePartRule> rules)
        {
            this.rules = rules;
        }

        public bool MachinePartIsAccepted(MachinePartRating rating, string initialRule = "in")
        {
            MachinePartRule currentRule = rules[initialRule];
            while (currentRule != null)
            {
                string nextRule = currentRule(rating);
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