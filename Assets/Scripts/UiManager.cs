using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    public Sprite aceSprite;
    public Sprite kingSprite;
    public Sprite queenSprite;
    public Sprite jokerSprite;
    public Sprite backSprite;
    public GameObject cardPrefab; // Prefab for card UI
    // TODO: get this by reference instead 
    public Transform[] playerHandsUI; // Containers for each player's cards


    private Dictionary<string, Sprite> cardLibrary;
    private TextMeshProUGUI turnTypeText;
    private TextMeshProUGUI turnPlayerText;
    private TextMeshProUGUI winText;
    private Transform commonBoard; // Containers for each player's cards

    public List<Card> selectedCards = new List<Card>();
    public List<Card> lastPlayedCards = new List<Card>();


    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        cardLibrary = new Dictionary<string, Sprite>()
        {
            { "Ace", aceSprite },
            { "King", kingSprite },
            { "Queen", queenSprite },
            { "Joker", jokerSprite },
        };

        turnPlayerText = GameObject.Find("TurnPlayer").GetComponent<TextMeshProUGUI>();
        turnTypeText = GameObject.Find("TurnType").GetComponent<TextMeshProUGUI>();
        winText = GameObject.Find("WinText").GetComponent<TextMeshProUGUI>();
        commonBoard = GameObject.Find("CommonBoard").transform;

    }



    /// <summary>
    /// Updates the UI to show the game's start state.
    /// </summary>
    public void UpdateGameUi(string currentTurnType, List<Player> players, Player currentPlayer, List<Card> commonDeck)
    {

        turnTypeText.text = currentTurnType;
        turnPlayerText.text = currentPlayer.Name;
        DisplayAllPlayerUi(players);
        DisplayAllCards(players, commonDeck);
    }


    private void DisplayAllPlayerUi(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            GameObject.Find("Player" + (i + 1) + "Name").GetComponent<TextMeshProUGUI>().text = players[i].Name;
            GameObject.Find("Player" + (i + 1) + "Gun").GetComponent<TextMeshProUGUI>().text = "1/" + players[i].Chances;
        }
    }

    private void DisplayAllCards(List<Player> players, List<Card> lCommonDeck)
    {

        for (int i = 0; i < players.Count; i++)
        {
            // First, clear previous data
            foreach(Transform child in playerHandsUI[i].transform)
            {
                Destroy(child.gameObject);
            }


            // TODO: set the current player
            bool isPlayerOne = i == 0;

            foreach (Card card in players[i].Cards)
            {
                GameObject cardObject = Instantiate(cardPrefab, playerHandsUI[i]);

                // Set card text and image
                Text cardText = cardObject.GetComponentInChildren<Text>();
                Image cardImage = cardObject.GetComponentInChildren<Image>();

                if (cardText != null)
                {
                    cardText.text = isPlayerOne ? card.Rank : ""; // Show rank for player 1, hide for others
                }

                if (cardImage != null)
                {
                    if (isPlayerOne && cardLibrary.ContainsKey(card.Rank))
                    {
                        cardImage.sprite = cardLibrary[card.Rank]; // Set the correct face-up image
                    }
                    else
                    {
                        cardImage.sprite = backSprite; // Or use your back card image
                    }
                }

                // Attach the card data to the clickable script
                CardClickable cardClickable = cardObject.GetComponent<CardClickable>();
                if (cardClickable != null)
                {
                    cardClickable.card = card;
                }
            }

            if (lCommonDeck != null)
            {
                // First, clear previous data
                foreach (Transform child in commonBoard)
                {
                    Destroy(child.gameObject);
                }

                foreach (Card card in lCommonDeck)
                {
                    GameObject cardObject = Instantiate(cardPrefab, playerHandsUI[i]);

                    // Set card text and image
                    Text cardText = cardObject.GetComponentInChildren<Text>();
                    Image cardImage = cardObject.GetComponentInChildren<Image>();
                    cardImage.sprite = backSprite;
                    cardObject.transform.SetParent(commonBoard);
                }
            }
        }
    }


    public void playCards()
    {

        //    // Validate the play
        if (selectedCards.Count == 0)
        {
            Debug.LogWarning("No cards selected!");
            return;
        }

        ServerGameManager.Instance.PlayCards(selectedCards);
        selectedCards.Clear();
    }

    public void CallLiar()
    {
        ServerGameManager.Instance.CallLiar();
    }


    public void NotifyClient(string message)
    {
        winText.text = message;
        StartCoroutine(ClearMessageAfterDelay(5f)); // Clear after 5 seconds
    }

    private IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        winText.text = ""; // Clear the text
    }
}
