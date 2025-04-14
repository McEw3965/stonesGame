using UnityEngine;

public class interactableObject : MonoBehaviour
{
    public float weight;

    private bool isSelected = false;

    private Vector3 originalScale;
    private Vector3 focusScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        originalScale = this.gameObject.GetComponent<Transform>().localScale;
        focusScale = originalScale + new Vector3(0.3f, 0.3f, 0.3f);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        focusEffect();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (this.tag == "Stone")
            {
                gameManager.Instance.selectedStone = this.gameObject;
            }
            else if (this.tag == "Scale")
            {
                gameManager.Instance.selectedScale = this.gameObject;

            }
        }
    }


    private void focusEffect()
    {

        if (this.gameObject == gameManager.Instance.selectedStone || this.gameObject == gameManager.Instance.selectedScale)
        {
            this.GetComponent<Transform>().localScale = originalScale + new Vector3(0.3f, 0.3f, 0.3f);
        }
        else
        {
            this.GetComponent<Transform>().localScale = originalScale;
        }
    }
}
