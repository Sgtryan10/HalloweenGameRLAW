using UnityEngine;
using UnityEngine.SceneManagement;

public class Extract : MonoBehaviour
{
    public int requiredCandy = 7;
    public float interactionDistance = 2f;

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            extractGo();
        }
    }

    private void extractGo()
    {
        int currentCandy = DoorCandy.interactionCount;

        if (currentCandy >= requiredCandy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOverGood");
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOverBad");
        }
    }
}
