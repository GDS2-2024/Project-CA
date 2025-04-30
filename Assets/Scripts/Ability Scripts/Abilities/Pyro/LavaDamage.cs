using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    private float damage = 11.0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage*Time.deltaTime, null);
        }
    }
}

