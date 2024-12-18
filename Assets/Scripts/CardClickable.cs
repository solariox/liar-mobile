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

        // Log the selected cards
        Debug.Log("Selected Cards: " + selectedCards.Count);
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
