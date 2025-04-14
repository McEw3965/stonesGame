using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;



public class boardGenerator : MonoBehaviour
{
    public GameObject stone;
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

            for (int i = 0; i < 6; i++)
            {
                GameObject stoneClone = Instantiate(stone);
                stoneClone.GetComponent<anchorObject>().anchorOffset = new Vector3(4 + (i * 2f), 1.5f, 0f);
                stoneClone.name = "Stone " + i + 1;
            }
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
