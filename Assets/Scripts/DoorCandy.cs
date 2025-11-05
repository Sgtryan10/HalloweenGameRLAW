using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DoorCandy : MonoBehaviour
{
    public static DoorCandy Instance { get; private set; }

    public TextMeshProUGUI counterText;
    public AudioClip doorSound, trickOrTreatSound, pickupSound;
    public List<GameObject> candies;
    public float interactionDistance = 3.0f;
    public static int interactionCount = 0;

    private bool isInteracting = false;
    private AudioSource audioSource;
    private Camera mainCamera;
    private bool hasBeenUsed = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;

        UpdateDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting  && !hasBeenUsed)
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
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(trickOrTreatSound);
        float delayTime = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(delayTime);

        GetCandy();

        hasBeenUsed = true;
        isInteracting = false;
    }

    private void GetCandy()
    {
            int randomIndex = Random.Range(0, candies.Count);
            GameObject selectedCandy = candies[randomIndex];

            Vector3 spawnPosition = transform.position + transform.forward * -0.3f + Vector3.up * 0.2f;
            GameObject spawnedCandy = Instantiate(selectedCandy, spawnPosition, Quaternion.identity);
    }

    public void CollectCandy()
    {
        interactionCount++;
        UpdateDisplay();
        audioSource.PlayOneShot(pickupSound);
    }

    public void LoseCandy()
    {
        interactionCount--;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        counterText.text = "Candy: " + interactionCount.ToString();
    }
}
