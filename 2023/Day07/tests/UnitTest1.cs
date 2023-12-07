using System.Linq;
namespace tests;

public class UnitTest1
{
    [Fact]
    public void FiveOfAKindTest()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[0]);
        hand.Cards.Add(cards[0]);
        hand.Cards.Add(cards[0]);
        hand.Cards.Add(cards[0]);
        hand.Cards.Add(cards[0]);
        var handType = new HandType();
        var type = HandType.CheckFiveOfAKind(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void FiveOfAKindTest2()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[5]);
        hand.Cards.Add(cards[5]);
        hand.Cards.Add(cards[5]);
        hand.Cards.Add(cards[5]);
        hand.Cards.Add(cards[5]);
        var handType = new HandType();
        var type = HandType.CheckFiveOfAKind(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void FourOfAKindTest1()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[0]);
        hand.Cards.Add(cards[0]);
        hand.Cards.Add(cards[0]);
        hand.Cards.Add(cards[0]);
        hand.Cards.Add(cards[1]);
        var handType = new HandType();
        var type = HandType.CheckFourOfAKind(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void FourOfAKindTest2()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[6]);
        hand.Cards.Add(cards[10]);
        hand.Cards.Add(cards[6]);
        hand.Cards.Add(cards[6]);
        hand.Cards.Add(cards[6]);
        var handType = new HandType();
        var type = HandType.CheckFourOfAKind(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void FullHouseTest1()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[6]);
        hand.Cards.Add(cards[6]);
        hand.Cards.Add(cards[6]);
        hand.Cards.Add(cards[10]);
        hand.Cards.Add(cards[10]);
        var handType = new HandType();
        var type = HandType.CheckFullHouse(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void FullHouseTest2()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[11]);
        hand.Cards.Add(cards[11]);
        hand.Cards.Add(cards[2]);
        hand.Cards.Add(cards[2]);
        hand.Cards.Add(cards[2]);
        var handType = new HandType();
        var type = HandType.CheckFullHouse(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void ThreeOfAKindTest1()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[8]);
        hand.Cards.Add(cards[9]);
        hand.Cards.Add(cards[11]);
        hand.Cards.Add(cards[11]);
        hand.Cards.Add(cards[11]);
        var handType = new HandType();
        var type = HandType.CheckThreeOfAKind(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void ThreeOfAKindTest2()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[8]);
        hand.Cards.Add(cards[4]);
        hand.Cards.Add(cards[8]);
        hand.Cards.Add(cards[5]);
        hand.Cards.Add(cards[8]);
        var handType = new HandType();
        var type = HandType.CheckThreeOfAKind(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void TwoPairTest1()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[8]);
        hand.Cards.Add(cards[4]);
        hand.Cards.Add(cards[8]);
        hand.Cards.Add(cards[4]);
        hand.Cards.Add(cards[5]);
        var handType = new HandType();
        var type = HandType.CheckTwoPair(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void OnePair()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[8]);
        hand.Cards.Add(cards[4]);
        hand.Cards.Add(cards[8]);
        hand.Cards.Add(cards[6]);
        hand.Cards.Add(cards[5]);
        var handType = new HandType();
        var type = HandType.CheckOnePair(hand.Cards);
        Assert.True(type);
    }

    [Fact]
    public void HighCardTest1()
    {
        var cards = Card.SeedCards();
        var hand = new Hand();
        hand.Cards.Add(cards[2]);
        hand.Cards.Add(cards[3]);
        hand.Cards.Add(cards[4]);
        hand.Cards.Add(cards[5]);
        hand.Cards.Add(cards[6]);
        var handType = new HandType();
        var type = HandType.GetHandType(hand);
        Assert.Equal(HandType.HandTypeNameEnum.HighCard, type);
    }
}