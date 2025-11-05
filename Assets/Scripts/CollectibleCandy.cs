using UnityEngine;

public class CollectibleCandy : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatHeight = 0.2f;
    public float rotateSpeed = 90f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void Update()
    {
        transform.position = startPosition + Vector3.up * (Mathf.Sin(Time.time * floatSpeed) * floatHeight);

        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            DoorCandy.Instance.CollectCandy();

            Destroy(gameObject);
        }
    }
}
