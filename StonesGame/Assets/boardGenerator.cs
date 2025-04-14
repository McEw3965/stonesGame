using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using Unity.Netcode;



public class boardGenerator : MonoBehaviour
{
    public GameObject stone;
    public GameObject testStone;

    private Vector3 SpawnPoint = new Vector3(2f, 2f, 0f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            Debug.Log("Multiplayer Scene Loaded");

            stone = GameObject.Find("Stone");
            Transform stoneTransform = stone.GetComponent<Transform>();


            for (int i = 0; i < 6; i++)
            {
                Transform TransformClone = Instantiate(stoneTransform);
                var stoneCloneNetworkObject = TransformClone.GetComponent<NetworkObject>();
                //stoneClone.GetComponent<anchorObject>().anchorOffset = new Vector3(4 + (i * 2f), 1.5f, 0f);
                //stoneClone.name = "Stone " + i + 1;
                stoneCloneNetworkObject.Spawn();
            }
        }
    }

    public void testSpawn()
    {
        Transform stoneTransform = testStone.GetComponent<Transform>();


        for (int i = 0; i < 6; i++)
        {
            Transform TransformClone = Instantiate(stoneTransform);
            var stoneCloneNetworkObject = TransformClone.GetComponent<NetworkObject>();
            //stoneClone.GetComponent<anchorObject>().anchorOffset = new Vector3(4 + (i * 2f), 1.5f, 0f);
            //stoneClone.name = "Stone " + i + 1;
            stoneCloneNetworkObject.Spawn();
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
}
