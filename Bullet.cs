using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocky"))
        {
            other.GetComponent<Rocky>().health -= Random.Range(5, 10);
            Destroy(gameObject);
        }
    }
}
