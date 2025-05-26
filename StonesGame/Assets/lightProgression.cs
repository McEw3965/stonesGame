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

    public void activateLights()
    {
        Debug.Log("Activate Lights Active");

        for(int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.SetActive(false);
        }
        float weight = 0;
        float scaleWeight = this.gameObject.GetComponent<interactableObject>().weight.Value;

        if (scaleWeight < 0) //If weight is negative, convert to positive value
        {
            weight -= scaleWeight;
        } else if (scaleWeight >= 0)
        {
            weight += scaleWeight;
        }
        
        int lightIndex = 0;

        for (int i = 0; i < weight; i++)
        {
            if((i + 1) % 2 == 0 && lightIndex < lights.Length)
            {
                lights[lightIndex].gameObject.SetActive(true);
                lightIndex++;
            }
        }
    }
}
