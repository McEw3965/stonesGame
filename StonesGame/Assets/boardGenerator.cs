using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using Unity.Netcode;



public class boardGenerator : NetworkBehaviour
{

    public static boardGenerator Instance { get; private set; }

    public GameObject player1Stone;
    public GameObject player2Stone;

    gameManager.whichPlayer currentPlayer;

    private Vector3 SpawnPoint = new Vector3(2f, 2f, 0f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MultiplayerScene")
        {
            ulong localClientId;

            localClientId = NetworkManager.Singleton.LocalClientId;

            switch (localClientId)
            {
                case ulong clientId when clientId == multiplayerManager.Instance.connectedClientIds[0]:
                    currentPlayer = gameManager.whichPlayer.player1;
                    Debug.Log("Current player is player 1: Board Generator");
                    break;

                case ulong clientId when clientId == multiplayerManager.Instance.connectedClientIds[1]:
                    currentPlayer = gameManager.whichPlayer.player2;
                    Debug.Log("Current player is player 2. Board Generator");

                    break;
            }
        }
    } 

    public void callSpawnStones(gameManager.whichPlayer tempPlayer)
    {
        spawnStonesRpc(tempPlayer);
    }

    [Rpc(SendTo.Server)]
    public void spawnStonesRpc(gameManager.whichPlayer tempPlayer)
    {



        for (int i = 0; i < 11; i++)
        {

            GameObject stoneClone;

            Debug.Log("TempPlayer Value == " + tempPlayer);

            var stoneCloneNetworkObject = player1Stone.GetComponent<NetworkObject>();
            stoneClone = Instantiate(player1Stone);
            stoneCloneNetworkObject = stoneClone.GetComponent<NetworkObject>();
            stoneCloneNetworkObject.Spawn();
            stoneCloneNetworkObject.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (i * 2f), 1.5f, 0f);
            stoneCloneNetworkObject.gameObject.name = "Stone " + i + 1;

            if ((i + 1) % 2 ==  0)
            {
                stoneCloneNetworkObject.gameObject.layer = 6;
            } else if ((i + 1) % 2 == 1)
            {
                stoneCloneNetworkObject.gameObject.layer = 7;
            }   

            //switch (tempPlayer) //Spawns different set of stones for each player
            //{
            //    case gameManager.whichPlayer.player1:
            //        Debug.Log("Spawning Stones for player 1");
            //        var stoneCloneNetworkObject = player1Stone.GetComponent<NetworkObject>();
            //        stoneClone = Instantiate(player1Stone);
            //        stoneCloneNetworkObject = stoneClone.GetComponent<NetworkObject>();
            //        stoneCloneNetworkObject.Spawn();
            //        stoneCloneNetworkObject.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (i * 2f), 1.5f, 0f);
            //        stoneCloneNetworkObject.name = "Player 2 Stone " + i + 1;
            //        break;
            //    case gameManager.whichPlayer.player2:
            //        Debug.Log("Spawning Stones for player 2");
            //        var P2stoneCloneNetworkObject = player2Stone.GetComponent<NetworkObject>();
            //        stoneClone = Instantiate(player2Stone);
            //        P2stoneCloneNetworkObject = stoneClone.GetComponent<NetworkObject>();
            //        P2stoneCloneNetworkObject.Spawn();
            //        P2stoneCloneNetworkObject.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (i * 2f), 1.5f, 0f);
            //        P2stoneCloneNetworkObject.name = "Player 2 Stone " + i + 1;
            //        break;
            //}
        }
    }


        //[Rpc(SendTo.ClientsAndHost)]
        //public void spawnStonesOnClientRpc()
        //{

        //    Debug.Log("Your Client Id is: " + multiplayerManager.Instance.connectedClientIds[1]);

        //    if (NetworkManager.Singleton.LocalClientId == multiplayerManager.Instance.connectedClientIds[0])
        //    {

        //        for (int i = 0; i < 5; i++)
        //        {
        //            GameObject stoneClone = Instantiate(player2Stone);
        //            var stoneNetworkObject = stoneClone.GetComponent<NetworkObject>();
        //            stoneNetworkObject.Spawn();

        //            //stoneClone.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (i * 2f), 1.5f, 0f);
        //            //stoneClone.name = "Player 1 Stone: " + (i + 1);
        //        }

        //    }
        //    else if (NetworkManager.Singleton.LocalClientId == multiplayerManager.Instance.connectedClientIds[1])
        //    {
        //        for (int i = 0; i < 5; i++)
        //        {
        //            //var stoneClone = Instantiate(player2Stone);
        //            //stoneClone.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (i * 2f), 1.5f, 0f);
        //            //stoneClone.name = "Player 2 Stone: " + (i + 1);
        //        }
        //    }
        //}


        private void OnEnable()
    {
        //SceneManager.sceneLoaded += onSceneLoaded; // Corrected line
    }

    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= onSceneLoaded; // Corrected line
    }
}
