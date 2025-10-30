using UnityEngine;

public class CursorToggle : MonoBehaviour
{
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
}