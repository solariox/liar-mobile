using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ServerGameManager : NetworkBehaviour
{
    // A simple list to represent a deck of cards (e.g., 1, 2, 3, ..., 10)
    private List<Card> deck = new List<Card>();
    private List<Card> commonDeck = new List<Card>();
    private List<Card> lastPlayedCards = new List<Card>();
    private List<Player> players = new List<Player>();


    private string currentTurnType;
    private Player currentPlayer;
    private int currentPlayerIndex;

    public static ServerGameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Optionally, make it persist across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CreatePlayers();

    }

    /// <summary>
    /// Client: Requests the server to shuffle the deck.
    /// </summary>
    public void RequestStartGame()
    {

        ShuffleDeck();
        DealCards();
        InitNewTurn();
        NotifyGameToUpdateUiClientRPC();
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
    private void DealCards()
    {
        foreach (Player player in players)
        {
            player.Cards.Clear();
            commonDeck.Clear();
            lastPlayedCards.Clear();

            for (int i = 0; i < 5; i++)
            {
                player.AddCard(deck[0]);
                deck.RemoveAt(0);
            }
        }
    }

    public void PlayCards(List<Card> cards)
    {
        lastPlayedCards = new List<Card>();

        Player player = cards[0].Owner;
        // Move cards from player to commonBoard
        foreach (Card card in cards)
        {
            lastPlayedCards.Add(card);
            commonDeck.Add(card);
            player.Cards.Remove(card);
        }

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        currentPlayer = players[currentPlayerIndex];

        NotifyGameToUpdateUiClientRPC();
    }

    public void CallLiar()
    {
        //Check the last played cards
        if (lastPlayedCards.Count == 0)
        {
            return;
        }

        Player looser = null;
        // Validate the claim
        foreach (Card card in lastPlayedCards)
        {
            if (card.Rank != currentTurnType && card.Rank != "Joker")
            {
                looser = players[currentPlayerIndex - 1];
                break;
            }
        }

        looser = looser ?? players[currentPlayerIndex];

        if (Random.Range(0, looser.Chances) == 0)
        {
            UIManager.Instance.NotifyClient(looser.Name + " died!");
        }
        else
        {
            UIManager.Instance.NotifyClient(looser.Name + " shoot and lived!");
            looser.Chances--;
        }

    }


    private void InitNewTurn()
    {
        string[] turnTypeArray = { "King", "Queen", "Ace" };
        int randomIndex = Random.Range(0, turnTypeArray.Length);
        currentTurnType = turnTypeArray[randomIndex];
        // TODO: Better way to do this ? 
        currentPlayerIndex = 0;
        currentPlayer = players[currentPlayerIndex];
    }


    /// <summary>
    /// Client: Notifies all clients that the deck has been shuffled.
    /// </summary>
    [ClientRpc]
    private void NotifyGameToUpdateUiClientRPC()
    {
        // Call UiManager to update the UI on all clients
        UIManager.Instance.UpdateGameUi(currentTurnType, players, currentPlayer, commonDeck);
    }


    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

}
