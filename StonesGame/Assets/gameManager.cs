using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Net.Sockets;

public class gameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static gameManager Instance { get; private set; }

    //VARIABLES FOR STONES AND SCALES
    [SerializeField]
    private GameObject leftScale;
    private GameObject rightScale;

    public GameObject selectedStone;
    public GameObject selectedScale;

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

    //GAME EVENTS

    private void addWeightToScale()
    {
        Debug.Log("Adding Weight");
        selectedScale.GetComponent<interactableObject>().weight += selectedStone.GetComponent<interactableObject>().weight;
        Destroy(selectedStone);
        selectedStone = null;
        selectedScale = null;
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
        } else
        {
            Debug.Log("Next Turn");
        }
    }

    public void endTurn()
    {
        addWeightToScale();
        calculateDifference();
    }

    //SCENE MANAGEMENT
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MultiplayerScene")
        {
            Debug.Log("Multiplayer Scene Loaded");
            leftScale = GameObject.Find("Left Scale");
            rightScale = GameObject.Find("Right Scale");
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
