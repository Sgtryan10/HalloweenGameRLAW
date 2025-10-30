using UnityEngine;
using UnityEngine.UI;

public class CameraSwticher : MonoBehaviour
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
    }
    
    void SwitchToMainCamera()
    {
        mainCamera.enabled = true;
        playerCamera.enabled = false;
        isUsingMainCamera = true;
    }
}