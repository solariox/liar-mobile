using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class TurnManager : MonoBehaviour
{
    public DeckManager deckManager; // Reference to DeckManager
    public TextMeshProUGUI turnText; // UI Text to display the current player's turn
    public GameObject timerUI; // Timer UI element to display the countdown
    public float turnTime = 30f; // 30 seconds for each turn
    private float currentTurnTime; // Tracks the current time for the player's turn

    public int currentPlayerIndex = 0; // Tracks which player's turn it is

    private void Start()
    {
        StartTurn(currentPlayerIndex);
    }

    void StartTurn(int playerIndex)
    {
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
        turnText.text = "Player " + (currentPlayerIndex + 1) + "'s Turn";
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

    void EndTurn()
    {

        // Check if the player has made a claim or if the turn ended automatically
        Debug.Log("Player " + (currentPlayerIndex + 1) + " has ended their turn.");

        // Move to the next player
        currentPlayerIndex = (currentPlayerIndex + 1) % 4; // Cycle through 4 players
        StartTurn(currentPlayerIndex); // Start the next player's turn
    }
}
