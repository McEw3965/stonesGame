using UnityEngine;
using Unity.Netcode;

public class networkSpriteChanger : NetworkBehaviour
{
    [SerializeField]
    private Sprite[] stoneSprites;

    public NetworkVariable<int> initialSpriteIndex = new NetworkVariable<int>();


    //public NetworkVariable<int> initialSpriteIndex = new NetworkVariable<int>(
    //    0,
    //    NetworkVariableReadPermission.Everyone, //Everyone can read var
    //    NetworkVariableWritePermission.Server //Only server can write value
    //    );

    private SpriteRenderer spriteRendComp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        spriteRendComp = GetComponent<SpriteRenderer>();
        initialSpriteIndex.Value = 1;
    }

    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        //changeSprite(initialSpriteIndex.Value);
    }

    // Update is called once per frame
    void Update()
    {
            changeSprite(initialSpriteIndex.Value);

    }

    private void changeSprite(int index)
    {
        if (spriteRendComp != null && stoneSprites != null && index >= 0 && index < stoneSprites.Length)
        {
            spriteRendComp.sprite = stoneSprites[index];
        } else
        {
            Debug.LogWarning("Invalid sprite index: " + index + " or missing setup for " + this.gameObject.name);
        }
    }

    public void SetSpriteOnSpawn(int index)
    {
        initialSpriteIndex.Value = index;

    }

    //public void SetSpriteOnSpawn(int index)
    //{
    //    Debug.Log("Index in SetSpriteOnSpawn: " + index);
    //    if (IsHost)
    //    {
    //        Debug.Log("isServer clause running");
    //        initialSpriteIndex.Value = index;
    //    } else
    //    {
    //        Debug.LogWarning("SetSpriteOnSpawn only called on server");
    //    }
    //}
}
