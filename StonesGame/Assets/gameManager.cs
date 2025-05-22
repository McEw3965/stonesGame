using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;


public class gameManager : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static gameManager Instance { get; private set; }
    private GameObject networkManager;

    [SerializeField]
    private GameObject seesaw;

    private Coroutine revealStonesCoroutine;

    //VARIABLES FOR STONES AND SCALES
    [SerializeField]
    private static GameObject leftScale;
    [SerializeField]
    private GameObject rightScale;


    public GameObject selectedStone;
    public GameObject selectedScale;

    public NetworkObject player1SelectedStone;
    public NetworkObject player1SelectedScale;

    public NetworkObject player2SelectedStone;
    public NetworkObject player2SelectedScale;

    private GameObject revealFirst;
    private GameObject revealSecond;

    [SerializeField]
    private NetworkVariable<bool> player1Ready;
    [SerializeField]
    private NetworkVariable<bool> player2Ready;

    public Dictionary<ulong, GameObject> clientIdToStone;
    public Dictionary<ulong, GameObject> clientIdToScale;

    //public NetworkVariable<Dictionary<ulong, string>> NetworkDictionaryTest;

    private ulong player1ID;
    private ulong player2ID;
    public ulong localClientId;

    [SerializeField]
    public whichPlayer currentPlayer;
    public whichPlayer playerTorevealFirst = gameManager.whichPlayer.player1;

    public enum whichPlayer
    {
        player1,
        player2
    }

    private void Awake()
    {
        networkManager = GameObject.Find("NetworkManager");

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(networkManager);
        //DontDestroyOnLoad(Camera.main);

        if (Instance != null)
        {
            Debug.LogError("More than one GameManager Instance");
        }
        Instance = this;

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //endTurnActionsRpc();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += HandleNetworkSceneEventServer;
        } else
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += HandleNetworkSceneEventClient;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null)
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent -= HandleNetworkSceneEventServer;
            NetworkManager.Singleton.SceneManager.OnSceneEvent -= HandleNetworkSceneEventClient;
        }
    }

    //GAME EVENTS

    public IEnumerator revealStones(NetworkObject firstStone, NetworkObject secondStone, NetworkObject firstScale, NetworkObject secondScale)
    {

        float firstStoneWeight = firstStone.GetComponent<interactableObject>().weight.Value;
        float secondStoneWeight = secondStone.GetComponent<interactableObject>().weight.Value;

        firstStone.gameObject.GetComponent<interactableObject>().isPlayed.Value = true;
        yield return new WaitForSecondsRealtime(1.0f); //Suspends execution for 2 seconds
        if (IsServer)
        {

            firstScale.GetComponent<interactableObject>().weight.Value += firstStoneWeight;

            if (firstScale.gameObject == rightScale)
            {
                leftScale.GetComponent<interactableObject>().weight.Value -= firstStoneWeight;
                seesaw.GetComponent<tiltController>().FindTarget(-firstStoneWeight);

            }
            else if (firstScale.gameObject == leftScale)
            {
                rightScale.GetComponent<interactableObject>().weight.Value -= firstStoneWeight;
                seesaw.GetComponent<tiltController>().FindTarget(firstStoneWeight);

            }
        }


        yield return new WaitForSecondsRealtime(1.0f);
        secondStone.gameObject.GetComponent<interactableObject>().isPlayed.Value = true;
        yield return new WaitForSecondsRealtime(1.0f);

        if (IsServer)
        {
            secondScale.GetComponent<interactableObject>().weight.Value += secondStoneWeight;


            if (secondScale.gameObject == rightScale)
            {
                leftScale.GetComponent<interactableObject>().weight.Value -= secondStoneWeight;
                seesaw.GetComponent<tiltController>().FindTarget(-secondStoneWeight);

            }
            else if (secondScale.gameObject == leftScale)
            {
                rightScale.GetComponent<interactableObject>().weight.Value -= secondStoneWeight;
                seesaw.GetComponent<tiltController>().FindTarget(secondStoneWeight);

            }
        }


        yield return new WaitForSecondsRealtime(2.0f);
        resetVarsRpc();
        checkWinCondition();
    }

    [Rpc(SendTo.Server)]
    public void endTurnActionsRpc()
    {
        Debug.Log("Turn Ended");
        if (player1Ready.Value == true || player2Ready.Value == true)
        {
            switch (playerTorevealFirst)
            {
                case whichPlayer.player1:
                    revealStonesCoroutine = StartCoroutine(revealStones(player1SelectedStone, player2SelectedStone, player1SelectedScale, player2SelectedScale));
                    break;
                case whichPlayer.player2:
                    revealStonesCoroutine = StartCoroutine(revealStones(player2SelectedStone, player1SelectedStone, player2SelectedScale, player1SelectedScale)); ;
                    break;
            }

            if (player1SelectedStone.GetComponent<interactableObject>().weight.Value > player2SelectedStone.GetComponent<interactableObject>().weight.Value)
            {
                playerTorevealFirst = whichPlayer.player1;
            }
            else if (player2SelectedStone.GetComponent<interactableObject>().weight.Value > player1SelectedStone.GetComponent<interactableObject>().weight.Value)
            {
                playerTorevealFirst = whichPlayer.player2;
            }

        }
        else
        {
            Debug.Log("Both Players must be ready to end turn");
        }
    }

    [Rpc(SendTo.Server)]
    private void resetVarsRpc()
    {
        //Destroy(player1SelectedStone.gameObject);
        //Destroy(player2SelectedStone.gameObject);
        //if (currentPlayer == gameManager.whichPlayer.player1)
        //{
        //    player1SelectedStone.gameObject.GetComponent<interactableObject>().player1ActiveRpc();
        //}
        //else if (currentPlayer == gameManager.whichPlayer.player2)
        //{
        //    player2SelectedStone.gameObject.GetComponent<interactableObject>().player2ActiveRpc();
        //}

        player1SelectedStone.GetComponent<interactableObject>().isDisabled.Value = true;
        player2SelectedStone.GetComponent<interactableObject>().isDisabled.Value = true;

        player1SelectedScale = null;
        player2SelectedScale = null;
        selectedStone = null;
        selectedScale = null;
        player1Ready.Value = false;
        player2Ready.Value = false;
    }

    [Rpc(SendTo.Server)]
    public void endTurnRpc(whichPlayer tempPlayer)
    {
        switch (tempPlayer)
        {
            case whichPlayer.player1:
                if (player1SelectedScale != null && player1SelectedStone != null)
                {
                    player1Ready.Value = true;
                }
                else
                {
                    Debug.Log(currentPlayer + " must select a stone and a scale");
                }
                break;

            case whichPlayer.player2:
                if (player2SelectedScale != null && player2SelectedStone != null)
                {
                    player2Ready.Value = true;
                }
                else
                {
                    Debug.Log(currentPlayer + " must select a stone and a scale");
                }

                break;
        }

        if (player1Ready.Value && player2Ready.Value)
        {
            endTurnActionsRpc();
        }
        else
        {
            Debug.Log("Wait for other player to be ready");
        }


    }

    public void callEndTurn()
    {
        endTurnRpc(currentPlayer);
    }


    [Rpc(SendTo.Server)]
    public void addWeightRpc() //May need to be split intop two separate methods for use with animation triggers
    {
        if (currentPlayer == whichPlayer.player1)
        {
            addPlayer1WeightRpc();
        }
        else if (currentPlayer == whichPlayer.player2)
        {
            addPlayer2WeightRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void addPlayer1WeightRpc()
    {
        player1SelectedScale.gameObject.GetComponent<interactableObject>().weight.Value += player1SelectedStone.gameObject.GetComponent<interactableObject>().weight.Value;
    }

    [Rpc(SendTo.Server)]
    private void addPlayer2WeightRpc()
    {
        player2SelectedScale.gameObject.GetComponent<interactableObject>().weight.Value += player2SelectedStone.gameObject.GetComponent<interactableObject>().weight.Value;
    }

    [Rpc(SendTo.Server)]
    private void assignStonesRpc()
    {

        switch (currentPlayer)
        {
            case whichPlayer.player1:
                player1SelectedStone = selectedStone.GetComponent<NetworkObject>();
                player1SelectedScale = selectedScale.GetComponent<NetworkObject>();
                break;
            case whichPlayer.player2:
                player2SelectedStone = selectedStone.GetComponent<NetworkObject>();
                player2SelectedScale = selectedScale.GetComponent<NetworkObject>();
                break;

        }
    }

    [Rpc(SendTo.Server)]
    private void calculateDifferenceRpc()
    {
        float leftScaleWeight = leftScale.GetComponent<interactableObject>().weight.Value;
        float rightScaleWeight = rightScale.GetComponent<interactableObject>().weight.Value;
        float difference = 0;

        if (leftScaleWeight > rightScaleWeight)
        {
            difference = leftScaleWeight - rightScaleWeight;
        }
        else if (rightScaleWeight > leftScaleWeight)
        {
            difference = rightScaleWeight - leftScaleWeight;
        }
        checkWinCondition();
    }

    private void checkWinCondition()
    {
        if (rightScale.GetComponent<interactableObject>().weight.Value >= 10 || leftScale.GetComponent<interactableObject>().weight.Value >= 10)
        {
            Debug.Log("Game Over");
            NetworkManager.SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Next Turn");
        }
    }

    //SCENE MANAGEMENT

    private void HandleNetworkSceneEventServer(SceneEvent sceneEvent)
    {
        switch(sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadEventCompleted:
                if(sceneEvent.SceneName == "MultiplayerScene")
                {
                    player1Ready.Value = false;
                    player2Ready.Value = false;

                    if(IsServer && NetworkManager.Singleton.ConnectedClientsIds.Count > 0)
                    {
                        if (NetworkManager.Singleton.ConnectedClientsIds[0] == NetworkManager.Singleton.LocalClientId)
                        {
                            currentPlayer = whichPlayer.player1;
                        } else if (NetworkManager.Singleton.ConnectedClientsIds.Count > 1 && NetworkManager.Singleton.ConnectedClientsIds[1] == NetworkManager.Singleton.LocalClientId)
                        {
                            currentPlayer = whichPlayer.player2;
                        }
                    }

                    if (boardGenerator.Instance != null) // Always check for null before accessing singleton Instance
                    {
                        boardGenerator.Instance.callSpawnStones(currentPlayer);
                        Debug.Log("SERVER: Initiated stone spawning after all clients loaded scene.");
                    }
                    else
                    {
                        Debug.LogError("SERVER: boardGenerator.Instance is null! Cannot spawn stones.");
                    }

                    Invoke(nameof(InitialiseSceneObjects), 0.5f);

                }


                break;
        }
    }

    private void HandleNetworkSceneEventClient(SceneEvent sceneEvent)
    {
        switch(sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                if(sceneEvent.SceneName == "MultiplayerScene")
                {
                    localClientId = NetworkManager.Singleton.LocalClientId;

                    if(multiplayerManager.Instance != null && multiplayerManager.Instance.connectedClientIds != null)
                    {
                        switch(localClientId)
                        {
                            case ulong clientId when multiplayerManager.Instance.connectedClientIds.Count > 0 && clientId == multiplayerManager.Instance.connectedClientIds[0]:
                                currentPlayer = whichPlayer.player1;
                                break;

                                case ulong clientId when multiplayerManager.Instance.connectedClientIds.Count > 1 && clientId == multiplayerManager.Instance.connectedClientIds[1]:
                                currentPlayer = whichPlayer.player2;
                                break;

                            default:
                                Debug.LogWarning("Client player role not assgined or clientIds not ready");
                                break;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("MultiplayerManager Instance or clientIds null");
                    }
                    Invoke(nameof(InitialiseSceneObjects), 0.5f);
                } 
 
                break;

            case SceneEventType.LoadEventCompleted:
                Debug.Log("Client recieved LoadEvent");
                break;
        }

    }
    //private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    //{

    //    if (scene.name == "MultiplayerScene")
    //    {
    //        localClientId = NetworkManager.Singleton.LocalClientId;

    //        if (IsServer)
    //        {
    //            player1Ready.Value = false;
    //            player2Ready.Value = false;
    //        }

    //        switch (localClientId)
    //        {
    //            case ulong clientId when clientId == multiplayerManager.Instance.connectedClientIds[0]:
    //                currentPlayer = whichPlayer.player1;
    //                Debug.Log("Current player is player 1");
    //                break;

    //            case ulong clientId when clientId == multiplayerManager.Instance.connectedClientIds[1]:
    //                currentPlayer = whichPlayer.player2;
    //                Debug.Log("Current player is player 2");

    //                break;
    //        }

    //        Invoke(nameof(InitialiseSceneObjects), 0.5f);

    //        Debug.Log("Scene Loaded");
    //        if (IsServer)
    //        {
    //            boardGenerator.Instance.callSpawnStones(currentPlayer);
    //        }
    //    }
    //    else if (scene.name == "GameOver")
    //    {
    //        Debug.Log("GameOver Scene loaded");
    //    }
    //}

    private void InitialiseSceneObjects()
    {
        if (IsServer)
        {
            leftScale = GameObject.Find("Left Scale");
            rightScale = GameObject.Find("Right Scale");
            seesaw = GameObject.Find("SeeSaw");

        }


    }

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += onSceneLoaded; // Corrected line
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= onSceneLoaded; // Corrected line
    //}

    public void onlineGame()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("MultiplayerScene", LoadSceneMode.Single);
        }
    }


}
