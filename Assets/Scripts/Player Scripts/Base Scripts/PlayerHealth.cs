using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{ 
    [Header("Health")]
    public float currentHealth;
    private float maxHealth = 100f;

    [Header("Damage tracking")]
    public bool abilityDamageTracker = false;
    public float durationDamageDealt = 0f;
    private float totalDamageDealt = 0f;

    [Header("Damage effects")]
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private GameObject hitDecal;
    [SerializeField] private DamageIndicator dmgDirectionIndicator;

    // Player Components
    private PlayerHUD playerHUD;
    private PlayerMoveBase playerMovement;
    private Rigidbody playerRigidbody;
    private PlayerScore playerScore;
    private PlayerGunHandler playerGun;
    private CapsuleCollider playerCollider;
    private PlayerAbilityHandler playerAbilityHandler;
    private InputDevice playerController;
    private PlayerRumbleHandler playerRumble;

    // Player Spawner used to Respawn
    private GameObject playerSpawner;
    private PlayerSpawner playerSpawnerScript;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        // Setup Player Spawner
        playerSpawner = GameObject.Find("Player Spawner");
        if (playerSpawner) { playerSpawnerScript = playerSpawner.GetComponent<PlayerSpawner>(); }

        // Get Player Components
        playerHUD = gameObject.GetComponent<PlayerHUD>();
        playerMovement = gameObject.GetComponent<PlayerMoveBase>();
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        playerScore = gameObject.GetComponent<PlayerScore>();
        playerGun = gameObject.GetComponent<PlayerGunHandler>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
        playerAbilityHandler = gameObject.GetComponent<PlayerAbilityHandler>();
        playerController = gameObject.GetComponent<PlayerController>().GetController();
        playerRumble = GetComponent<PlayerRumbleHandler>();
    }

    public void TakeDamage(float damage, GameObject attacker, RaycastHit hitPoint)
    {
        TakeDamage(damage, attacker);
        if (playerRumble) playerRumble.StartRumble(damage/1f, 0f, 0.1f);
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
        currentHealth -= damage;
        TrackDamageDealt(damage, attacker);
        if (playerHUD)
            playerHUD.UpdateHealthBar(currentHealth);

        if (currentHealth > 0)
        {
            if (currentHealth > maxHealth) { currentHealth = maxHealth; }
        }
        else
        {
            GiveKillScoreToAttacker(attacker);
            OnDeath();
        }
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
            PlayerHealth attackerHealth = attacker.GetComponent<PlayerHealth>();
            attackerHealth.totalDamageDealt += damage;
            if (attackerHealth.abilityDamageTracker)
            {
                attackerHealth.durationDamageDealt += damage;
                print(attackerHealth.durationDamageDealt);
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
            playerSpawnerScript.RespawnPlayer(playerController, gameObject);
        }
        else
        {
            Debug.LogWarning("RESPAWN FAILED: No Player Spawner object");
        }
    }
}
