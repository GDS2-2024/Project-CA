using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatManager : MonoBehaviour
{
    private InputDevice thisController;
    private PlayerController controllerScript;
    private float health;

    public float maxHealth = 100f;
    public int clipSize;
    public int currentClip;

    private PlayerHUD playerHUD;

    // Start is called before the first frame update
    void Start()
    {
        playerHUD = gameObject.GetComponent<PlayerHUD>();
        currentClip = clipSize;
        if (playerHUD) playerHUD.UpateAmmoUI(currentClip);
        controllerScript = gameObject.GetComponent<PlayerController>();
        thisController = controllerScript.GetController();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisController is Keyboard keyboard)
        {
            if (keyboard.rKey.wasPressedThisFrame)
            {
                Reload();
            }
        }
        else if (thisController is Gamepad controller)
        {
            if (controller.buttonWest.wasPressedThisFrame)
            {
                Reload();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health > 0)
        {
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        else
        {
            Die();
        }

        print(health);
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
