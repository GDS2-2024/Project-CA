using UnityEngine;

public class DamageExplosion : MonoBehaviour
{
    public float deathCD = 0.5f;
    private float damage = 25f;

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
            playerHealth.TakeDamage(damage, null);
        }
    }
}
