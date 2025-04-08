using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public float cooldown;
    public float currentCooldownTime;
    protected bool isOnCooldown = false;

    public abstract void OnPressAbility();
    public abstract void OnHoldingAbility();
    public abstract void OnReleaseAbility();

    public virtual void StartCooldown()
    {
        isOnCooldown = true;
        currentCooldownTime = (int)cooldown;
    }

    // Abilities must not have an Update function
    protected virtual void Update()
    {
        if (!isOnCooldown) { return; }

        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
        }
        else
        {
            isOnCooldown = false;
            currentCooldownTime = 0;
        }
    }

}



