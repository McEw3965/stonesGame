using UnityEngine;

public class interactableObject : MonoBehaviour
{
    [SerializeField]
    public float weight;

    private bool isSelected = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            focusEffect();
        }
    }

    private void focusEffect()
    {
        if (!isSelected)
        {
            this.GetComponent<Transform>().localScale += new Vector3 (0.3f, 0.3f, 0.3f);
            isSelected = true;
        } else if (isSelected)
        {
            this.GetComponent<Transform>().localScale -= new Vector3(0.3f, 0.3f, 0.3f);
            isSelected=false;

        }
    }
}
