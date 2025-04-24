using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using Unity.Netcode;



public class boardGenerator : NetworkBehaviour
{

    public static boardGenerator Instance { get; private set; }

    public GameObject stone;
    public GameObject testStone;

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

    [Rpc(SendTo.Server)]
    public void SpawnStonesRpc()
    {
        Debug.Log("Spawn Stones RPC Running...");
        //GameObject stone = GameObject.Find("Stone");

        for (int i = 0; i < 6; i++)
        {
            GameObject stoneClone = Instantiate(stone);
            var stoneCloneNetworkObject = stoneClone.GetComponent<NetworkObject>();

            //SPAWN ON NETWORK
            //stoneCloneNetworkObject.Spawn();
            //stoneCloneNetworkObject.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (i * 2f), 1.5f, 0f);
            //stoneCloneNetworkObject.name = "Stone " + i + 1;

            switch(currentPlayer) //Spawns different set of stones for each player
            {
                case gameManager.whichPlayer.player1:
                    stoneCloneNetworkObject.Spawn();
                    stoneCloneNetworkObject.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (i * 2f), 1.5f, 0f);
                    stoneCloneNetworkObject.name = "Stone " + i + 1;
                    break;

                case gameManager.whichPlayer.player2:
                    stoneCloneNetworkObject.Spawn();
                    stoneCloneNetworkObject.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (i * 2f), 1.5f, 0f);
                    stoneCloneNetworkObject.name = "Stone " + i + 1;
                    break;
            }
        }
    }


    private void OnEnable()
    {
        //SceneManager.sceneLoaded += onSceneLoaded; // Corrected line
    }

    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= onSceneLoaded; // Corrected line
    }
}
