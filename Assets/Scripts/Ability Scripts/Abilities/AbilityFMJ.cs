using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFMJ : Ability
{
    // Player Components
    private PlayerGunHandler gunHandler;

    // FMJ Variables
    public GameObject normalBullet;
    public GameObject FMJBullet;
    public float fmjDuration;

    // Start is called before the first frame update
    void Start()
    {
        gunHandler = GetComponentInParent<PlayerGunHandler>();

        isOnCooldown = true;
        StartCooldown();
    }

    public override void OnPressAbility()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(ActivateFMJ());
        }
    }

    private IEnumerator ActivateFMJ()
    {
        gunHandler.bulletPrefab = FMJBullet;
        yield return new WaitForSeconds(fmjDuration);
        DeactivateFMJ();
    }

    private void DeactivateFMJ()
    {
        gunHandler.bulletPrefab = normalBullet;
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
