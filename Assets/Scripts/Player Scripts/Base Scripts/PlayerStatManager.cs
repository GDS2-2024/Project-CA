using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public float health;
    public int clipSize;
    public int currentClip;

    // Start is called before the first frame update
    void Start()
    {
        currentClip = clipSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            Reload();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health > 0)
        {
            
        }
        else
        {
            Die();
        }
    }

    public void ReduceAmmo()
    {
        currentClip -= 1;
    }

    public void Reload()
    {
        currentClip = clipSize;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
