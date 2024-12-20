using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public class Card
    {
        public string Rank;
        public bool IsFaceUp;

        public Card(string rank, bool isFaceUp = false)
        {
            Rank = rank;
            IsFaceUp = isFaceUp;
        }
    }

    private List<Card> deck = new List<Card>();
    private List<Card>[] playersHands = new List<Card>[4];

    public GameObject cardPrefab; // Prefab for card UI
    public Transform[] playerHandsUI; // Containers for each player's cards
    public Transform CommonBoard; // Containers for each player's cards


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
        deck.Add(new Card("Joker"));
        deck.Add(new Card("Joker"));
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
            { "Joker", jokerSprite },
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

                if (cardText != null)
                {
                    cardText.text = isPlayerOne ? card.Rank : ""; // Show rank for player 1, hide for others
                }

                if (cardImage != null)
                {
                    if (isPlayerOne && cardLibrary.ContainsKey(card.Rank))
                    {
                        cardImage.sprite = cardLibrary[card.Rank]; // Set the correct face-up image
                    }
                    else
                    {
                        cardImage.sprite = backSprite; // Or use your back card image
                    }
                }

                // Attach the card data to the clickable script
                CardClickable cardClickable = cardObject.GetComponent<CardClickable>();
                if (cardClickable != null)
                {
                    cardClickable.card = card;
                }
            }
        }
    }


    public void RemoveCardsFromPlayer(int playerIndex, List<Card> card)
    {
        foreach(Card c in card)
        {
            playersHands[playerIndex].Remove(c);
            // destroy them from UI

            // Find the corresponding card object in the UI and move it to the common board
            foreach (Transform cardTransform in playerHandsUI[playerIndex])
            {
                CardClickable cardClickable = cardTransform.GetComponent<CardClickable>();
                if (cardClickable != null && cardClickable.card == c)
                {
                    // make it face down
                    Image cardImage = cardTransform.GetComponentInChildren<Image>();
                    if (cardImage != null)
                    {
                        cardImage.sprite = backSprite;
                    }
                    cardTransform.SetParent(CommonBoard);
                    break;
                }
            }

        }

    }

    internal void revealCard(int numberToReveal)
    {
        // reveal the n last cards of the common board
        for (int i = 0; i < numberToReveal; i++)
        {
            Transform cardTransform = CommonBoard.GetChild(CommonBoard.childCount - 1 - i);
            Image cardImage = cardTransform.GetComponentInChildren<Image>();
            cardImage.sprite = cardLibrary[cardTransform.GetComponent<CardClickable>().card.Rank];
        }
    }
}
