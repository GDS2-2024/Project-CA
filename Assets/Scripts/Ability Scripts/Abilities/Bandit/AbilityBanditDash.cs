using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBanditDash : Ability
{
    public float dashForce;
    public float damage;
    public float dashDuration;

    private Rigidbody rb;
    private BoxCollider bc;
    private PlayerHealth playerHealth;
    private PlayerMoveBase playerMoveScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        playerMoveScript = GetComponentInParent<PlayerMoveBase>();
    }

    public override void OnPressAbility()
    {
        if (!isOnCooldown)
        {
            playerMoveScript.TempDisableMovement(dashDuration);
            StartCoroutine(DashCollision());
            rb.AddForce(playerMoveScript.playerCam.transform.forward * dashForce, ForceMode.Impulse);
            StartCooldown();
        }
    }

    public override void OnHoldingAbility()
    {
        
    }

    public override void OnReleaseAbility()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        rb.AddForce(0, 0, 0);

        if (other.gameObject.tag == "Player")
        {
            print("player hit");
            playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage, transform.parent.gameObject);
            if (playerHealth.currentHealth <= 0)
            {
                currentCooldownTime = 0.0f;
                isOnCooldown = false;
            }
        }
    }

    private IEnumerator DashCollision()
    {
        bc.enabled = true;
        yield return new WaitForSeconds(dashDuration);
        bc.enabled = false;
    }
}
