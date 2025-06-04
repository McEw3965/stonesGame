using UnityEngine;
using Unity.Netcode;

public class lightController : NetworkBehaviour
{

    public NetworkVariable<bool> isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enableLight(bool previousValue, bool newValue)
    {
        Debug.Log("enableLight running");
        this.gameObject.GetComponent<SpriteRenderer>().enabled = newValue;
    }

    public override void OnNetworkSpawn()
    {
        isActive.OnValueChanged += enableLight;
    }

    public override void OnNetworkDespawn()
    {
        isActive.OnValueChanged -= enableLight;
    }
}
