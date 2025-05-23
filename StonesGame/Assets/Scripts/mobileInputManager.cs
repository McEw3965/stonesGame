using UnityEngine;
using UnityEngine.InputSystem;
using mobileInputs;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;

public class mobileInputManager : MonoBehaviour
{
    private mobileInputActions mobileInputs;
    private Camera MainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        mobileInputs = new mobileInputActions();
        MainCamera = Camera.main;
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
            MainCamera = Camera.main;
        
    }

    private void OnEnable()
    {
        mobileInputs.Mobile.Enable();
        mobileInputs.Mobile.Tap.performed += OnTapPerformed;
    }

    private void OnDisable()
    {
        mobileInputs.Mobile.Tap.performed -= OnTapPerformed;
        mobileInputs.Mobile.Disable();
    }

    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("OnTapPerformed running");
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
        Vector2 touchPosition = context.ReadValue<Vector2>();
        Ray ray = MainCamera.ScreenPointToRay(touchPosition);
        //RaycastHit hit;

        Vector3 worldPosition3D = MainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, MainCamera.nearClipPlane));
        Vector2 worldPosition2D = new Vector2(worldPosition3D.x, worldPosition3D.y);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition2D, Vector2.zero);
        {
            Debug.Log("Object Hit");

            interactableObject interactable;

            if (hit.collider.TryGetComponent<interactableObject>(out interactableObject interactableObject) == true)
            {
                interactable = hit.collider.GetComponent<interactableObject>();
            } else
            {
                interactable = null;
            }


            if (interactable != null)
            {
                Debug.Log("Object Tapped: " + interactable.gameObject.name);
                interactable.handleTap();
            } else
            {
                Debug.Log("Hit collider does not contain InteractableObject");
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
