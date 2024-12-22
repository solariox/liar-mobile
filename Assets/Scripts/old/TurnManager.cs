using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;


public class TurnManager : MonoBehaviour
{
    //public DeckManager deckManager; // Reference to DeckManager
    //public TextMeshProUGUI turnPlayerText; // UI Text to display the current player's turn
    //public TextMeshProUGUI turnTypeText; // UI Text to display the current player's turn
    //public GameObject timerUI; // Timer UI element to display the countdown
    //public float turnTime = 30f; // 30 seconds for each turn
    //public int currentPlayerIndex = 0; // Tracks which player's turn it is

    //private float currentTurnTime; // Tracks the current time for the player's turn
    //private string[] turnTypeArray = new string[] { "Ace", "King", "Queen" };
    //private string turnType;

    //CardClickable cardClickable;



    //private void Start()
    //{
    //    cardClickable = Object.FindFirstObjectByType<CardClickable>();

    //    // Init all players gun 
    //    for (int i = 0; i < 4; i++)
    //    {
    //        GameObject player = GameObject.Find("Player" + (i + 1));
    //        GameObject gun = player.transform.Find("Player" + (i + 1) + "Gun").gameObject;
    //        // find textmesh of gun
    //        gun.GetComponentInChildren<TextMeshProUGUI>().text = "0";
    //    }

    //    StartTurn(currentPlayerIndex);

    //}

    //void StartTurn(int playerIndex)
    //{

    //    currentPlayerIndex = playerIndex;
    //    currentTurnTime = turnTime;


    //    timerUI.SetActive(true); // Show the timer UI
    //    UpdateTurnUI();

    //    // Start a coroutine to handle the countdown timer
    //    StartCoroutine(TurnCountdown());
    //}

    //void UpdateTurnUI()
    //{
    //    // Display whose turn it is
    //    turnPlayerText.text = "Player " + (currentPlayerIndex + 1) + "'s Turn";
    //    turnTypeText.text = turnType;
    //}


    //IEnumerator TurnCountdown()
    //{
    //    while (currentTurnTime > 0)
    //    {
    //        currentTurnTime -= Time.deltaTime;
    //        timerUI.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Ceil(currentTurnTime).ToString(); // Update timer UI
    //        yield return null; // Wait until the next frame
    //    }

    //    EndTurn();
    //}

    //public void PlayCards()
    //{

    //    // Get the selected cards
    //    List<Card> selectedCards = CardClickable.selectedCards;

    //    // Validate the play
    //    if (selectedCards.Count == 0)
    //    {
    //        Debug.LogWarning("No cards selected!");
    //        return;
    //    }

    //    // If valid, record the played cards
    //    //lastPlayedCards = new List<Card>(selectedCards);
    //    deckManager.RemoveCardsFromPlayer(currentPlayerIndex, selectedCards);
    //    EndTurn();
    //}

    //public void CallLiar()
    //{
    //    // Check the last played cards
    //    if (lastPlayedCards.Count == 0)
    //    {
    //        Debug.LogWarning("No cards to challenge!");
    //        return;
    //    }

    //    TextMeshProUGUI looserText = GameObject.Find("WinText").GetComponent<TextMeshProUGUI>();
    //    // reveal the cards
    //    deckManager.revealCard(lastPlayedCards.Count);

    //    string text = null;

    //    // Validate the claim
    //    foreach (Card card in lastPlayedCards)
    //    {

    //        if (card.Rank != turnType && card.Rank != "Joker")
    //        {
    //            text = "Player " + (currentPlayerIndex) + " lost!";
    //            EndGame();
    //            break;
    //        }
    //    }

    //    text ??= "Player " + (currentPlayerIndex + 1) + " lost!";
    //    looserText.text = text;

    //    // Make looser shot one bullet
    //    GameObject player = GameObject.Find("Player" + (currentPlayerIndex + 1));
    //    GameObject gun = player.transform.Find("Player" + (currentPlayerIndex + 1) + "Gun").gameObject;
    //    TextMeshProUGUI gunText = gun.GetComponentInChildren<TextMeshProUGUI>();
    //    int freeSlots = int.Parse(gunText.text);
    //    // try to shoot, the chance of shooting is 1/freeslots
    //    if (Random.Range(0, freeSlots) == 0)
    //    {
    //        Debug.Log("Player " + (currentPlayerIndex + 1) + " died!");
    //    }
    //    else
    //    {
    //        // player survived
    //        gunText.text = (freeSlots - 1).ToString();
    //    }
    //    EndGame();
    //}



    //private void EndGame()
    //{
    //    this.turnPlayerText.text = "Game Over!";
    //    this.turnTypeText.text = "";
    //    timerUI.SetActive(false);
    //}

    //void EndTurn()
    //{
    //    cardClickable.DeselectAllCards();

    //    // Move to the next player
    //    currentPlayerIndex = (currentPlayerIndex + 1) % 4; // Cycle through 4 players
    //    StartTurn(currentPlayerIndex); // Start the next player's turn
    //}
}
