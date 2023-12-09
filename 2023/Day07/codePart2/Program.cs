using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

public class Program
{
    public static async Task Main()
    {
        var cards = Card.SeedCards();
        var hands = new List<Hand>();

        var input = await File.ReadAllLinesAsync("Input1.txt");
        foreach (var line in input)
        {
            var lineArray = line.Split(" ");
            var hand = new Hand();
            foreach (var cardChar in lineArray[0])
            {
                hand.Cards.Add(cards.Single(_ => _.Label == cardChar.ToString()));
                hand.OriginalCards.Add(cards.Single(_ => _.Label == cardChar.ToString()));
            }
            hand.Bid = long.Parse(lineArray[1]);
            hand.HandTypeName = HandType.GetHandType(hand);

            var handsSet = Hand.FindBestHand(hand, null);
            foreach (var handSet in handsSet)
            {
                handSet.HandTypeName = HandType.GetHandType(handSet);
                //Console.WriteLine($"{string.Join("", handSet.Cards.Select(_ => _.Label))}:{handSet.HandTypeName}");
            }
            hand = handsSet.OrderByDescending(_ => (int)_.HandTypeName).First();

            hands.Add(hand);
        }

        hands = hands
            .OrderBy(_ => _.HandTypeName)
            .ThenBy(_ => _.GetCardValueString())
            .ToList();

        long result = 0;
        for (long i = 0; i < hands.Count; i++)
        {
            Console.WriteLine($"{string.Join("", hands[(int)i].Cards.Select(_ => _.Label))} {hands[(int)i].Bid} * {i + 1} -> OriginalCards:{string.Join("", hands[(int)i].OriginalCards.Select(_ => _.Label))}");
            result += hands[(int)i].Bid * (i + 1);
        }

        Console.WriteLine(result);
    }
}

public class HandType
{
    public enum HandTypeNameEnum
    {
        FiveOfAKind = 7,
        FourOfAKind = 6,
        FullHouse = 5,
        ThreeOfAKind = 4,
        TwoPair = 3,
        OnePair = 2,
        HighCard = 1,
    }

    public static HandTypeNameEnum GetHandType(Hand hand)
    {
        if (CheckFiveOfAKind(hand.Cards))
            return HandTypeNameEnum.FiveOfAKind;
        if (CheckFourOfAKind(hand.Cards))
            return HandTypeNameEnum.FourOfAKind;
        if (CheckFullHouse(hand.Cards))
            return HandTypeNameEnum.FullHouse;
        if (CheckThreeOfAKind(hand.Cards))
            return HandTypeNameEnum.ThreeOfAKind;
        if (CheckTwoPair(hand.Cards))
            return HandTypeNameEnum.TwoPair;
        if (CheckOnePair(hand.Cards))
            return HandTypeNameEnum.OnePair;
        return HandTypeNameEnum.HighCard;
    }

    public static bool CheckFiveOfAKind(List<Card> cards)
    {
        if (cards.Count(_ => _.Label == cards.First().Label) == 5)
            return true;
        return false;
    }
    public static bool CheckFourOfAKind(List<Card> cards)
    {
        var groupedCards = cards.Select(_ => _.Label).GroupBy(_ => _);
        if (groupedCards.Where(_ => _.Count() == 4).Count() == 1
            && groupedCards.Where(_ => _.Count() == 1).Count() == 1)
            return true;
        return false;
    }
    public static bool CheckFullHouse(List<Card> cards)
    {
        var groupedCards = cards.Select(_ => _.Label).GroupBy(_ => _);
        if (groupedCards.Where(_ => _.Count() == 3).Count() == 1
            && groupedCards.Where(_ => _.Count() == 2).Count() == 1)
            return true;
        return false;
    }
    public static bool CheckThreeOfAKind(List<Card> cards)
    {
        var groupedCards = cards.Select(_ => _.Label).GroupBy(_ => _);
        if (groupedCards.Where(_ => _.Count() == 3).Count() == 1
            && groupedCards.Where(_ => _.Count() == 1).Count() == 2)
            return true;
        return false;
    }
    public static bool CheckTwoPair(List<Card> cards)
    {
        var groupedCards = cards.Select(_ => _.Label).GroupBy(_ => _);
        if (groupedCards.Where(_ => _.Count() == 2).Count() == 2
            && groupedCards.Where(_ => _.Count() == 1).Count() == 1)
            return true;
        return false;
    }
    public static bool CheckOnePair(List<Card> cards)
    {
        var groupedCards = cards.Select(_ => _.Label).GroupBy(_ => _);
        if (groupedCards.Where(_ => _.Count() == 2).Count() == 1
            && groupedCards.Where(_ => _.Count() == 1).Count() == 3)
            return true;
        return false;
    }
}

