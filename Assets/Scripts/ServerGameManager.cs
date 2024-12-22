using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ServerGameManager : NetworkBehaviour
{
    // A simple list to represent a deck of cards (e.g., 1, 2, 3, ..., 10)
    private List<Card> deck = new List<Card>();
    private List<Player> players = new List<Player>();
    private string currentTurnType;
    private Player currentPlayer;

    public static ServerGameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple instances of ServerGameManager detected! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Optionally, make it persist across scenes
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// Client: Requests the server to shuffle the deck.
    /// </summary>
    public void RequestStartGame()
    {

        CreatePlayers();
        ShuffleDeck();
        DealCards();
        InitTurn();
        NotifyGameIsReadyToBeginClientRPC();
    }


    private void CreatePlayers()
    {
        players.Add(new Player("Soos", 6, new List<Card>(), new List<Card>()));
        players.Add(new Player("Runj", 6, new List<Card>(), new List<Card>()));
        players.Add(new Player("Nocta", 6, new List<Card>(), new List<Card>()));
        players.Add(new Player("Popo", 6, new List<Card>(), new List<Card>()));
    }

    /// <summary>
    /// Server: Shuffles the deck.
    /// </summary>
    private void ShuffleDeck()
    {
        for (int i = 0; i < 6; i++)
        {
            deck.Add(new Card("King"));
            deck.Add(new Card("Queen"));
            deck.Add(new Card("Ace"));
        }

        deck.Add(new Card("Joker"));
        deck.Add(new Card("Joker"));
        // shuffle the deck
        deck = deck.OrderBy(x => Random.value).ToList();
    }


    /// <summary>
    /// Server: Deals cards to players.
    /// </summary>
    void DealCards()
    {
        foreach (Player player in players)
        {
            for (int i = 0; i < 5; i++)
            {
                player.AddCard(deck[0]);
                deck.RemoveAt(0);
            }
        }
    }


    private void InitTurn()
    {
        string[] turnTypeArray = { "King", "Queen", "Ace" };
        int randomIndex = Random.Range(0, turnTypeArray.Length);
        currentTurnType = turnTypeArray[randomIndex];
        currentPlayer = players[0];
    }


    /// <summary>
    /// Client: Notifies all clients that the deck has been shuffled.
    /// </summary>
    [ClientRpc]
    private void NotifyGameIsReadyToBeginClientRPC()
    {
        // Call UiManager to update the UI on all clients
        UIManager.Instance.UpdateGameStartUI(currentTurnType, players, currentPlayer);
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }
}
