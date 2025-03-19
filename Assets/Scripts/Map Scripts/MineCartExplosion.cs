using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCartExplosion : MonoBehaviour
{

    public float health = 10f;
    public GameObject bombCart;
    public GameObject healthCart;
    public GameObject damageExplosion;
    public GameObject healthExplosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            ExplodeCart();
        }
    }

    public void ExplodeCart()
    {
        if (gameObject.name.StartsWith(bombCart.name))
        {
            print("bomb cart explode");
            Instantiate(damageExplosion, gameObject.transform.position, Quaternion.identity, null);
        }
        else if (gameObject.name.StartsWith(healthCart.name))
        {
            print("heal cart explode");
            Instantiate(healthExplosion, gameObject.transform.position, Quaternion.identity, null);
        }
        Destroy(gameObject);
    }
}
