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

    public interface IHandRanker
    {
        HandType RankHand(string hand);
        int[] CardValues(string hand);
    }

    public class DefaultCamelRanker : IHandRanker
    {
        public HandType RankHand(string hand)
        {
            Dictionary<char, int> cardMultiplicity = hand.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            var highestCount = cardMultiplicity.Values.Max();
            return highestCount switch
            {
                2 => cardMultiplicity.Count == 3 ? HandType.TwoPair : HandType.OnePair,
                3 => cardMultiplicity.Count == 2 ? HandType.FullHouse : HandType.ThreeOfAKind,
                4 => HandType.FourOfAKind,
                5 => HandType.FiveOfAKind,
                _ => HandType.HighCard,
            };
        }

        public int[] CardValues(string hand)
        {
            return hand.Select(c => c switch
            {
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => int.Parse(c.ToString())
            }).ToArray();
        }
    }

    public class JokerCamelRanker : IHandRanker
    {
        public HandType RankHand(string hand)
        {
            Dictionary<char, int> cardMultiplicity = hand.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            if (cardMultiplicity.ContainsKey('J') && cardMultiplicity.Count > 1)
            {
                var jokerCount = cardMultiplicity['J'];
                cardMultiplicity.Remove('J');
                var max = cardMultiplicity.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                cardMultiplicity[max] += jokerCount;
            }
            var highestCount = cardMultiplicity.Values.Max();
            return highestCount switch
            {
                2 => cardMultiplicity.Count == 3 ? HandType.TwoPair : HandType.OnePair,
                3 => cardMultiplicity.Count == 2 ? HandType.FullHouse : HandType.ThreeOfAKind,
                4 => HandType.FourOfAKind,
                5 => HandType.FiveOfAKind,
                _ => HandType.HighCard,
            };
        }

        public int[] CardValues(string hand)
        {
            return hand.Select(c => c switch
            {
                'T' => 10,
                'J' => 0,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => int.Parse(c.ToString())
            }).ToArray();
        }
    }

    public class CamelHand : IComparable<CamelHand>
    {
        public HandType HandType { get; init; }
        public int[] CardValues { get; init; }

        public int CompareTo(CamelHand other)
        {
            if (HandType != other.HandType)
            {
                return HandType.CompareTo(other.HandType);
            }
            return CardValues.Zip(other.CardValues, (left, right) => left.CompareTo(right)).FirstOrDefault(c => c != 0);
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

    public record CamelBid(string Hand, int Bid, CamelHand? ParsedHand = null);

    public class CamelCards
    {
        private readonly List<CamelBid> bids;

        public CamelCards(List<CamelBid> bids, IHandRanker ranker)
        {
            this.bids = bids.Select(b => new CamelBid(b.Hand, b.Bid, ParseCamelHand(b.Hand, ranker))).ToList();
        }

        private static CamelHand ParseCamelHand(string hand, IHandRanker handRanker)
        {
            var handType = handRanker.RankHand(hand);
            var cardValues = handRanker.CardValues(hand);
            return new CamelHand { HandType = handType, CardValues = cardValues };
        }

        public int[] SortedBidValues()
        {
            return bids.OrderBy(b => b.ParsedHand).Select(b => b.Bid).ToArray();
        }

        public int TotalWinnings()
        {
            var sortedBids = SortedBidValues();
            return sortedBids.Select((bid, index) => bid * (index + 1)).Sum();
        }
    }
}