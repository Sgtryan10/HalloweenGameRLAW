using UnityEngine;
using TMPro;
using System.Collections;

public class DoorCandy : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public AudioClip doorSound;
    public float interactionDistance = 3.0f;

    private int interactionCount = 0;
    private bool isInteracting = false;
    private AudioSource audioSource;
    private Camera mainCamera;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;

        UpdateDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
                Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

                if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        StartCoroutine(DelayedInteraction());
                    }
                }
        }
    }

    private IEnumerator DelayedInteraction()
    {
        isInteracting = true;
        audioSource.PlayOneShot(doorSound);
        float delayTime = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(delayTime);

        GetCandy();

        isInteracting = false;
    }

    private void GetCandy()
    {
        interactionCount++;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        counterText.text = "Candy: " + interactionCount.ToString();
    }
}
