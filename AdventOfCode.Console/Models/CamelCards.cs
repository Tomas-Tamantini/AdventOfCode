namespace AdventOfCode.Console.Models
{
    public enum HandType
    {
        HighCard = 0,
        OnePair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6
    }

    public class CamelHand : IComparable<CamelHand>
    {
        public HandType HandType { get; private set; }
        private readonly int[] cardValues;

        public CamelHand(string hand)
        {
            Dictionary<char, int> cardMultiplicity = hand.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            var highestCount = cardMultiplicity.Values.Max();
            HandType = highestCount switch
            {
                2 => cardMultiplicity.Count == 3 ? HandType.TwoPair : HandType.OnePair,
                3 => cardMultiplicity.Count == 2 ? HandType.FullHouse : HandType.ThreeOfAKind,
                4 => HandType.FourOfAKind,
                5 => HandType.FiveOfAKind,
                _ => HandType.HighCard,
            };

            cardValues = hand.Select(c => c switch
            {
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => int.Parse(c.ToString())
            }).ToArray();
        }

        public int CompareTo(CamelHand other)
        {
            if (HandType != other.HandType)
            {
                return HandType.CompareTo(other.HandType);
            }
            return cardValues.Zip(other.cardValues, (left, right) => left.CompareTo(right)).FirstOrDefault(c => c != 0);
        }

        public static bool operator <(CamelHand left, CamelHand right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(CamelHand left, CamelHand right)
        {
            return left.CompareTo(right) > 0;
        }
    }

    public record CamelBid(CamelHand Hand, int Bid);

    public class CamelCards
    {
        private readonly List<CamelBid> bids;

        public CamelCards(List<CamelBid> bids)
        {
            this.bids = bids;
        }

        public int[] SortedBidValues()
        {
            return bids.OrderBy(b => b.Hand).Select(b => b.Bid).ToArray();
        }

        public int TotalWinnings()
        {
            var sortedBids = SortedBidValues();
            return sortedBids.Select((bid, index) => bid * (index + 1)).Sum();
        }
    }
}