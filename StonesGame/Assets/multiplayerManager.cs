using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class multiplayerManager : NetworkBehaviour
{
    public static multiplayerManager Instance { get; private set; }
    private string sceneName = "Multiplayer Scene";

    private List<ulong> connectedClientIds = new List<ulong>();
    private Dictionary<ulong, NetworkObject> playerStates = new Dictionary<ulong, NetworkObject>();
    private bool gameStarted = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
        Instance = this;
    }

    private void OnClientConnected(ulong clientId)
    {
        connectedClientIds.Add(clientId);
        Debug.Log("OnClientConnected is running");

        if (!gameStarted && connectedClientIds.Count == 2)
        {
            Debug.Log("Game starting...");
            StartGame();
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        connectedClientIds.Remove(clientId);
        playerStates.Remove(clientId);
    }

    private void StartGame()
    {
        gameStarted = true;
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsServer && scene.name == sceneName)
        {
            foreach(ulong clientId in connectedClientIds)
            {
                //playerStates[clientId] = NetworkObject;
            }
        }
    }

    void OnEnable()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null)
        {
            SceneManager.sceneLoaded += OnGameSceneLoaded;
        }
    }

    void OnDisable()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null)
        {
            SceneManager.sceneLoaded -= OnGameSceneLoaded;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
