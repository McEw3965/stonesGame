using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;
using TMPro;


public class interactableObject : NetworkBehaviour
{
    //public float weight;
    public NetworkVariable<float> weight;

    private bool isSelected = false;

    private ulong localClientID;
    private Vector3 originalScale;
    private Vector3 focusScale;

    [SerializeField]
    gameManager.whichPlayer currentPlayer;

    public NetworkVariable<bool> isDisabled = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> isPlayed = new NetworkVariable<bool>(false);

    private TextMeshPro weightLabel;



    private void Awake()
    {

        if (this.gameObject.tag == "Scale")
        {
            weight.Value = 10;
        }

        originalScale = this.gameObject.GetComponent<Transform>().localScale;
        focusScale = originalScale + new Vector3(0.3f, 0.3f, 0.3f);

        weightLabel = this.gameObject.GetComponentInChildren<TextMeshPro>();

        ulong localClientId;

        localClientId = NetworkManager.Singleton.LocalClientId;

        switch (localClientId)
        {
            case ulong clientId when clientId == multiplayerManager.Instance.connectedClientIds[0]:
                currentPlayer = gameManager.whichPlayer.player1;
                break;

            case ulong clientId when clientId == multiplayerManager.Instance.connectedClientIds[1]:
                currentPlayer = gameManager.whichPlayer.player2;

                break;
        }

    }

    private void disableStone(bool previousValue, bool newValue)
    {
        this.gameObject.SetActive(false);
    }

    private void playStone(bool previousValue, bool newValue)
    {
        this.gameObject.GetComponent<Animator>().SetBool("Reveal?", true);
    }
    public override void OnNetworkSpawn()
    {
        isDisabled.OnValueChanged += disableStone;
        isPlayed.OnValueChanged += playStone;
        weight.OnValueChanged += updateWeightLabel;
    }

    public override void OnNetworkDespawn()
    {
        isDisabled.OnValueChanged -= disableStone;
        isPlayed.OnValueChanged -= playStone;
        weight.OnValueChanged -= updateWeightLabel;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        focusEffect();

    }

    [Rpc(SendTo.ClientsAndHost)]
    public void player1ActiveRpc()
    {

        if (currentPlayer == gameManager.whichPlayer.player1)
        {
            Debug.Log("Enabling Stone for player 1. CurrentPlayer: " + currentPlayer);

            this.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Disabling Stone for player 1. CurrentPlayer: " + currentPlayer);

            this.gameObject.SetActive(false);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void player2ActiveRpc()
    {

        if (currentPlayer == gameManager.whichPlayer.player2)
        {
            Debug.Log("Enabling Stone for player 2. CurrentPlayer: " + currentPlayer);

            this.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Disabling Stone for player 2. CurrentPlayer: " + currentPlayer);
            this.gameObject.SetActive(false);
        }
    }

    //TEXT UPDATING

    private void updateWeightLabel(float previousValue, float newValue)
    {
        if (weight != null && this.tag == "Stone")
        {
            weightLabel.text = weight.Value.ToString();
        } else
        {
            Debug.LogWarning("Cannot update label to null");
        }
    }

    //TOUCHSCREEN CONTROLS (NEW INPUT SYSTEM)

    [Rpc(SendTo.ClientsAndHost)]
    private void disableStoneRpc()
    {
        if (currentPlayer == gameManager.whichPlayer.player1 && this.gameObject.layer == 6)
        {
            this.name = "Player 1 Stone";
            this.gameObject.SetActive(false);
        }
        else if (currentPlayer == gameManager.whichPlayer.player2 && this.gameObject.layer == 7)
        {
            this.name = "Player 2 Stone";
            this.gameObject.SetActive(false);
        }
    }


    [Rpc(SendTo.Server)]
    private void assignPlayer1ObjectsRpc()
    {
        if (this.tag == "Stone")
        {
            gameManager.Instance.player1SelectedStone = this.gameObject.GetComponent<NetworkObject>();
            Debug.Log("Player 1 Stone assigned");

        }
        else if (this.tag == "Scale")
        {
            gameManager.Instance.player1SelectedScale = this.gameObject.GetComponent<NetworkObject>();
            Debug.Log("Player 1 Scale assigned");

        }

    }

    [Rpc(SendTo.Server)]
    private void assignPlayer2ObjectsRpc()
    {
        if (this.tag == "Stone")
        {
            gameManager.Instance.player2SelectedStone = this.gameObject.GetComponent<NetworkObject>();
            Debug.Log("Player 2 stone assigned");
        }
        else if (this.tag == "Scale")
        {
            gameManager.Instance.player2SelectedScale = this.gameObject.GetComponent<NetworkObject>();
            Debug.Log("Player 2 Scale assigned");

        }
    }

    private void callAddWeight()
    {
        gameManager.Instance.addWeightRpc();
    }

    private void OnMouseOver()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    switch (currentPlayer)
        //    {
        //        case gameManager.whichPlayer.player1:
        //            assignPlayer1ObjectsRpc();
        //            break;

        //        case gameManager.whichPlayer.player2:
        //            assignPlayer2ObjectsRpc();
        //            break;
        //    }
        //}
    }

    public void handleTap()
    {
        switch (currentPlayer)
        {
            case gameManager.whichPlayer.player1:
                assignPlayer1ObjectsRpc();
                break;

            case gameManager.whichPlayer.player2:
                assignPlayer2ObjectsRpc();
                break;
        }
    }

    private void focusEffect()
    {

        if (isSelected)
        {
            this.GetComponent<Transform>().localScale = originalScale + new Vector3(0.3f, 0.3f, 0.3f);
        }
        else
        {
            this.GetComponent<Transform>().localScale = originalScale;
        }
    }
}
