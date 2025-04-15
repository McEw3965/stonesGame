using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using Unity.Netcode;



public class boardGenerator : NetworkBehaviour
{

    public static boardGenerator Instance { get; private set; }

    public GameObject stone;
    public GameObject testStone;

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
    /*private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MultiplayerScene")
        {
            Debug.Log("Multiplayer Scene Loaded");

            stone = testStone;
            Transform stoneTransform = stone.GetComponent<Transform>();


            for (int i = 0; i < 6; i++)
            {
                Transform TransformClone = Instantiate(stoneTransform);
                var stoneCloneNetworkObject = TransformClone.GetComponent<NetworkObject>();
                TransformClone.GetComponent<anchorObject>().anchorOffset = new Vector3(4 + (i * 2f), 1.5f, 0f);
                TransformClone.name = "Stone " + i + 1;
                stoneCloneNetworkObject.Spawn();
            }
        }
    } */

    [Rpc(SendTo.Server)]
    public void SpawnStonesRpc()
    {
        Debug.Log("Spawn Stones RPC Running...");
        //GameObject stone = GameObject.Find("Stone");

        for (int i = 0; i < 6; i++)
        {
            GameObject stoneClone = Instantiate(stone);
            var stoneCloneNetworkObject = stoneClone.GetComponent<NetworkObject>();
            stoneCloneNetworkObject.Spawn();
            stoneCloneNetworkObject.GetComponent<anchorObject>().anchorOffset.Value = new Vector3(4 + (i * 2f), 1.5f, 0f);
            stoneCloneNetworkObject.name = "Stone " + i + 1;
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
