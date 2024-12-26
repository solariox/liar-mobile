using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using Unity.Netcode;
using System;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Collections;
using System.Linq;
using System.Collections.Generic;
using Unity.Services.Qos.V2.Models;


public class CustomRelayManager : NetworkBehaviour
{
    public TextMeshProUGUI Status;
    public TMP_InputField PlayerIdInput;
    public TMP_InputField RoomIdInput;

    private string joinCode = "n/a";
    private string playerId = "Not signed in";
    private Allocation allocation;

    private NetworkList<PlayerObject> players;

    void Awake()
    {
        players = new NetworkList<PlayerObject>();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        players.OnListChanged += OnPlayersListChanged;
        AddPlayer(NetworkManager.Singleton.LocalClientId, PlayerIdInput.text);
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();
        UpdateUI();
    }

    void UpdateUI()
    {
        // Replace the problematic line with the following code
        var playerNames = new List<string>();
        foreach (var player in players)
        {
            playerNames.Add(player.PlayerName.ToString());
        }

        // Join player names into a single string for display
        Status.text = $"Join Code: {joinCode}\nPlayers: {string.Join(", ", playerNames)}";
    }

    private void OnPlayersListChanged(NetworkListEvent<PlayerObject> changeEvent)
    {
        UpdateUI();
    }



    public async void OnCreateRoom()
    {
        await OnSignIn();
        await OnAllocate();
        await OnCreateCode();

        // Start Netcode as Host
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        NetworkManager.Singleton.StartHost();

        Debug.Log("Host started. Waiting for players...");
        UpdateUI();
    }

    public async void OnJoinRoom()
    {
        string JoinInputText = RoomIdInput.text;
        await OnSignIn();

        try
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(JoinInputText);
            Debug.Log($"Player joined room with Allocation ID: {joinAllocation.AllocationId}");

            // Connect as Client
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
            NetworkManager.Singleton.StartClient();

        }
        catch (RelayServiceException ex)
        {
            Debug.LogError($"Failed to join room: {ex.Message}");
        }
    }

    private async Task OnSignIn()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"Signed in. Player ID: {playerId}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Sign-in failed: {ex.Message}");
        }
    }

    private async Task OnAllocate()
    {
        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(4);
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError($"Allocation failed: {ex.Message}");
        }
    }

    private async Task OnCreateCode()
    {
        try
        {
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"Join Code: {joinCode}");
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError($"Failed to get join code: {ex.Message}");
        }
    }

    // Create a function that tell all client to change scene
    public void OnStartGame()
    {
       NetworkManager.Singleton.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void AddPlayer(ulong clientId, string playerName)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            players.Add(new PlayerObject(clientId, playerName));
        }
        else
        {
            SubmitPlayerRequestServerRpc(clientId, playerName);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitPlayerRequestServerRpc(ulong clientId, string playerName)
    {
        players.Add(new PlayerObject(clientId, playerName));
    }
}
