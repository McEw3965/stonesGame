using UnityEngine;
using Unity.Netcode;

public class lightProgression : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject[] lights;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Rpc(SendTo.Server)]
    public void activateLightsRpc()
    {
        Debug.Log("Activate Lights Active");

        for(int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.GetComponent<lightController>().isActive.Value = false;
        }
        float scaleWeight = this.gameObject.GetComponent<interactableObject>().weight.Value;
        Debug.Log("ScaleWeight: " + scaleWeight);
        
        float lightQuantity = scaleWeight / 2;
        Debug.Log("lightQuantity: " + lightQuantity);

        int roundedDownInt = Mathf.FloorToInt(lightQuantity);
        Debug.Log("RoundedDownInt: " + roundedDownInt);

        if (roundedDownInt <= 10)
        {

            for (int i = 0; i < roundedDownInt; i++)
            {
                Debug.Log("Turning on light: " + lights[i].gameObject.name);
                lights[i].gameObject.GetComponent<lightController>().isActive.Value = true;

            }
        } else if (roundedDownInt > 10)
        {
            
        }
    }
}
