using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CustomRelayManager : MonoBehaviour
{

    /// <summary>
    /// The textbox displaying the Status.
    /// </summary>
    public TextMeshProUGUI Status;
        /// <summary>
    /// The Button displaying the Player Id.
    /// </summary>
    public TMP_InputField PlayerIdInput;

    /// <summary>
    /// The Button displaying the Player Id.
    /// </summary>
    public TMP_InputField RoomIdInput;



    Guid hostAllocationId;
    Guid playerAllocationId;
    string allocationRegion = "";
    string joinCode = "n/a";
    string playerId = "Not signed in";
    string playerName = "Not signed in";


    async void Start()
    {
        await UnityServices.InitializeAsync();

        UpdateUI();
    }

    void UpdateUI()
    {
        Status.text = joinCode;

    }
    public async void OnCreateRoom()
    {
        await OnSignIn();
        await OnAllocate();
        await OnCreateCode();
        UpdateUI();
    }

    /// <summary>
    /// Event handler for when the Sign In button is clicked.
    /// </summary>
    private async Task OnSignIn()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            playerId = AuthenticationService.Instance.PlayerId;


            Debug.Log($"Signed in. Player ID: {playerId};");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to sign in: {ex.Message}");
        }
    }

    public async Task OnAssignPlayerName()
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(PlayerIdInput.ToString());
        playerName = AuthenticationService.Instance.PlayerName;
    }




    /// <summary>
    /// Event handler for when the Allocate button is clicked.
    /// </summary>
    private async Task OnAllocate()
    {
        Debug.Log("Host - Creating an allocation.");


        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
        // Important: Once the allocation is created, you have ten seconds to BIND
        hostAllocationId = allocation.AllocationId;
        allocationRegion = allocation.Region;
      
        Debug.Log($"Host Allocation ID: {hostAllocationId}, region: {allocationRegion}");

    }


    /// <summary>
    /// Event handler for when the Get Join Code button is clicked.
    /// </summary>
    private async Task OnCreateCode()
    {
        Debug.Log("Host - Getting a join code for my allocation. I would share that join code with the other players so they can join my session.");

        try
        {
            joinCode = await RelayService.Instance.GetJoinCodeAsync(hostAllocationId);
            Debug.Log("Host - Got join code: " + joinCode);
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }

    }

    /// <summary>
    /// Event handler for when the Join button is clicked.
    /// </summary>
    private async Task OnJoin()
    {
        Debug.Log("Player - Joining host allocation using join code.");

        try
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            playerAllocationId = joinAllocation.AllocationId;
            Debug.Log("Player Allocation ID: " + playerAllocationId);
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }

    }

}
