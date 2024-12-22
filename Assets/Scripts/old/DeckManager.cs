using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class DeckManager : MonoBehaviour
{

    private List<Card> deck = new List<Card>();
    private List<Card>[] playersHands = new List<Card>[4];

    public GameObject cardPrefab; // Prefab for card UI


    public Sprite aceSprite;
    public Sprite kingSprite;
    public Sprite queenSprite;
    public Sprite jokerSprite;
    public Sprite backSprite;


    //void Start()
    //{
    //    InitializeCardLibrary();
    //    //DisplayCardsInUI();
    //}


    //void DisplayCardsInUI()
    //{

    //    // Display all players' cards, but show ranks and images for player 1, face down for others
    //    for (int player = 0; player < 4; player++)
    //    {
    //        bool isPlayerOne = player == 0;
    //        foreach (Card card in playersHands[player])
    //        {
    //            GameObject cardObject = Instantiate(cardPrefab, playerHandsUI[player]);

    //            // Set card text and image
    //            Text cardText = cardObject.GetComponentInChildren<Text>();
    //            Image cardImage = cardObject.GetComponentInChildren<Image>();

    //            if (cardText != null)
    //            {
    //                cardText.text = isPlayerOne ? card.Rank : ""; // Show rank for player 1, hide for others
    //            }

    //            if (cardImage != null)
    //            {
    //                if (isPlayerOne && cardLibrary.ContainsKey(card.Rank))
    //                {
    //                    cardImage.sprite = cardLibrary[card.Rank]; // Set the correct face-up image
    //                }
    //                else
    //                {
    //                    cardImage.sprite = backSprite; // Or use your back card image
    //                }
    //            }

    //            // Attach the card data to the clickable script
    //            CardClickable cardClickable = cardObject.GetComponent<CardClickable>();
    //            if (cardClickable != null)
    //            {
    //                cardClickable.card = card;
    //            }
    //        }
    //    }
    //}


    //public void RemoveCardsFromPlayer(int playerIndex, List<Card> cards)
    //{
    //    foreach (Card card in cards)
    //    {
    //        playersHands[playerIndex].Remove(card);
    //        // destroy them from UI

    //        // Find the corresponding card object in the UI and move it to the common board
    //        foreach (Transform cardTransform in playerHandsUI[playerIndex])
    //        {
    //            CardClickable cardClickable = cardTransform.GetComponent<CardClickable>();
    //            if (cardClickable != null && cardClickable.card == card)
    //            {
    //                // make it face down
    //                Image cardImage = cardTransform.GetComponentInChildren<Image>();
    //                if (cardImage != null)
    //                {
    //                    cardImage.sprite = backSprite;
    //                }
    //                cardTransform.SetParent(CommonBoard);
    //                break;
    //            }
    //        }

    //    }
    //}

    //internal void revealCard(int numberToReveal)
    //{
    //    // reveal the n last cards of the common board
    //    for (int i = 0; i < numberToReveal; i++)
    //    {
    //        Transform cardTransform = CommonBoard.GetChild(CommonBoard.childCount - 1 - i);
    //        Image cardImage = cardTransform.GetComponentInChildren<Image>();
    //        cardImage.sprite = cardLibrary[cardTransform.GetComponent<CardClickable>().card.Rank];
    //    }
    //}
}
