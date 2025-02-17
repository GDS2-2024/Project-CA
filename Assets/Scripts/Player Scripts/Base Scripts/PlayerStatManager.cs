using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatManager : MonoBehaviour
{
    private InputDevice thisController;
    private PlayerController controllerScript;

    public float health;
    public int clipSize;
    public int currentClip;

    // Start is called before the first frame update
    void Start()
    {
        currentClip = clipSize;
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
