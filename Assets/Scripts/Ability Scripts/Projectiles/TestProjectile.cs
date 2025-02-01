using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{

    private Rigidbody rb;

    public float bulletSpeed;
    public float damage;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector3 moveDir)
    {
        rb.velocity = moveDir * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        //Deal damage if hit a player
        if (collision.gameObject.tag == "Player")
        {
            PlayerStatManager statScript = collision.gameObject.GetComponent<PlayerStatManager>();
            statScript.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
