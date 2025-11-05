using UnityEngine;

public class CollectibleThrownCandy : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DoorCandy.Instance.CollectCandy();

            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
}
