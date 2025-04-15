using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class multiplayerManager : NetworkBehaviour
{
    public static multiplayerManager Instance { get; private set; }
    private string sceneName = "MultiplayerScene";

    private List<ulong> connectedClientIds = new List<ulong>(); //Stores ClientID's of connected players
    private Dictionary<ulong, NetworkObject> playerStates = new Dictionary<ulong, NetworkObject>(); //MapsClientID's to associated networkObjects
    private bool gameStarted = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer) //Checks if this insance is running on the server or client
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected; //OnClientConnected called when player joins
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected; //OnClientDisconnected called when player Leaves
        }
        Instance = this;
    }

    private void OnClientConnected(ulong clientId)
    {
        connectedClientIds.Add(clientId); //Adds Client Id to list of Client Id's
        Debug.Log("OnClientConnected is running");

        if (!gameStarted && connectedClientIds.Count == 2) //Checks if game has started and if 2 players are present
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

    public void StartGame()
    {
        Debug.Log("Start Game Running...");
        gameStarted = true;
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single); //Called on server, instructs all clients to load scene
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsServer && scene.name == sceneName)
        {
            boardGenerator.Instance.SpawnStonesRpc();

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
