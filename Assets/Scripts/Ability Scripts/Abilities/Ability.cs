using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{

    public float cooldown;
    public int currentCooldownTime;
    protected bool isOnCooldown = false;

    public abstract void OnPressAbility();
    public abstract void OnHoldingAbility();
    public abstract void OnReleaseAbility();

    public virtual IEnumerator Cooldown() // use as: StartCoroutine(Cooldown());
    {
        isOnCooldown = true;
        currentCooldownTime = (int)cooldown;

        while (currentCooldownTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentCooldownTime--;
        }

        isOnCooldown = false;
    }


}



