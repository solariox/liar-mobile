using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    //public DeckManager.Card selectedCard;
    //private List<DeckManager.Card> playerHand;

    //public GameObject cardPrefab; // Prefab for card UI
    //public Transform playerHandUI; // Player's hand UI container

    //void Start()
    //{
    //    // Example hand setup, you can get this from your DeckManager if it's set up elsewhere
    //    playerHand = new List<DeckManager.Card>();
    //    DisplayCards();
    //}

    //// Display cards in UI, only for the active player
    //void DisplayCards()
    //{
    //    foreach (DeckManager.Card card in playerHand)
    //    {
    //        GameObject cardObject = Instantiate(cardPrefab, playerHandUI);
    //        Text cardText = cardObject.GetComponentInChildren<Text>();

    //        if (cardText != null)
    //        {
    //            cardText.text = card.Rank;
    //        }

    //        Button cardButton = cardObject.GetComponentInChildren<Button>();
    //        cardButton.onClick.AddListener(() => SelectCard(card));  // Add click listener to select card
    //    }
    //}

    //// When a card is selected
    //public void SelectCard(DeckManager.Card card)
    //{
    //    if (selectedCard != null) return; // Avoid selecting another card if one is already selected

    //    selectedCard = card;
    //    Debug.Log($"Card selected: {selectedCard.Rank}");

    //    // Change visual state to show selection (optional)
    //    // You can modify this to make it visually more distinct
    //}

    //// When the player confirms their move (e.g., by pressing a button)
    //public void PlaySelectedCard()
    //{
    //    if (selectedCard != null)
    //    {
    //        Debug.Log($"Card played: {selectedCard.Rank}");
    //        playerHand.Remove(selectedCard);  // Remove the played card from the player's hand

    //        // Here you can send the played card to a central game controller or update the game state

    //        selectedCard = null; // Deselect card
    //        UpdateUI();
    //    }
    //}

    //void UpdateUI()
    //{
    //    // Optionally, you can update the UI after a card is played (like refreshing the player's hand)
    //    // This can include updating the hand display or showing the remaining cards
    //}
}
