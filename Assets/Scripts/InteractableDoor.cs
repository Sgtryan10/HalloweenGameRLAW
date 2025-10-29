using UnityEngine;
public class InteractableDoor : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;

    public AudioClip openSound;
    public AudioClip closeSound;

    private bool _isOpen = false;
    private const string IS_OPEN_PARAM = "IsOpen";

    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {

        _isOpen = !_isOpen;

        if (_isOpen)
        {
            if (openSound != null)
            {
                _audioSource.PlayOneShot(openSound);
            }
        }
        else
        {
            if (closeSound != null)
            {
                _audioSource.PlayOneShot(closeSound);
            }
        }

        _animator.SetBool(IS_OPEN_PARAM, _isOpen);
    }
}
