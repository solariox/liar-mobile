using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : NetworkBehaviour
{
    // Method to change scenes for all clients
    public void ChangeSceneForAll()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Only the server/host can change the scene.");
        }
    }
}
