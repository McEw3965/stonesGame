using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using Unity.Netcode;



public class boardGenerator : NetworkBehaviour
{

    public static boardGenerator Instance { get; private set; }

    public GameObject player1Stone;
    public GameObject player2Stone;

    [SerializeField]
    private Sprite[] stoneSprites;

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
        float offsetAdjustment = 0f;
        float stoneWeight = 0f;
        int stoneIndex = 0;

        for (int i = 0; i < 10; i++)
        {

            GameObject stoneClone;
            stoneClone = Instantiate(player1Stone);

            Debug.Log("Stone Index before calliing setSpriteOnSpawn: " + stoneIndex);
            var stoneCloneNetworkObject = stoneClone.GetComponent<NetworkObject>();
            stoneCloneNetworkObject.gameObject.GetComponent<networkSpriteChanger>().SetSpriteOnSpawn(stoneIndex);
            stoneCloneNetworkObject.gameObject.name = "Stone " + i + 1;
            stoneCloneNetworkObject.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (offsetAdjustment), 1.5f, 0f);
            stoneCloneNetworkObject.Spawn();
            stoneCloneNetworkObject.GetComponent<interactableObject>().weight.Value = stoneWeight;


            if ((i + 1) % 2 == 0)
            {
                stoneWeight++;
                offsetAdjustment += 2f;
                stoneIndex++;
                stoneCloneNetworkObject.gameObject.GetComponent<interactableObject>().player1ActiveRpc();
            } else
            {
                stoneCloneNetworkObject.gameObject.GetComponent<interactableObject>().player2ActiveRpc();
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
