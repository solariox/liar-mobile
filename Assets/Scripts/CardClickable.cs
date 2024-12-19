using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardClickable : MonoBehaviour, IPointerClickHandler
{
    public DeckManager.Card card; // This will be the card data associated with the UI
    public static List<DeckManager.Card> selectedCards = new List<DeckManager.Card>(); // Keeps track of selected cards

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(card);
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

    void DeselectAllCards()
    {
        foreach (DeckManager.Card selectedCard in selectedCards)
        {
            // Find the corresponding CardClickable component and update its visuals
            CardClickable[] cardClickables = FindObjectsOfType<CardClickable>();
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
