using UnityEngine;
using UnityEngine.UI;

public class buttonAssignment : MonoBehaviour
{
    public static buttonAssignment Instance { get; private set; }
    public string BtnText = "End Turn";

    public bool stoneSelected = false;
    public bool scaleSelected = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
        this.gameObject.GetComponent<Button>().onClick.AddListener(callEndTurn);

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    //public void updateText()
    //{
    //    Text[] BtnTextComp = this.GetComponentsInChildren<Text>();


    //    if (!stoneSelected && !scaleSelected || scaleSelected && !stoneSelected)
    //    {
    //        BtnTextComp[0].text = "Select a stone";
    //        BtnTextComp[1].text = "Select a stone";
    //    }
    //    else if (!scaleSelected && stoneSelected)
    //    {
    //        BtnTextComp[0].text = "Selet a side";
    //        BtnTextComp[1].text = "Selet a side";
    //    }
    //    else if (stoneSelected && scaleSelected)
    //    {
    //        BtnTextComp[0].text = "End Turn";
    //        BtnTextComp[1].text = "End Turn";
    //    }

    //}


    public void updateText()
    {
        Text[] BtnTextComp = this.GetComponentsInChildren<Text>();


        switch (gameManager.Instance.currentPlayer)
        {
            case gameManager.whichPlayer.player1:
                if (gameManager.Instance.player1SelectedStone == null)
                {
                    BtnTextComp[0].text = "Select a stone";
                    BtnTextComp[1].text = "Select a stone";
                }
                else if (gameManager.Instance.player1SelectedScale == null)
                {
                    BtnTextComp[0].text = "Selet a side";
                    BtnTextComp[1].text = "Selet a side";
                }
                else if (gameManager.Instance.player1SelectedScale != null && gameManager.Instance.player1SelectedStone != null)
                {
                    BtnTextComp[0].text = "End Turn";
                    BtnTextComp[1].text = "End Turn";
                }
                break;

            case gameManager.whichPlayer.player2:
                Debug.Log("Player2 Stone Selected: " + gameManager.Instance.player2Stone.Value);
                Debug.Log("Player2 Scale Selected: " + gameManager.Instance.player2Scale.Value);

                if (gameManager.Instance.player2Stone.Value == false && gameManager.Instance.player2Scale.Value == false || gameManager.Instance.player2Stone.Value == false && gameManager.Instance.player2Scale.Value == true)
                {
                    BtnTextComp[0].text = "Select a stone";
                    BtnTextComp[1].text = "Select a stone";
                }
                else if (gameManager.Instance.player2Scale.Value == false)
                {
                    BtnTextComp[0].text = "Selet a side";
                    BtnTextComp[1].text = "Selet a side";
                }
                else if (gameManager.Instance.player2Scale.Value == true && gameManager.Instance.player2Stone.Value == true)
                {
                    BtnTextComp[0].text = "End Turn";
                    BtnTextComp[1].text = "End Turn";
                }
                break;
        }
    }
    private void callEndTurn()
    {
        gameManager.Instance.callEndTurn();
    }
}
