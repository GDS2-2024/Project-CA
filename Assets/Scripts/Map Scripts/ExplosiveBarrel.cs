using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    private float health = 50f;
    public GameObject damageExplosion;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            ExplodeBarrel();
        }
    }

    public void ExplodeBarrel()
    {
        Instantiate(damageExplosion, gameObject.transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }
}
