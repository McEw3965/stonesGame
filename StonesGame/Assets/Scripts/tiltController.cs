using UnityEngine;
using Unity.Netcode;

public class tiltController : NetworkBehaviour
{
    private Animator animator;

    [SerializeField]
    //private Vector3 targetRotation = new Vector3(0f, 0f, 5f);

    public NetworkVariable<Vector3> targetRotation;
    [SerializeField]
    private float rotationSpeed = 1f;

    public NetworkVariable<float> weight;

    private Quaternion targetQuart;

    [SerializeField]
    public NetworkVariable<bool> isTilting;

    [SerializeField]
    private AudioClip[] creaks;
    [SerializeField]
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRotation.Value = new Vector3(0f, 0f, 3.3f);
        isTilting.Value = false;
    }

    [Rpc(SendTo.Server)]
    public void FindTargetRpc()
    {
        targetRotation.Value = new Vector3 (0f, 0f, 3.3f);
        Vector3 tempRotation = targetRotation.Value;
        tempRotation.z *= weight.Value;
        targetRotation.Value = tempRotation;
        targetQuart = Quaternion.Euler(targetRotation.Value);
        isTilting.Value = true;
        weight.Value = 0f;
        playAudioRpc();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTilting.Value && transform.rotation != targetQuart)
        {
            //Debug.Log("Tilting Seesaw: " + targetQuart);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuart, Time.deltaTime * rotationSpeed);
        } else if (transform.rotation == targetQuart)
        {
            isTilting.Value = false;
            targetQuart = Quaternion.identity;
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void playAudioRpc()
    {
        int randIndex = Random.Range(0, 1);
        audioSource.clip = creaks[randIndex];
        audioSource.Play();
    }

}
