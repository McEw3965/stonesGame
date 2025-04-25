using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;


public class gameManager : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static gameManager Instance { get; private set; }
    private GameObject networkManager;

    //VARIABLES FOR STONES AND SCALES
    [SerializeField]
    private GameObject leftScale;
    private GameObject rightScale;


    public GameObject selectedStone;
    public GameObject selectedScale;

    public NetworkObject player1SelectedStone;
    public NetworkObject player1SelectedScale;

    public NetworkObject player2SelectedStone;
    public NetworkObject player2SelectedScale;

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

    }

    //GAME EVENTS



    //[Rpc(SendTo.Server)]
    //public void callEndTurn()
    //{
    //    if(IsServer && IsClient)
    //    {
    //        endTurnRpc();
    //    } else if(IsServer && !IsClient)
    //    {
    //        endTurnRpc();
    //    }
    //}

    [Rpc(SendTo.Server)]
    public void endTurnActionsRpc()
    {
        if (player1Ready.Value == true || player2Ready.Value == true)
        {
            //addWeightToScaleRpc();
            //assignStonesRpc();
            addWeightRpc();
            calculateDifferenceRpc();
            Destroy(player1SelectedStone.gameObject);
            Destroy(player2SelectedStone.gameObject);
            player1SelectedScale = null;
            player2SelectedScale = null;
            selectedStone = null;
            selectedScale = null;
            player1Ready.Value = false;
            player2Ready.Value = false;
        }
        else
        {
            Debug.Log("Both Players must be ready to end turn");
        }
    }

    [Rpc(SendTo.Server)]
    public void endTurnRpc()
    {
        switch (currentPlayer)
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

        endTurnRpc();
    }

    [Rpc(SendTo.Server)]
    private void addWeightRpc() //May need to be split intop two separate methods for use with animation triggers
    {
        player1SelectedScale.gameObject.GetComponent<interactableObject>().weight.Value += player1SelectedStone.gameObject.GetComponent<interactableObject>().weight.Value;
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
        checkWinCondition(difference);
    }

    private void checkWinCondition(float difference)
    {
        if (difference > 6)
        {
            Debug.Log("Game Over");
        }
        else
        {
            Debug.Log("Next Turn");
        }
    }

    //SCENE MANAGEMENT
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {


        if (scene.name == "MultiplayerScene")
        {
            localClientId = NetworkManager.Singleton.LocalClientId;

            player1Ready.Value = false;
            player2Ready.Value = false;

            switch (localClientId)
            {
                case ulong clientId when clientId == multiplayerManager.Instance.connectedClientIds[0]:
                    currentPlayer = whichPlayer.player1;
                    Debug.Log("Current player is player 1");
                    break;

                case ulong clientId when clientId == multiplayerManager.Instance.connectedClientIds[1]:
                    currentPlayer = whichPlayer.player2;
                    Debug.Log("Current player is player 2");

                    break;
            }

            ////playerOne.Value.playerId = multiplayerManager.Instance.connectedClientIds[0];
            //Debug.Log("Player 1 CLientId: " + player1ID);
            //player2ID = multiplayerManager.Instance.connectedClientIds[1];
            //Debug.Log("Player 2 ClientId " + player2ID);

            ////NetworkDictionaryTest.Value[player1ID] = "Successful";
            ////Debug.Log("Network Dictionary Test: " + NetworkDictionaryTest.Value[player1ID]);

            ////clientIdToStone[player1ID] = null;
            ////clientIdToStone[player2ID] = null;


            if (IsServer)
            {
                Debug.Log("Multiplayer Scene Loaded");
                leftScale = GameObject.Find("Left Scale");
                rightScale = GameObject.Find("Right Scale");

                Debug.Log(boardGenerator.Instance);
                boardGenerator.Instance.SpawnStonesRpc();
            }

            GameObject endTurnObj = GameObject.Find("End Turn Button");
            Button endTurnBtn = endTurnObj.GetComponent<Button>();
            endTurnBtn.onClick.AddListener(endTurnRpc);

        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded; // Corrected line
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded; // Corrected line
    }

    public void onlineGame()
    {
        SceneManager.LoadScene("MultiplayerScene", LoadSceneMode.Single);
    }


}
