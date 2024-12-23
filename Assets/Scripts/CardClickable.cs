using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardClickable : MonoBehaviour, IPointerClickHandler
{
    public Card card; // This will be the card data associated with the UI
    private UIManager UIManager;
    private ServerGameManager ServerGameManager;

    void Start()
    {
        UIManager = UIManager.Instance;
        ServerGameManager = ServerGameManager.Instance;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if it's the correct player's turn
        if ((ServerGameManager.GetCurrentPlayer() != card.Owner))
        {
            return;
        }

        // Check if the card is already selected
        if (UIManager.selectedCards.Contains(card))
        {
            UIManager.selectedCards.Remove(card);
            UpdateCardVisuals(false); 
        }
        else
        {
            // If we have less than 3 cards selected, allow selecting a new card
            if (UIManager.selectedCards.Count < 3)
            {
                UIManager.selectedCards.Add(card);
                UpdateCardVisuals(true); 
            }
        }
    }


    //public void DeselectAllCards()
    //{
    //    foreach (Card selectedCard in selectedCards)
    //    {
    //        // Find the corresponding CardClickable component and update its visuals
    //        CardClickable[] cardClickables = FindObjectsByType<CardClickable>(FindObjectsSortMode.None);
    //        foreach (CardClickable cardClickable in cardClickables)
    //        {
    //            if (cardClickable.card == selectedCard)
    //            {
    //                cardClickable.UpdateCardVisuals(false);
    //            }
    //        }
    //    }
    //    selectedCards.Clear();
    //}

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

