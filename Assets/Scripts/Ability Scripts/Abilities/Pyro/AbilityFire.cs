using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AbilityFire : Ability
{
    public GameObject fireBomb;
    Vector3 spawnPosition;
    RaycastHit hit;

    public override void OnPressAbility()
    {
        if (isOnCooldown) { return; }

        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 100f)) { spawnPosition = hit.point; }
        GameObject newFireBomb = Instantiate(fireBomb, spawnPosition, transform.rotation);
        newFireBomb.GetComponent<FireBomb>().SetFiringPlayer(this.transform.parent.gameObject);
        StartCooldown();
    }

    public override void OnHoldingAbility()
    {
        // This ability does not need this function
    }

    public override void OnReleaseAbility()
    {
        // This ability does not need this function
    }
}
