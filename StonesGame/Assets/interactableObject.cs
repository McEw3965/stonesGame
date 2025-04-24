using UnityEngine;
using Unity.Netcode;

public class interactableObject : MonoBehaviour
{
    public float weight;

    private bool isSelected = false;

    private ulong localClientID;
    private Vector3 originalScale;
    private Vector3 focusScale;

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
                Debug.Log("Current player is player 1: Board Generator");
                break;

            case ulong clientId when clientId == multiplayerManager.Instance.connectedClientIds[1]:
                currentPlayer = gameManager.whichPlayer.player2;
                Debug.Log("Current player is player 2. Board Generator");

                break;
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

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentPlayer)
            {
                case gameManager.whichPlayer.player1:
                    if(this.tag == "Stone")
                    {
                        gameManager.Instance.player1SelectedStone = this.gameObject.GetComponent<NetworkObject>();
                    } else if (this.tag == "Scale")
                    {
                        gameManager.Instance.player1SelectedScale = this.gameObject.GetComponent<NetworkObject>();
                    }
                    break;

                case gameManager.whichPlayer.player2:
                    if (this.tag == "Stone")
                    {
                        gameManager.Instance.player2SelectedStone = this.gameObject.GetComponent<NetworkObject>();
                    }
                    else if (this.tag == "Scale")
                    {
                        gameManager.Instance.player2SelectedScale = this.gameObject.GetComponent<NetworkObject>();
                    }
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
