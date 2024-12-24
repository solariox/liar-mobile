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

public class CustomRelayManager : MonoBehaviour
{
    public TextMeshProUGUI Status;
    public TMP_InputField PlayerIdInput;
    public TMP_InputField RoomIdInput;

    private string joinCode = "n/a";
    private string playerId = "Not signed in";
    private Allocation allocation;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        UpdateUI();
    }

    void UpdateUI()
    {
        Status.text = $"Join Code: {joinCode}";
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

    // Triggered by the host to start the game
    public void OnStartGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            ChangeSceneForAll("GameScene");
        }
        else
        {
            Debug.LogError("Only the host can start the game.");
        }
    }

    // Change the scene for all connected players
    private void ChangeSceneForAll(string sceneName)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
