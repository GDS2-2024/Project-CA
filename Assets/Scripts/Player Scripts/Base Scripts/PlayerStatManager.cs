using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatManager : MonoBehaviour
{
    // Player Stat Variables
    public int maxAmmoInClip;
    public int currentAmmo;
    public float reloadTime;
    public float maxHealth = 100f;

    // Player Components
    private PlayerHUD playerHUD;
    private PlayerMoveBase playerMovement;
    private Rigidbody playerRigidbody;
    private PlayerScore playerScore;
    private float health;

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
        health = maxHealth;
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
        playerScore = gameObject.GetComponent<PlayerScore>();
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

    public void TakeDamage(float damage, GameObject attacker)
    {
        health -= damage;
        if (playerHUD) playerHUD.UpdateHealthBar(health);

        if (health > 0)
        {
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        else
        {
            GiveKillScoreToAttacker(attacker);
            OnDeath();
        }

        print(health);
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

    public void CancelReload()
    {
        // Stop reload animation here
        StopCoroutine(Reload());
    }

    private void GiveKillScoreToAttacker(GameObject attacker)
    {
        if (attacker.GetComponent<PlayerScore>()) { attacker.GetComponent<PlayerScore>().AddPlayerKill(); }
    }

    private void OnDeath()
    {
        if (playerScore) playerScore.AddPlayerDeath();
        // Hide all MeshRenderers in this object and its children
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }

        // Disable movement
        if (playerMovement) playerMovement.enabled = false;
        if (playerRigidbody) playerRigidbody.velocity = Vector3.zero;

        // Disable score component (so they cant get score when dead)
        if (playerScore) playerScore.enabled = false;

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
        else { Debug.LogWarning("RESPAWN FAILED: No Player Spawner object"); }
    }
}
