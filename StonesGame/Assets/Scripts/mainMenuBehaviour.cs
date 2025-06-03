using UnityEngine;
using System.Collections.Generic;

public class mainMenuBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static mainMenuBehaviour Instance { get; private set; }

    [SerializeField]
    private GameObject createSession;
    [SerializeField]
    private GameObject playerList;
    [SerializeField]
    private GameObject joinSession;
    [SerializeField]
    private GameObject sessionCode;
    [SerializeField]
    private GameObject title;
    [SerializeField]
    private GameObject multiplayerButton;

    private Vector3 ClickPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void multiplayerMenu()
    {
        sessionCode.SetActive(true);
        joinSession.SetActive(true);
        createSession.SetActive(true);
        //playerList.SetActive(true);
        multiplayerButton.SetActive(false);
        //title.SetActive(false);
    }
}
