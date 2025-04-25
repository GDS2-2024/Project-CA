using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthExplosion : MonoBehaviour
{
    public float deathCD = 0.5f;
    private float heal = 25.0f;

    private float deathCurrent = 0f;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deathCurrent += Time.deltaTime;

        if (deathCurrent >= deathCD)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(-heal, null);
        }
    }
}
