using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBanditHeal : Ability
{
    public float duration;

    private float currentDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
        }
        else
        {
            currentDuration = 0f;
        }
    }

    public override void OnPressAbility()
    {
        currentDuration = duration;
    }

    public override void OnHoldingAbility()
    {

    }

    public override void OnReleaseAbility()
    {

    }
}
