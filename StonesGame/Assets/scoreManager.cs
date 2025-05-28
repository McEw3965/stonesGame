using UnityEngine;
using Unity.Netcode;
using System.Collections;
using Unity.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

[GenerateSerializationForType(typeof(string))] //Allows strings to be used as networkVariables. Host not started without this
public class scoreManager : NetworkBehaviour
{
    public static scoreManager Instance { get; private set; }
    public NetworkVariable<int> player1Score;
    public NetworkVariable<int> player2Score;

    public int roundNum = 1;

    public gameManager.whichPlayer winner;

    public GameObject test;


    //[SerializeField]
    //private TextMeshPro winnerText;
    //[SerializeField]
    //private TextMeshPro countdown;

    //[SerializeField]
    //public Text winnerText;
    //[SerializeField]
    //public Text countdown;
    //[SerializeField]
    //public Text subheading;

    [SerializeField]
    private NetworkObject winnerText;
    [SerializeField]
    private NetworkObject countdown;

    [SerializeField]
    private NetworkVariable<FixedString128Bytes> winnerString = new NetworkVariable<FixedString128Bytes>("Placeholder");
    [SerializeField]
    private NetworkVariable<FixedString128Bytes> countdownString = new NetworkVariable<FixedString128Bytes>("Placeholder");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void findTextElements()
    {
        //winnerText = GameObject.Find("Winner Text").GetComponent<NetworkObject>();
        //countdown = GameObject.Find("Countdown").GetComponent<NetworkObject>();
        test = GameObject.Find("Canvas");
    }

    private void HandleNetworkSceneEventServer(SceneEvent sceneEvent)
    {
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadEventCompleted:
                if (sceneEvent.SceneName == "MultiplayerScene")
                {
                    //Invoke(nameof(findTextElements), 1f);

                    //winnerText = GameObject.Find("Winner Text");
                    //countdown = GameObject.Find("Countdown");
                }

                break;
        }
    }

    public override void OnNetworkSpawn()
    {
        winnerString.Value = "Placeholder";
        countdownString.Value = "OPlaceholder";
        winnerString.OnValueChanged += updateText;
        countdownString.OnValueChanged += updateCountdown;
    }

    public override void OnNetworkDespawn()
    {
        winnerString.OnValueChanged -= updateText;

    }

    public void updateText(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        scoreboardManager.Instance.winnerText.GetComponent<Text>().text = newValue.ToString();
    }

    public void updateCountdown(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        scoreboardManager.Instance.countdownText.GetComponent<Text>().text = newValue.ToString();
    }

    public void callReloadScene()
    {
        Debug.Log("Inside callReloadScene");
        StartCoroutine(reloadScene());
    }

    private IEnumerator reloadScene()
    {
        Debug.Log("Inside reloadScene");
        Debug.Log("winnerText: " + scoreboardManager.Instance.winnerText);
        Debug.Log("CountdownText: " + scoreboardManager.Instance.countdownText);

        if (scoreboardManager.Instance.winnerText != null || scoreboardManager.Instance.countdownText != null)
        {
            scoreboardManager.Instance.winnerText.gameObject.SetActive(true);
            scoreboardManager.Instance.countdownText.gameObject.SetActive(true);
        } else
        {
            Debug.LogWarning("UI Text is null");
        }

        winnerString.Value = "Player " + winner + " wins round " + roundNum;

        for (int i = 5; i > -1; i--)
        {
            countdownString.Value = i.ToString();
            yield return new WaitForSecondsRealtime(1.0f);
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        NetworkManager.SceneManager.LoadScene(currentSceneName, LoadSceneMode.Single);
    }
}


