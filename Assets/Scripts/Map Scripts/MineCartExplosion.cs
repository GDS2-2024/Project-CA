using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCartExplosion : MonoBehaviour
{

    public float health = 10f;
    public GameObject bombCart;
    public GameObject healthCart;

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
        if (this.gameObject == bombCart)
        {

        }
        else if (this.gameObject == healthCart)
        {

        }
        Destroy(gameObject);
    }
}
