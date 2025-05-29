using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;

public class scoreboardManager : MonoBehaviour
{
    public static scoreboardManager Instance { get; private set; }


    [SerializeField]
    private GameObject[] player1Points;
    [SerializeField]
    private GameObject[] player2Points;
    [SerializeField]
    private Sprite[] pointSprites;

    public NetworkVariable<int> player1Score;
    public NetworkVariable<int> player2Score;
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

    [Rpc(SendTo.ClientsAndHost)]
    public void updateScoreboardRpc()
    {
        Debug.Log("Updating scoreboard");
        for (int i = 0; i < player1Score.Value; i++)
        {
            player1Points[i].GetComponent<Image>().sprite = pointSprites[1];
        }

        for (int i = 0; i < player2Score.Value; i++)
        {
            player2Points[i].GetComponent<Image>().sprite = pointSprites[1];
        }
    }
}
