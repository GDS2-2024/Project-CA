using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{

    private Rigidbody rb;

    public float bulletSpeed;
    public float damage;
    private GameObject shooter; // Which player shot this bullet

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    public void Shoot(Vector3 moveDir, GameObject whoShot)
    {
        rb.velocity = moveDir * bulletSpeed;
        shooter = whoShot;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject != shooter) // Ignores if the player shoots themselves
            {
                PlayerStatManager statScript = collision.gameObject.GetComponent<PlayerStatManager>();
                statScript.TakeDamage(damage, shooter);
            }
        }

        Destroy(gameObject);
    }
}
