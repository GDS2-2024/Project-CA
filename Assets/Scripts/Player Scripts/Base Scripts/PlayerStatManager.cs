using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatManager : MonoBehaviour
{
    // Player Stat Variables
    public float health;
    public int maxAmmoInClip;
    public int currentAmmo;
    public float reloadTime;

    // Player Components
    private PlayerHUD playerHUD;
    private PlayerMoveBase playerMovement;
    private Rigidbody playerRigidbody;

    // Player Controller
    private PlayerController controllerScript;
    private InputDevice thisController;

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
        if (controllerScript) thisController = controllerScript.GetController();

        // Setup Player Spawner
        playerSpawner = GameObject.Find("Player Spawner");
        if (playerSpawner) playerSpawnerScript = playerSpawner.GetComponent<PlayerSpawner>();

        // Get Player Components
        playerMovement = gameObject.GetComponent<PlayerMoveBase>();
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisController is Keyboard keyboard)
        {
            if (keyboard.rKey.wasPressedThisFrame)
            {
                StartCoroutine(Reload());
            }
        }
        else if (thisController is Gamepad controller)
        {
            if (controller.buttonWest.wasPressedThisFrame)
            {
                StartCoroutine(Reload());
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

    public IEnumerator Reload()
    {
        // Start reload animation here
        yield return new WaitForSeconds(reloadTime);
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
        if (playerMovement) playerMovement.enabled = false;
        if (playerRigidbody) playerRigidbody.velocity = Vector3.zero;

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
        if (playerSpawnerScript) { playerSpawnerScript.RespawnPlayer(thisController, gameObject); }
        else { Debug.Log("RESPAWN FAILED: No Player Spawner object"); }
    }
}
