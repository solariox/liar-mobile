using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardClickable : MonoBehaviour, IPointerClickHandler
{
    public DeckManager.Card card; // This will be the card data associated with the UI
    public static List<DeckManager.Card> selectedCards = new List<DeckManager.Card>(); // Keeps track of selected cards

    private TurnManager turnManager; // Reference to TurnManager

    void Start()
    {
        turnManager = Object.FindFirstObjectByType<TurnManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int cardOwnerIndex = GetCardOwnerIndex();

        // Check if it's the correct player's turn
        if (turnManager.currentPlayerIndex != cardOwnerIndex)
        {
            Debug.Log("Not your turn!");
            return;
        }

        Debug.Log(selectedCards.Count);

        // Check if the card is already selected
        if (selectedCards.Contains(card))
        {
            // Deselect the card if it's already selected
            selectedCards.Remove(card);
            UpdateCardVisuals(false); // Update the visual (e.g., deselect color)
        }
        else
        {
            // If we have less than 3 cards selected, allow selecting a new card
            if (selectedCards.Count < 3)
            {
                selectedCards.Add(card);
                UpdateCardVisuals(true); // Update the visual (e.g., select color)
            }
        }
    }

    private int GetCardOwnerIndex()
    {
        for (int i = 0; i < turnManager.deckManager.playerHandsUI.Length; i++)
        {
            if (transform.parent == turnManager.deckManager.playerHandsUI[i])
            {
                return i;
            }
        }
        Debug.LogError("Card owner could not be determined.");
        return -1;
    }

    public void DeselectAllCards()
    {
        foreach (DeckManager.Card selectedCard in selectedCards)
        {
            // Find the corresponding CardClickable component and update its visuals
            CardClickable[] cardClickables = FindObjectsByType<CardClickable>(FindObjectsSortMode.None);
            foreach (CardClickable cardClickable in cardClickables)
            {
                if (cardClickable.card == selectedCard)
                {
                    cardClickable.UpdateCardVisuals(false);
                }
            }
        }
        selectedCards.Clear();
    }

    void UpdateCardVisuals(bool isSelected)
    {
        // Change the card's appearance based on selection
        Image cardImage = GetComponentInChildren<Image>();

        if (isSelected)
        {
            cardImage.color = Color.yellow; // Example: Highlight the card when selected
        }
        else
        {
            cardImage.color = Color.white; // Reset to default color when deselected
        }
    }
}

