using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Camera playerCamera;
    
    private bool isUsingMainCamera = true;
    
    void Start()
    {
        // Find cameras if not assigned
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        // Start with main camera active
        SwitchToMainCamera();
    }
    
    void Update()
    {
        // Press C to switch cameras
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCamera();
        }
    }
    
    void ToggleCamera()
    {
        if (isUsingMainCamera)
        {
            SwitchToPlayerCamera();
        }
        else
        {
            SwitchToMainCamera();
        }
    }
    
    void SwitchToMainCamera()
    {
        mainCamera.enabled = true;
        playerCamera.enabled = false;
        isUsingMainCamera = true;
        Debug.Log("Switched to Main Camera");
    }
    
    void SwitchToPlayerCamera()
    {
        mainCamera.enabled = false;
        playerCamera.enabled = true;
        isUsingMainCamera = false;
        Debug.Log("Switched to Player Camera");
    }
    
    // Public methods you can call from other scripts
    public void UseMainCamera()
    {
        SwitchToMainCamera();
    }
    
    public void UsePlayerCamera()
    {
        SwitchToPlayerCamera();
    }
}