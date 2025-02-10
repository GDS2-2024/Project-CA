using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFMJ : Ability
{
    private TestShoot gunHandler;
    private PlayerStatManager statScript;
    private bool bulletsLoaded = false;

    public GameObject normalBullet;
    public GameObject FMJBullet;

    // Start is called before the first frame update
    void Start()
    {
        gunHandler = GetComponentInParent<TestShoot>();
        statScript = GetComponentInParent<PlayerStatManager>();

        isOnCooldown = true;
        StartCoroutine(Cooldown());
    }

    // Update is called once per frame
    void Update()
    {
        // If player has used all their FMJ bullets
        if (bulletsLoaded && statScript.currentClip == 0)
        {
            Debug.Log("Out of FMJ bullets");
            LoadNormalBullets();
        }
    }

    public override void OnPressAbility()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Loading FMJ Bullet into Rifle...");
            LoadFMJBullets();
            StartCoroutine(Cooldown());
        }
        else
        {
            Debug.Log("FMJ Ability on cooldown, wait to use again!");
        }
    }

    private void LoadFMJBullets()
    {
        bulletsLoaded = true;
        statScript.Reload();
        gunHandler.bullet = FMJBullet;
    }

    private void LoadNormalBullets()
    {
        bulletsLoaded = false;
        gunHandler.bullet = normalBullet;
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
