using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBanditHeal : Ability
{
    public float abilityDuration;
    public float healingDenom;

    private PlayerStatManager playerStatScript;

    // Start is called before the first frame update
    void Start()
    {
        playerStatScript = GetComponentInParent<PlayerStatManager>();
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
        playerStatScript.abilityDamageTracker = true;
        yield return new WaitForSeconds(abilityDuration);
        playerStatScript.health += playerStatScript.durationDamageDealt / healingDenom;
        playerStatScript.abilityDamageTracker = false;
        playerStatScript.durationDamageDealt = 0f;
        StartCooldown();
    }
}
