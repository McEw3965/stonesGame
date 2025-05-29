using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOverManager : NetworkBehaviour
{
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text gameOverTextShadow;


    public NetworkVariable<gameManager.whichPlayer> gameWinner;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameWinner.Value = gameManager.Instance.winner.Value;
        updateTextRpc();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void updateTextRpc()
    {
        gameOverText.text = gameWinner.Value.ToString() + " has won the game";
        gameOverTextShadow.text = gameWinner.Value.ToString() + " has won the game";
    }

    public void mainMenuBtn()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
