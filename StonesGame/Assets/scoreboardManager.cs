using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;

public class scoreboardManager : MonoBehaviour
{
    public static scoreboardManager Instance { get; private set; }

    [SerializeField]
    private Text player1Name;
    [SerializeField]
    private Text player2Name;
    [SerializeField]
    private Text buttonText;

    [SerializeField]
    private GameObject[] player1Points;
    [SerializeField]
    private GameObject[] player2Points;
    [SerializeField]
    private Sprite[] pointSprites;
    [SerializeField]
    private GameObject player1Frame;
    [SerializeField]
    private GameObject player2Frame;

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
    public void revealDisplay()
    {
        switch (gameManager.Instance.playerTorevealFirst)
        {
            case gameManager.whichPlayer.player1:
                player1Name.color = Color.blue;
                player2Name.color = Color.white;
                break;

            case gameManager.whichPlayer.player2:
                player2Name.color = Color.blue;
                player1Name.color = Color.white;
                break;
        }
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

        revealDisplay();
    }
}
