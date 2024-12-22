using System.Buffers;

public class Card
{
    public string Rank;
    public bool IsFaceUp;
    public Player Owner;

    public Card(string rank, bool isFaceUp = false, Player owner = null)
    {
        Rank = rank;
        IsFaceUp = isFaceUp;
        Owner = owner;
    }

    public void SetOwner(Player player)
    {
        Owner = player;
    }
}

