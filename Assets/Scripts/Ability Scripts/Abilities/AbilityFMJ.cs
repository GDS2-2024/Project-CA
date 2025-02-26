using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFMJ : Ability
{
    // Player Components
    private TestShoot gunHandler;
    private PlayerStatManager playerStatManager;
    private PlayerHUD playerHUD;

    // FMJ Variables
    public GameObject normalBullet;
    public GameObject FMJBullet;
    public float fmjDuration;

    // Start is called before the first frame update
    void Start()
    {
        gunHandler = GetComponentInParent<TestShoot>();
        playerStatManager = GetComponentInParent<PlayerStatManager>();
        playerHUD = GetComponentInParent<PlayerHUD>();

        isOnCooldown = true;
        StartCoroutine(Cooldown());
    }

    public override void OnPressAbility()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Activating FMJ...");
            StartCoroutine(ActivateFMJ());
        }
        else
        {
            Debug.Log("FMJ Ability on cooldown, wait to use again!");
        }
    }

    private IEnumerator ActivateFMJ()
    {
        gunHandler.bullet = FMJBullet;
        playerStatManager.currentAmmo = playerStatManager.maxAmmoInClip;
        playerHUD.UpateAmmoUI(playerStatManager.currentAmmo);
        yield return new WaitForSeconds(fmjDuration);
        Debug.Log("FMJ ended, reverting to normal bullets");
        DeactivateFMJ();
    }

    private void DeactivateFMJ()
    {
        gunHandler.bullet = normalBullet;
        StartCoroutine(Cooldown());
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
