using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using mobileInputs;


public class roundManager : NetworkBehaviour
{
    public static roundManager Instance { get; private set; }

    private float currentTime = 5f;
    private Coroutine countdownCoroutine;
    [SerializeField]
    private GameObject winnerText;
    [SerializeField]
    private GameObject winnerShadow;
    [SerializeField]
    private GameObject countdownText;
    [SerializeField]
    private GameObject countdownShadow;
    [SerializeField]
    private GameObject seesaw;
    [SerializeField]
    private GameObject rightScale;
    [SerializeField]
    private GameObject leftScale;

    [SerializeField]
    public List<GameObject> stones;

    public NetworkVariable<int> turnNum;

    public mobileInputActions playerInputs;


    public NetworkVariable<gameManager.whichPlayer> roundWinner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerInputs = new mobileInputActions();    
        Instance = this;
        turnNum = new NetworkVariable<int>(1);
    }
    void Start()
    {
        //Invoke(nameof(findStones), 1f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void findStones()
    {
        GameObject[] stoneObjects = GameObject.FindGameObjectsWithTag("Stone");
        for (int i = 0; i < stoneObjects.Length; i++)
        {
            stones.Add(stoneObjects[i]);
        }
    }

    private IEnumerator countdown()
    {
     //   playerInputs.Mobile.Disable();
        currentTime = 5f;
        Debug.Log("Countdown Running");
        //GameObject winnerText = GameObject.Find("Winner Text");
        //GameObject countdownText = GameObject.Find("Countdown Text");
        disableSceneObjectsRpc();
        winnerText.SetActive(true);
        countdownText.SetActive(true);

        winnerText.GetComponent<Text>().text = roundWinner.Value.ToString() + " has won this round";
        winnerShadow.GetComponent<Text>().text = roundWinner.Value.ToString() + " has won this round";


        while (currentTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log("CurrentTime: " + currentTime);
            currentTime -= 0.1f;

            if (currentTime < 0)
            {
                currentTime = 0;
            }

            updateCountdown();
        }

        //currentTime = 5f;

       // playerInputs.Mobile.Enable();

        seesaw.GetComponent<Transform>().rotation = Quaternion.identity;

        if (IsServer)
        {

            seesaw.GetComponent<tiltController>().isTilting.Value = false;

            turnNum.Value = 1;

            rightScale.GetComponent<interactableObject>().weight.Value = 0;
            leftScale.GetComponent<interactableObject>().weight.Value = 0;
        }

        winnerText.SetActive(false);
        countdownText.SetActive(false);

        //gameManager.Instance.resetVarsRpc();
        
        enableSceneObjectsRpc();
        scoreboardManager.Instance.updateScoreboardRpc();
        yield return null;
    }

    private void updateCountdown()
    {
        int timeToDisplay = Mathf.CeilToInt(currentTime);
        countdownText.GetComponent<Text>().text = timeToDisplay.ToString();
        countdownShadow.GetComponent<Text>().text = timeToDisplay.ToString();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void callCountdownCoroutineRpc()
    {
        countdownCoroutine = StartCoroutine(countdown());
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void disableSceneObjectsRpc()
    {
        //GameObject[] stoneObjects = GameObject.FindGameObjectsWithTag("Stone");
        //GameObject seesaw = GameObject.Find("SeeSaw");
        seesaw.SetActive(false);
        for (int i = 0; i < stones.Count; i++)
        {
            if (stones[i] != null)
            {
                //stones[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //stones[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
                stones[i].gameObject.GetComponent<interactableObject>().isDisabled.Value = false;
                stones[i].gameObject.GetComponent<anchorObject>().enabled = true;
                stones[i].gameObject.GetComponent<interactableObject>().disableAcrossNetworkRpc();

            }
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void enableSceneObjectsRpc()
    {
        //GameObject[] stoneObjects = GameObject.FindGameObjectsWithTag("Stone");
        Debug.Log("StoneObjects length: " + stones.Count);
        seesaw.SetActive(true);
        for (int i = 0; i < stones.Count; i++)
        {
            //stones[i].GetComponent<interactableObject>().isDisabled.Value = false;
            stones[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;
            stones[i].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            stones[i].gameObject.GetComponent<interactableObject>().undoParentingRpc();

            //TEST
            //Debug.Log(stones[i].name + " Initial Position: " + stones[i].GetComponent<interactableObject>().initialPosition);
            stones[i].gameObject.GetComponent<interactableObject>().resetPositionRpc();

            stones[i].gameObject.GetComponent<anchorObject>().enabled = true;

            stones[i].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            Debug.Log("Activating Stone: " + stones[i].name);
            if ((i + 1) % 2 == 0)
            {
                stones[i].GetComponent<interactableObject>().player1ActiveRpc();
            } else
            {
                stones[i].GetComponent<interactableObject>().player2ActiveRpc();
            }
        }
    }

}
