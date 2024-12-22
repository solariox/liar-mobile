using NUnit.Framework;
using System.Collections.Generic;

public class Player
{
    public string Name;
    public int Chances;
    public List<Card> Cards;
    public List<Card> LastPlayedCards;

    public Player(string name, int chances, List<Card> cards, List<Card> lastPlayedCards)
    {
        this.Name = name;
        this.Chances = chances;
        this.Cards = cards;
        this.LastPlayedCards = lastPlayedCards;
    }


    public void AddCard(Card card)
    {
        Cards.Add(card);
        card.SetOwner(this);
    }
}