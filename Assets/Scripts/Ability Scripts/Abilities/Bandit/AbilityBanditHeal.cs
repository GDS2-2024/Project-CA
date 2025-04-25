using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBanditHeal : Ability
{
    public float abilityDuration;
    public float healingDenom;

    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
    }

    public override void OnPressAbility()
    {
        StartCoroutine(SanguineReaver());
    }

    public override void OnHoldingAbility()
    {

    }

    public override void OnReleaseAbility()
    {

    }

    private IEnumerator SanguineReaver()
    {
        playerHealth.abilityDamageTracker = true;
        yield return new WaitForSeconds(abilityDuration);
        playerHealth.currentHealth += playerHealth.durationDamageDealt / healingDenom;
        playerHealth.abilityDamageTracker = false;
        playerHealth.durationDamageDealt = 0f;
        StartCooldown();
    }
}
