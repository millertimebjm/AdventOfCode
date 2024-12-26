using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

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
            }
            hand.Bid = long.Parse(lineArray[1]);
            hand.HandTypeName = HandType.GetHandType(hand);
            hands.Add(hand);
        }

        hands = hands
            .OrderBy(_ => _.HandTypeName)
            .ThenBy(_ => _.GetCardValueString())
            .ToList();

        long result = 0;
        for (long i = 0; i < hands.Count; i++)
        {
            Console.WriteLine($"{string.Join("", hands[(int)i].Cards.Select(_ => _.Label))} {hands[(int)i].Bid} * {i+1}");
            result += hands[(int)i].Bid * (i+1);
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
    public List<Card> Cards {get; set;} = new List<Card>();
    public long Bid {get; set;}
    public HandType.HandTypeNameEnum HandTypeName {get; set;}

    public string GetCardValueString()
    {
        return string.Join("", Cards.Select(_ => _.Value));
    }
}

public class Card
{
    public string Label {get; set;}
    public string Value {get; set;}
    public Card(string label, string value)
    {
        Label = label;
        Value = value;
    }

    public static List<Card> SeedCards()
    {
        return new List<Card>()
        {
            new Card("2", "A"),
            new Card("3", "B"),
            new Card("4", "C"),
            new Card("5", "D"),
            new Card("6", "E"),
            new Card("7", "F"),
            new Card("8", "G"),
            new Card("9", "H"),
            new Card("T", "I"),
            new Card("J", "J"),
            new Card("Q", "K"),
            new Card("K", "L"),
            new Card("A", "M"),
        };
    }
}