using UnityEngine;

public class tiltController : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private Vector3 targetRotation = new Vector3(0f, 0f, 5f);
    [SerializeField]
    private float rotationSpeed = 1f;

    public float weight;

    private Quaternion targetQuart;

    private bool isTilting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    public void FindTarget(float weight)
    {
        targetRotation.z *= weight;
        targetQuart = Quaternion.Euler(targetRotation);
        isTilting = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (isTilting && transform.rotation != targetQuart)
        {
            Debug.Log("Tilting Seesaw: " + targetQuart);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuart, Time.deltaTime * rotationSpeed);
        } else if (transform.rotation == targetQuart)
        {
            isTilting = false;
            targetQuart = Quaternion.identity;
        }
    }


}
