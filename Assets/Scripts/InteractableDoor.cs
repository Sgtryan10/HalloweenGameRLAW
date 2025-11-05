using UnityEngine;
public class InteractableDoor : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private Camera mainCamera;

    public AudioClip openSound;
    public AudioClip closeSound;
    public float interactionDistance = 3.0f;

    private bool isOpen = false;
    private const string IS_OPEN_PARAM = "IsOpen";

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
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
                    ToggleDoor();
                }
            }
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            audioSource.PlayOneShot(openSound);
        }
        else
        {
            audioSource.PlayOneShot(closeSound);
        }

        animator.SetBool(IS_OPEN_PARAM, isOpen);
    }
}

