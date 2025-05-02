using UnityEngine;
using UnityEngine.UI;

public class buttonAssignment : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {

        this.gameObject.GetComponent<Button>().onClick.AddListener(callEndTurn);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void callEndTurn()
    {
        gameManager.Instance.endTurnRpc();
        Debug.Log("End Turn Called from Button");
    }
}
