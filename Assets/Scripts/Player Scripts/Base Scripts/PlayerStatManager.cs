using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatManager : MonoBehaviour
{
    // Player Controller
    private PlayerController controllerScript;
    private InputDevice thisController;

    // Player Stat Variables
    public float health;
    public int maxAmmoInClip;
    public int currentAmmo;

    // Player HUD
    private PlayerHUD playerHUD;

    // Player Spawner used to Respawn
    private GameObject playerSpawner;
    private PlayerSpawner playerSpawnerScript;

    // Start is called before the first frame update
    void Start()
    {
        // Setup Ammo
        playerHUD = gameObject.GetComponent<PlayerHUD>();
        currentAmmo = maxAmmoInClip;
        if (playerHUD) playerHUD.UpateAmmoUI(currentAmmo);

        // Setup Controller
        controllerScript = gameObject.GetComponent<PlayerController>();
        thisController = controllerScript.GetController();

        // Setup Player Spawner
        playerSpawner = GameObject.Find("Player Spawner");
        playerSpawnerScript = playerSpawner.GetComponent<PlayerSpawner>();
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
            OnDeath();
        }
    }

    public void ReduceAmmo()
    {
        currentAmmo -= 1;
        if (playerHUD) playerHUD.UpateAmmoUI(currentAmmo);
    }

    public void Reload()
    {
        currentAmmo = maxAmmoInClip;
        if (playerHUD) playerHUD.UpateAmmoUI(currentAmmo);
    }

    private void OnDeath()
    {
        // Hide all MeshRenderers in this object and its children
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }

        // Disable movement
        gameObject.GetComponent<PlayerMoveBase>().enabled = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Show 3-second UI countdown for respawning
        StartRespawnCountdown();
        Invoke("Respawn", 3.0f);
    }

    private void StartRespawnCountdown()
    {
        if (!playerHUD) { return; }

        playerHUD.StartRespawnTimer();
    }

    private void Respawn()
    {       
        playerSpawnerScript.RespawnPlayer(thisController, gameObject);
    }
}