public class Hand
{
    public List<Card> OriginalCards {get; set;} = new List<Card>();
    public List<Card> Cards { get; set; } = new List<Card>();
    public long Bid { get; set; }
    public HandType.HandTypeNameEnum HandTypeName { get; set; }

    public string GetCardValueString()
    {
        return string.Join("", OriginalCards.Select(_ => _.OrderValue));
    }

    public Hand Clone()
    {
        var newHand = new Hand();
        newHand.Bid = Bid;
        foreach (var card in Cards)
        {
            newHand.Cards.Add(new Card(card.Label));
        }
        foreach (var originalCard in OriginalCards)
        {
            newHand.OriginalCards.Add(originalCard);
        }
        return newHand;
    }

    // public static List<Hand> FindBestHand(Hand hand, int? index, List<Hand> newHands)
    // {
    //     var cardsWithoutJoker = Card.SeedCards().Where(_ => _.Label != "J");
    //     if (index == null)
    //     {
    //         newHands = new List<Hand>();
    //         newHands.Add(hand);
    //         index = 0;
    //     }
    //     for (int i = index.Value; i < hand.Cards.Count; i++)
    //     {
    //         if (hand.Cards[i].Label == "J")
    //         {
    //             foreach (var cardWithoutJoker in cardsWithoutJoker)
    //             {
    //                 var newHand = hand.Clone();
    //                 newHand.Cards[i] = cardWithoutJoker;
    //                 newHands.AddRange(FindBestHand(nweHand, index+1, newHands));
    //             }
    //             break;
    //         }
    //     }
        
    //     return newHands;
    // }

    public static List<Hand> FindBestHand(Hand hand, int? index)
    {
        List<Hand> newHands = new List<Hand>();
        var allCards = Card.SeedCards();
        var first = false;
        if (index == null)
        {
            first = true;
            index = 0;
        }
        int i = index.Value;
        for (; i < hand.Cards.Count; i++)
        {
            if (hand.Cards[i].Label == "J")
            {
                foreach (var card in allCards)
                {
                    var newHand = hand.Clone();
                    newHand.Cards[i] = card;
                    var beforeCount = newHands.Count;
                    newHands.AddRange(FindBestHand(newHand, i + 1));
                }
                break;
            }
        }
        if (i == hand.Cards.Count)
        {
            return new List<Hand> {hand};
        }
        return newHands;
    }
}

public class Card
{
    public string Label { get; set; }
    public Card(string label)
    {
        Label = label;
    }

    public string OrderValue
    {
        get
        {
            switch (Label)
            {
                case "J":
                    return "A";
                case "2":
                    return "B";
                case "3":
                    return "C";
                case "4":
                    return "D";
                case "5":
                    return "E";
                case "6":
                    return "F";
                case "7":
                    return "G";
                case "8":
                    return "H";
                case "9":
                    return "I";
                case "T":
                    return "J";
                case "Q":
                    return "K";
                case "K":
                    return "L";
                case "A":
                    return "M";
            }
            throw new NotImplementedException();
        }
    }

    public static List<Card> SeedCards()
    {
        return new List<Card>()
        {
            new Card("2"),
            new Card("3"),
            new Card("4"),
            new Card("5"),
            new Card("6"),
            new Card("7"),
            new Card("8"),
            new Card("9"),
            new Card("T"),
            new Card("J"),
            new Card("Q"),
            new Card("K"),
            new Card("A"),
        };
    }
}