using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;

public class scoreboardManager : MonoBehaviour
{
    public static scoreboardManager Instance { get; private set; }
    [SerializeField]
    public Text winnerText;
    [SerializeField]
    public Text countdownText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        winnerText = GameObject.Find("Winner Text").GetComponent<Text>();
        countdownText= GameObject.Find("Countdown Text").GetComponent<Text>();
        Instance = this;


    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
