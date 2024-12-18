using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public class Card
    {
        public string Rank;
        public string Type;

        public Card(string rank, string type = "")
        {
            Rank = rank;
            Type = type;
        }
    }

    private List<Card> deck = new List<Card>();
    private List<Card>[] playersHands = new List<Card>[4];

    private List<CardClickable> selectedCards = new List<CardClickable>(); // Track selected cards

    public GameObject cardPrefab; // Prefab for card UI
    public Transform[] playerHandsUI; // Containers for each player's cards

    public Sprite aceSprite;
    public Sprite kingSprite;
    public Sprite queenSprite;
    public Sprite jokerSprite;
    public Sprite backSprite;

    private Dictionary<string, Sprite> cardLibrary;

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
        DealCards();
        InitializeCardLibrary();
        DisplayCardsInUI();
    }

    void InitializeDeck()
    {
        for (int i = 0; i < 6; i++)
        {
            deck.Add(new Card("King"));
            deck.Add(new Card("Queen"));
            deck.Add(new Card("Ace"));
        }
        deck.Add(new Card("Joker", "1"));
        deck.Add(new Card("Joker", "2"));
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = Random.Range(0, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    void DealCards()
    {
        for (int i = 0; i < 4; i++)
        {
            playersHands[i] = new List<Card>();
        }

        for (int i = 0; i < 5; i++)
        {
            for (int player = 0; player < 4; player++)
            {
                if (deck.Count > 0)
                {
                    playersHands[player].Add(deck[0]);
                    deck.RemoveAt(0);
                }
            }
        }
    }

    void InitializeCardLibrary()
    {
        cardLibrary = new Dictionary<string, Sprite>()
        {
            { "Ace", aceSprite },
            { "King", kingSprite },
            { "Queen", queenSprite },
            { "Joker1", jokerSprite },
            { "Joker2", jokerSprite }
        };
    }
    void DisplayCardsInUI()
    {

        // Display all players' cards, but show ranks and images for player 1, face down for others
        for (int player = 0; player < 4; player++)
        {
            bool isPlayerOne = player == 0;
            foreach (Card card in playersHands[player])
            {
                GameObject cardObject = Instantiate(cardPrefab, playerHandsUI[player]);

                // Set card text and image
                Text cardText = cardObject.GetComponentInChildren<Text>();
                Image cardImage = cardObject.GetComponentInChildren<Image>();
                CardClickable cardClickable = cardObject.GetComponent<CardClickable>();

                if (cardText != null)
                {
                    cardText.text = isPlayerOne ? card.Rank : ""; // Show rank for player 1, hide for others
                }

                if (cardImage != null)
                {
                    if (isPlayerOne && cardLibrary.ContainsKey(card.Rank + card.Type))
                    {
                        cardImage.sprite = cardLibrary[card.Rank + card.Type]; // Set the correct face-up image
                    }
                    else
                    {
                        cardImage.sprite = backSprite; // Or use your back card image
                    }
                }
            }
        }
    }

    public List<CardClickable> GetSelectedCards()
    {
        return selectedCards; // Return the list of selected cards
    }
}
