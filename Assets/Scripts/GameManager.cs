using UnityEngine;
using UnityEngine.UI;

public class CameraAndCursorManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera playerCamera;
    public Button switchButton;
    
    private bool isUsingMainCamera = true;
    
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        SwitchToMainCamera();
        
        // Add button listener
        if (switchButton != null)
            switchButton.onClick.AddListener(ToggleCamera);
    }
    
    void Update()
    {
        // Press X to unlock cursor
        if (Input.GetKeyDown(KeyCode.X))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        // Press ESC to unlock cursor (common pattern)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    void ToggleCamera()
    {
        if (isUsingMainCamera)
        {
            mainCamera.enabled = false;
            playerCamera.enabled = true;
            isUsingMainCamera = false;
        }
        else
        {
            mainCamera.enabled = true;
            playerCamera.enabled = false;
            isUsingMainCamera = true;
        }
        
        // Hide and lock cursor after switching camera
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void SwitchToMainCamera()
    {
        mainCamera.enabled = true;
        playerCamera.enabled = false;
        isUsingMainCamera = true;
    }
}