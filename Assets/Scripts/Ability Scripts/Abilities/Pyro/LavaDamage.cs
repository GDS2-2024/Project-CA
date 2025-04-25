using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    private float damage = 10.0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage*Time.deltaTime, null);
        }
    }
}

