using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityJetpack : Ability
{
    private float initialBoost = 1.0f;
    private float maxJetpackForce = 1.0f;
    private float jetpackDecayRate = 1.0f;
    private float fuelDuration = 3.0f;

    private Rigidbody rb;
    private bool isUsingJetpack = false;
    private float currentJetpackForce;
    public float fuelRemaining;
    private ParticleSystem particleSys;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        fuelRemaining = fuelDuration;
        particleSys = GetComponentInChildren<ParticleSystem>();
    }

    override public void OnPressAbility()
    {
        if (!isOnCooldown && !isUsingJetpack)
        {
            rb.useGravity = false;
            rb.AddForce(Vector3.up * initialBoost, ForceMode.Impulse);  // Initial burst
            isUsingJetpack = true;
            currentJetpackForce = maxJetpackForce;
            fuelRemaining = fuelDuration;
            particleSys.Play();
        }
    }
    override public void OnHoldingAbility()
    {
        if (isUsingJetpack && fuelRemaining > 0)
        {
            rb.AddForce(Vector3.up * currentJetpackForce, ForceMode.Force);
            currentJetpackForce = Mathf.Max(0, currentJetpackForce - jetpackDecayRate * Time.deltaTime);
            fuelRemaining -= Time.deltaTime;
        } else
        {
            StartFalling();
        }
    }

    override public void OnReleaseAbility()
    {
        StartFalling();
    }

    private void StartFalling()
    {
        if (!isUsingJetpack) { return; }

        fuelRemaining = 0;
        isUsingJetpack = false;
        rb.useGravity = true;
        particleSys.Stop();
        StartCooldown();
    }

}
