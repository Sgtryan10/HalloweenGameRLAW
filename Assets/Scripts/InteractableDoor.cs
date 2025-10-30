using UnityEngine;
public class InteractableDoor : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private const string IS_OPEN_PARAM = "IsOpen";

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        ToggleDoor();
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
