using UnityEngine;
using Unity.Netcode;

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
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        originalScale = this.gameObject.GetComponent<Transform>().localScale;
        focusScale = originalScale + new Vector3(0.3f, 0.3f, 0.3f);

    }
    void Start()
    {
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

        if (currentPlayer == gameManager.whichPlayer.player1 && this.gameObject.layer == 6)
        {
            this.gameObject.SetActive(false);
        } else if (currentPlayer == gameManager.whichPlayer.player2 && this.gameObject.layer == 7)
        {
            this.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        focusEffect();

    }

    //private void OnMouseOver()
    //{
    //    localClientID = NetworkManager.Singleton.LocalClientId;
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (this.tag == "Stone")
    //        {

    //            gameManager.Instance.selectedStone = this.gameObject;
    //            //gameManager.Instance.clientIdToStone[localClientID] = this.gameObject;
    //        }
    //        else if (this.tag == "Scale")
    //        {
    //            gameManager.Instance.selectedScale = this.gameObject;
    //            //gameManager.Instance.clientIdToScale[localClientID] = this.gameObject;


    //        }

    //        if(isSelected)
    //        {
    //            isSelected = false;
    //        } else if (!isSelected)
    //        {
    //            isSelected = true;
    //        }
    //    }
    //}

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
        if (Input.GetMouseButtonDown(0))
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
