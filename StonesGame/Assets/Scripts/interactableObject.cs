using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;


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

    [SerializeField]
    private Transform seeSaw;

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

    [Rpc(SendTo.ClientsAndHost)]
    public void parentToSeesawRpc()
    {
        gameObject.GetComponent<Transform>().SetParent(seeSaw);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void undoParentingRpc()
    {
        gameObject.GetComponent<Transform>().SetParent(null);
    }

    private void disableStone(bool previousValue, bool newValue)
    {
        this.gameObject.SetActive(false);
    }

    private void playStone(bool previousValue, bool newValue)
    {
        //this.gameObject.GetComponent<Animator>().SetBool("Reveal?", true);
    }
    public override void OnNetworkSpawn()
    {
        //isDisabled.OnValueChanged += disableStone;
        isPlayed.OnValueChanged += playStone;
        weight.OnValueChanged += updateWeightLabel;
    }

    public override void OnNetworkDespawn()
    {
        //isDisabled.OnValueChanged -= disableStone;
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

            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            this.gameObject.GetComponentInChildren<TextMeshPro>().enabled = true;

        }
        else
        {
            Debug.Log("Disabling Stone for player 1. CurrentPlayer: " + currentPlayer);

            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponentInChildren<TextMeshPro>().enabled = false;

        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void player2ActiveRpc()
    {

        if (currentPlayer == gameManager.whichPlayer.player2)
        {
            Debug.Log("Enabling Stone for player 2. CurrentPlayer: " + currentPlayer);

            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            this.gameObject.GetComponentInChildren<TextMeshPro>().enabled = true;
        }
        else
        {
            Debug.Log("Disabling Stone for player 2. CurrentPlayer: " + currentPlayer);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponentInChildren<TextMeshPro>().enabled = false;

        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void toggleAcrossNetworkRpc()
    {
        if (isDisabled.Value == false)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            this.gameObject.GetComponentInChildren<TextMeshPro>().enabled = true;
            if (IsServer)
            {
                isDisabled.Value = true;
            }
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponentInChildren<TextMeshPro>().enabled = false;
            if (IsServer)
            {
                isDisabled.Value = false;
            }
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void enableAcrossNetworkRpc()
    {

        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        this.gameObject.GetComponentInChildren<TextMeshPro>().enabled = true;
        if (IsServer)
        {
            isDisabled.Value = true;
        }

    }

    [Rpc(SendTo.ClientsAndHost)]
    public void disableAcrossNetworkRpc()
    {

        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponentInChildren<TextMeshPro>().enabled = false;
        if (IsServer)
        {
            isDisabled.Value = false;
        }

    }

    //TEXT UPDATING

    private void updateWeightLabel(float previousValue, float newValue)
    {
        if (weight != null && this.tag == "Stone")
        {
            weightLabel.text = weight.Value.ToString();
        }
        else
        {
            Debug.LogWarning("Cannot update label to null");
        }
    }


    [Rpc(SendTo.Server)]
    private void assignPlayer1ObjectsRpc()
    {
        if (gameManager.Instance.player1SelectedStone?.gameObject != null)
        {
            gameManager.Instance.player1SelectedStone.gameObject.GetComponent<anchorObject>().enabled = true;
            gameManager.Instance.player1SelectedStone.gameObject.GetComponent<interactableObject>().undoParentingRpc();
        }
        if (this.tag == "Stone")
        {
            //gameManager.Instance.selectedStone = this.gameObject;
            gameManager.Instance.player1SelectedStone = this.gameObject.GetComponent<NetworkObject>();
            Debug.Log("Player 1 Stone assigned");

        }
        else if (this.tag == "Scale")
        {
            gameManager.Instance.player1SelectedScale = this.gameObject.GetComponent<NetworkObject>();
            Debug.Log("Player 1 Scale assigned");


            if (gameManager.Instance.player1SelectedStone != null && gameManager.Instance.player1SelectedScale != null)
            {
                stoneInSlot(gameManager.Instance.player1SelectedStone.gameObject, gameManager.Instance.player1SelectedScale.gameObject);
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void assignPlayer2ObjectsRpc()
    {
        if (gameManager.Instance.player2SelectedStone?.gameObject != null)
        {
            gameManager.Instance.player2SelectedStone.gameObject.GetComponent<anchorObject>().enabled = true;
        }

        if (this.tag == "Stone")
        {
            gameManager.Instance.player2SelectedStone = this.gameObject.GetComponent<NetworkObject>();
            gameManager.Instance.player2Stone.Value = true;
            Debug.Log("Player 2 stone assigned");
        }
        else if (this.tag == "Scale")
        {
            gameManager.Instance.player2SelectedScale = this.gameObject.GetComponent<NetworkObject>();
            gameManager.Instance.player2Scale.Value = true;
            Debug.Log("Player 2 Scale assigned");
        }

        if (gameManager.Instance.player2SelectedStone != null && gameManager.Instance.player2SelectedScale != null)
        {
            stoneInSlot(gameManager.Instance.player2SelectedStone.gameObject, gameManager.Instance.player2SelectedScale.gameObject);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void toggleStonesRpc()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void stoneInSlot(GameObject stone, GameObject scale)
    {
        if (stone != null && scale != null) //Change to player1 and player2 selected objects instead! Will need to enable both stones on both screens during play
        {
            if (stone.GetComponent<anchorObject>().enabled == true)
            {
                stone.GetComponent<anchorObject>().enabled = false;
            }
            parentToSeesawRpc();
            stone.GetComponent<Transform>().position = scale.transform.position;
        }
    }

    private void callAddWeight()
    {
        gameManager.Instance.addWeightRpc();
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

        buttonAssignment.Instance.updateText();
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
