using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int currentPlayerIndex = 0; // Index for the current player (0 to 3)
    public DeckManager deckManager; // Reference to DeckManager to interact with cards
    public float turnDuration = 30f; // 30 seconds for each player to think

    private bool isTurnActive = false;
    private float turnTimer = 0f;

    void Start()
    {
        StartTurn();
    }

    void Update()
    {
        if (isTurnActive)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0f)
            {
                EndTurn();
            }
        }
    }

    void StartTurn()
    {
        isTurnActive = true;
        turnTimer = turnDuration;
        Debug.Log("Player " + (currentPlayerIndex + 1) + "'s turn");

        // Enable the UI and card interaction for the current player
        deckManager.EnablePlayerTurn(currentPlayerIndex);
    }

    void EndTurn()
    {
        isTurnActive = false;
        Debug.Log("End of Player " + (currentPlayerIndex + 1) + "'s turn");

        // Disable card interaction for the current player
        deckManager.DisablePlayerTurn(currentPlayerIndex);

        // Move to the next player
        currentPlayerIndex = (currentPlayerIndex + 1) % 4;
        StartTurn(); // Start the next player's turn
    }
}
