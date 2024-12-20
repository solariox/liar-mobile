using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;


public class TurnManager : MonoBehaviour
{
    public DeckManager deckManager; // Reference to DeckManager
    public TextMeshProUGUI turnPlayerText; // UI Text to display the current player's turn
    public TextMeshProUGUI turnTypeText; // UI Text to display the current player's turn
    public GameObject timerUI; // Timer UI element to display the countdown
    public float turnTime = 30f; // 30 seconds for each turn
    public int currentPlayerIndex = 0; // Tracks which player's turn it is

    private float currentTurnTime; // Tracks the current time for the player's turn
    private string[] turnTypeArray = new string[] { "Ace", "King", "Queen" };
    private string turnType;

    private List<DeckManager.Card> lastPlayedCards = new List<DeckManager.Card>();




    private void Start()
    {
        StartTurn(currentPlayerIndex);
    }

    void StartTurn(int playerIndex)
    {
        // select the turn type random between Ace, King, Queen, Joker
        int randomIndex = Random.Range(0, turnTypeArray.Length);
        turnType = turnTypeArray[randomIndex];

        currentPlayerIndex = playerIndex;
        currentTurnTime = turnTime;
 

        timerUI.SetActive(true); // Show the timer UI
        UpdateTurnUI();

        // Start a coroutine to handle the countdown timer
        StartCoroutine(TurnCountdown());
    }

    void UpdateTurnUI()
    {
        // Display whose turn it is
        turnPlayerText.text = "Player " + (currentPlayerIndex + 1) + "'s Turn";
        Debug.Log("Player " + (currentPlayerIndex + 1) + "'s Turn with turn type: " + turnType);
        turnTypeText.text = "Turn Type: " + turnType;
    }


    IEnumerator TurnCountdown()
    {
        while (currentTurnTime > 0)
        {
            currentTurnTime -= Time.deltaTime;
            timerUI.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Ceil(currentTurnTime).ToString(); // Update timer UI
            yield return null; // Wait until the next frame
        }

        EndTurn();
    }

    public void PlayCards()
    {

       Debug.Log("PlayCards");
        // Get the selected cards
        List<DeckManager.Card> selectedCards = CardClickable.selectedCards;
        // log all cards selected
        foreach (DeckManager.Card card in selectedCards)
        {
            Debug.Log("Selected card: " + card.Rank);
        }

        // Validate the play
        //if (selectedCards.Count == 0)
        //{
        //    Debug.LogWarning("No cards selected!");
        //    return;
        //}

        //// Check if cards match the turn type
        //foreach (DeckManager.Card card in selectedCards)
        //{
        //    if (card.Rank != turnType)
        //    {
        //        Debug.LogWarning("Selected cards do not match the turn type!");
        //        return;
        //    }
        //}

        //// If valid, record the played cards
        //lastPlayedCards = new List<DeckManager.Card>(selectedCards);

        //// Remove played cards from the player's hand
        //deckManager.RemoveCardsFromPlayer(currentPlayerIndex, selectedCards);

        // Move to the next turn
        EndTurn();
    }

    public void CallLiar()
    {
        // Check the last played cards
        if (lastPlayedCards.Count == 0)
        {
            Debug.LogWarning("No cards to challenge!");
            return;
        }

        // Validate the claim
        foreach (DeckManager.Card card in lastPlayedCards)
        {
            if (card.Rank != turnType)
            {
                Debug.Log("Liar detected! Previous player loses!");
                return;
            }
        }

        Debug.Log("False accusation! Current player loses!");

        // Apply penalties (e.g., draw cards, lose points) and move to the next turn
        EndTurn();
    }


    void EndTurn()
    {

        // Check if the player has made a claim or if the turn ended automatically
        Debug.Log("Player " + (currentPlayerIndex + 1) + " has ended their turn.");

        // Move to the next player
        currentPlayerIndex = (currentPlayerIndex + 1) % 4; // Cycle through 4 players
        StartTurn(currentPlayerIndex); // Start the next player's turn
    }
}
