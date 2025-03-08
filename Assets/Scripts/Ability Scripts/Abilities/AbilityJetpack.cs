using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityJetpack : Ability
{
    [SerializeField] private float initialBoost;  // The initial strong burst upwards
    [SerializeField] private float maxJetpackForce;  // Max force applied while holding
    [SerializeField] private float jetpackDecayRate;  // How fast the force weakens
    [SerializeField] private float fuelDuration;  // Maximum time the jetpack can be used

    private Rigidbody rb;
    private bool isUsingJetpack = false;
    private float currentJetpackForce;
    public float fuelRemaining;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        fuelRemaining = fuelDuration;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void OnPressAbility()
    {
        if (!isOnCooldown && !isUsingJetpack)
        {
            Debug.Log("Jetpack Activated!");
            rb.useGravity = false;
            rb.AddForce(Vector3.up * initialBoost, ForceMode.Impulse);  // Initial burst
            isUsingJetpack = true;
            currentJetpackForce = maxJetpackForce;
            fuelRemaining = fuelDuration;
        } else
        {
            Debug.Log("Ability on cooldown, wait to use again!");
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
            Debug.Log("Jetpack is out of fuel!");
            StartFalling();
        }
    }

    override public void OnReleaseAbility()
    {
        StartFalling();
    }

    private void StartFalling()
    {
        fuelRemaining = 0;
        isUsingJetpack = false;
        rb.useGravity = true;
        StartCooldown();
    }

}
