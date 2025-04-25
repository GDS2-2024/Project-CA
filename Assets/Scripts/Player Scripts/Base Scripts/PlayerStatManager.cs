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
    private bool isReloading;
    private float reloadDurationTimer;
    public float maxHealth = 100f;
    public bool abilityDamageTracker = false;
    public float durationDamageDealt = 0f;

    public float health;
    public ParticleSystem hitEffect;

    [SerializeField]
    private GameObject hitDecal;

    [SerializeField]
    private DamageIndicator dmgDirectionIndicator;

    // Player Components
    private PlayerHUD playerHUD;
    private PlayerMoveBase playerMovement;
    private Rigidbody playerRigidbody;
    private PlayerScore playerScore;
    private PlayerGunHandler playerGun;
    private CapsuleCollider playerCollider;
    private PlayerAbilityHandler playerAbilityHandler;

    // Player Controller
    private PlayerController controllerScript;
    private InputDevice thisController;

    // Player Spawner used to Respawn
    private GameObject playerSpawner;
    private PlayerSpawner playerSpawnerScript;

    private float totalDamageDealt = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Setup Ammo
        playerHUD = gameObject.GetComponent<PlayerHUD>();
        currentAmmo = maxAmmoInClip;
        health = maxHealth;
        if (playerHUD)
            playerHUD.UpateAmmoUI(currentAmmo);

        // Setup Controller
        controllerScript = gameObject.GetComponent<PlayerController>();
        if (controllerScript)
            thisController = controllerScript.GetController();

        // Setup Player Spawner
        playerSpawner = GameObject.Find("Player Spawner");
        if (playerSpawner)
            playerSpawnerScript = playerSpawner.GetComponent<PlayerSpawner>();

        // Get Player Components
        playerMovement = gameObject.GetComponent<PlayerMoveBase>();
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        playerScore = gameObject.GetComponent<PlayerScore>();
        playerGun = gameObject.GetComponent<PlayerGunHandler>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
        playerAbilityHandler = gameObject.GetComponent<PlayerAbilityHandler>();
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
        if (isReloading)
        {
            ManageReloadCountdown();
        }
    }

    public void TakeDamage(float damage, GameObject attacker, RaycastHit hitPoint)
    {
        TakeDamage(damage, attacker);
        if (hitEffect)
        {
            ParticleSystem effect = Instantiate(
                hitEffect,
                hitPoint.point,
                Quaternion.LookRotation(hitPoint.normal)
            );
            // attach the effect to the player
            effect.transform.SetParent(transform);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }
        if (hitDecal)
        {
            GameObject decal = Instantiate(
                hitDecal,
                hitPoint.point,
                Quaternion.LookRotation(-hitPoint.normal)
            );
            decal.transform.SetParent(transform);
            Destroy(decal, 5f);
        }
        if (dmgDirectionIndicator)
        {
            dmgDirectionIndicator.damageLocation = attacker.transform.position;
            GameObject dmgObj = Instantiate(dmgDirectionIndicator.gameObject, GetComponentInChildren<Canvas>().transform);
            dmgObj.SetActive(true);
        }
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        health -= damage;
        TrackDamageDealt(damage, attacker);
        if (playerHUD)
            playerHUD.UpdateHealthBar(health);

        if (health > 0)
        {
            if (health > maxHealth) { health = maxHealth; }
        }
        else
        {
            GiveKillScoreToAttacker(attacker);
            OnDeath();
        }
    }

    public bool isPlayerReloading()
    {
        return isReloading;
    }

    public void ReduceAmmo()
    {
        currentAmmo -= 1;
        if (playerHUD)
            playerHUD.UpateAmmoUI(currentAmmo);
    }

    public IEnumerator Reload()
    {
        // Start reload animation here
        isReloading = true;
        reloadDurationTimer = 0;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = maxAmmoInClip;
        if (playerHUD)
            playerHUD.UpateAmmoUI(currentAmmo);
    }

    public void CancelReload()
    {
        // Stop reload animation here
        isReloading = false;
        StopCoroutine(Reload());
    }

    public void ManageReloadCountdown()
    {
        reloadDurationTimer += Time.deltaTime;
        playerHUD.UpdateReloadCooldown(1 - (reloadDurationTimer / reloadTime));
    }

    private void GiveKillScoreToAttacker(GameObject attacker)
    {
        if (attacker != null)
        {
            attacker.GetComponent<PlayerScore>().AddPlayerKill();
        }
    }

    public void TrackDamageDealt(float damage, GameObject attacker)
    {
        if (attacker != null)
        {
            PlayerStatManager attackerStatScript = attacker.GetComponent<PlayerStatManager>();
            attackerStatScript.totalDamageDealt += damage;
            if (attackerStatScript.abilityDamageTracker)
            {
                attackerStatScript.durationDamageDealt += damage;
                print(attackerStatScript.durationDamageDealt);
            }
        }
    }
    private void OnDeath()
    {
        if (playerScore)
            playerScore.AddPlayerDeath();
        // Hide all MeshRenderers in this object and its children
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }

        // Disable Components
        if (playerMovement)
            playerMovement.enabled = false;
        if (playerRigidbody)
            playerRigidbody.velocity = Vector3.zero;
        if (playerRigidbody)
            playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        if (playerScore)
            playerScore.enabled = false;
        if (playerGun)
            playerGun.enabled = false;
        if (playerCollider)
            playerCollider.enabled = false;
        if (playerAbilityHandler)
            playerAbilityHandler.enabled = false;

        // Show 3-second UI countdown for respawning
        StartRespawnCountdown();
        Invoke("Respawn", 3.0f);
    }

    private void StartRespawnCountdown()
    {
        if (!playerHUD)
        {
            return;
        }

        playerHUD.StartRespawnTimer();
    }

    private void Respawn()
    {
        if (playerSpawnerScript)
        {
            playerSpawnerScript.RespawnPlayer(thisController, gameObject);
        }
        else
        {
            Debug.LogWarning("RESPAWN FAILED: No Player Spawner object");
        }
    }
}
