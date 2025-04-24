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
    [SerializeField]
    private GameObject Stone;

    [SerializeField]
    private List<GameObject> stoneClones;
    private int numOfStones = 0;

    [SerializeField]
    private Sprite[] stoneSprites;

    private Vector3 ClickPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickPos = Input.mousePosition;
            ClickPos.z = 10f;
            Vector3 spawnPoint = Camera.main.ScreenToWorldPoint(ClickPos);
            Debug.Log("Mouse clicked at: " + ClickPos);
            numOfStones++;
            GameObject stoneClone = Instantiate(Stone, spawnPoint, Quaternion.identity);
            int spriteIndex = Random.Range(0, stoneSprites.Length);
            stoneClone.GetComponent<SpriteRenderer>().sprite = stoneSprites[0];
            stoneClones.Add(stoneClone);

            if (numOfStones > 8)
            {
                Destroy(stoneClones[numOfStones - 1]);
                stoneClones.Remove(stoneClones[numOfStones -1]);
                numOfStones--;
            }
        }
    }

    public void multiplayerMenu()
    {
        sessionCode.SetActive(true);
        joinSession.SetActive(true);
        createSession.SetActive(true);
        playerList.SetActive(true);
        multiplayerButton.SetActive(false);
        title.SetActive(false);
    }
}
