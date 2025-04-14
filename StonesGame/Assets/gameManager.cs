using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Net.Sockets;

public class gameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static gameManager Instance { get; private set; }

    //VARIABLES FOR STONES AND SCALES
    private GameObject leftScale;
    private GameObject rightScale;
    private GameObject selectedStone { get; set; }
    private GameObject selectedScale { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);

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

    //SCENE MANAGEMENT
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MultiplayerScene")
        {
            Debug.Log("Multiplayer Scene Loaded");
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
