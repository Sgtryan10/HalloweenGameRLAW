    using UnityEngine;

    public class PlayerThrow : MonoBehaviour
    {
        public GameObject candy;
        public Transform throwPoint;
        public AudioClip shootSound;
        private AudioSource audioSource;

        public float throwForce = 20f;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PerformThrow();
            }
        }

        void PerformThrow()
        {
            if(DoorCandy.interactionCount > 0) {
                DoorCandy.Instance.LoseCandy();
                GameObject thrownCandy = Instantiate(candy, throwPoint.position, throwPoint.rotation);

                Rigidbody rb = thrownCandy.GetComponent<Rigidbody>();

                Vector3 direction = transform.forward;

                rb.AddForce(direction * throwForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
                audioSource.PlayOneShot(shootSound);
            }
        }
    }
