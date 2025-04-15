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

    public GameObject player1SelectedStone;
    public GameObject player1SelectedScale;
    public GameObject player2SelectedStone;
    public GameObject player2SelectedScale;

    public Dictionary<ulong, GameObject> clientIdToStone;
    public Dictionary<ulong, GameObject> clientIdToScale;

    //public NetworkVariable<Dictionary<ulong, string>> NetworkDictionaryTest;

    private ulong player1ID;
    private ulong player2ID;
    private ulong localClientId;

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
    public void endTurnRpc()
    {
        Debug.Log("Local Client ID: " + localClientId);
        addWeightToScaleRpc();
        calculateDifference();
    }

    [Rpc(SendTo.Server)]
    private void addWeightToScaleRpc()
    {
        Debug.Log("Adding Weight");
        clientIdToScale[localClientId].GetComponent<interactableObject>().weight += clientIdToStone[localClientId].GetComponent<interactableObject>().weight;
        Destroy(clientIdToStone[localClientId]);
        clientIdToStone[localClientId] = null;
        clientIdToScale[localClientId] = null;
    }

    private void calculateDifference()
    {
        float leftScaleWeight = leftScale.GetComponent<interactableObject>().weight;
        float rightScaleWeight = rightScale.GetComponent<interactableObject>().weight;
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
            player1ID = multiplayerManager.Instance.connectedClientIds[0];
            Debug.Log("Player 1 CLientId: " + player1ID);
            player2ID = multiplayerManager.Instance.connectedClientIds[1];
            Debug.Log("Player 2 ClientId " + player2ID);

            //NetworkDictionaryTest.Value[player1ID] = "Successful";
            //Debug.Log("Network Dictionary Test: " + NetworkDictionaryTest.Value[player1ID]);

            clientIdToStone[player1ID] = null;
            clientIdToStone[player2ID] = null;


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
