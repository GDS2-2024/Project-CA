using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    /// <summary>
    /// Cooldown starts once ability is finsihed being used.
    /// </summary>
    public float cooldownTime;
    protected bool isOnCooldown = false;

    public abstract void OnPressAbility();
    public abstract void OnHoldingAbility();
    public abstract void OnReleaseAbility();
    protected virtual void OnFinishCooldown()
    {
        // Optional, override function if ability has a cooldown
    }

    public virtual IEnumerator Cooldown() // StartCoroutine(Cooldown());
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
        OnFinishCooldown();
    }


}



