using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public float health;
    public int clipSize;
    public int currentClip;

    private PlayerHUD playerHUD;

    // Start is called before the first frame update
    void Start()
    {
        playerHUD = gameObject.GetComponent<PlayerHUD>();
        currentClip = clipSize;
        if (playerHUD) playerHUD.UpateAmmoUI(currentClip);
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
        if (playerHUD) playerHUD.UpateAmmoUI(currentClip);
    }

    public void Reload()
    {
        currentClip = clipSize;
        if (playerHUD) playerHUD.UpateAmmoUI(currentClip);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
