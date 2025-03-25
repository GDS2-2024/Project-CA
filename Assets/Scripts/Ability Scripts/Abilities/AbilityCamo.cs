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
        cooldown = 10.0f;
        playerMeshRenderer = GetComponentInParent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPressAbility()
    {
        if (!isInvisible && !isOnCooldown)
        {
            Debug.Log("Going Invisible!");
            StartCoroutine(ActivateCamo());
        } else
        {
            Debug.Log("Camo Ability on cooldown, wait to use again!");
        }
    }

    private IEnumerator ActivateCamo()
    {
        isInvisible = true;
        ApplyInvisibility(true);  // Make player invisible

        yield return new WaitForSeconds(abilityDuration);  // Wait for duration

        isInvisible = false;
        ApplyInvisibility(false);  // Remove invisibility

        Debug.Log("Camo Wears Off!");

        StartCooldown();  // Start cooldown timer before reusing ability
    }

    private void ApplyInvisibility(bool state)
    {
        playerMeshRenderer.enabled = !state;
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
