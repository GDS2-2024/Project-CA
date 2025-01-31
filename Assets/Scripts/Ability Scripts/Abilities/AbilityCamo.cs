using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCamo : Ability
{
    private MeshRenderer playerMeshRenderer;
    public float abilityDuration = 5.0f;
    private bool isInvisible = false;

    // Start is called before the first frame update
    void Start()
    {
        cooldownTime = 10.0f;
        playerMeshRenderer = GetComponentInParent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void OnPressAbility()
    {
        if (!isInvisible && !isOnCooldown)
        {
            Debug.Log("Going Invisible!");
            StartCoroutine(ActivateCamo());
        } else
        {
            Debug.Log("Ability on cooldown, wait to use again!");
        }      
    }

    protected override void OnFinishCooldown()
    {
        Debug.Log("Camo cooldown finished, can use again!");
        playerMeshRenderer.enabled = true;
    }
    private IEnumerator ActivateCamo()
    {
        isInvisible = true;
        ApplyInvisibility(true);  // Make player invisible

        yield return new WaitForSeconds(abilityDuration);  // Wait for duration

        isInvisible = false;
        ApplyInvisibility(false);  // Remove invisibility

        Debug.Log("Camo Wears Off!");

        StartCoroutine(Cooldown());  // Start cooldown timer before reusing ability
    }

    private void ApplyInvisibility(bool state)
    {
        playerMeshRenderer.enabled = !state;
    }

    override public void OnHoldingAbility()
    {
        // This ability does not need this function
    }

    override public void OnReleaseAbility()
    {
        // This ability does not need this function
    }
}
