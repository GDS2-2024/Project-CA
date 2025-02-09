using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityHandler : MonoBehaviour
{
    public Ability UtilityAbility;
    public Ability DamageAbility;
    private bool hasUtilityAbility = true;
    private bool hasDamageAbility = true;

    // Start is called before the first frame update
    void Start()
    {
        if (UtilityAbility == null)
        {
            Debug.Log("The Character does not have a UTILITY ability.");
            hasUtilityAbility = false;
        }
        if (DamageAbility == null)
        {
            Debug.Log("The Character does not have a DAMAGE ability.");
            hasDamageAbility = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasUtilityAbility) { ManageUtilityAbility(); }
        if (hasDamageAbility) { ManageDamageAbility(); }
    }

    private void ManageUtilityAbility()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            UtilityAbility.OnPressAbility();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            UtilityAbility.OnHoldingAbility();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            UtilityAbility.OnReleaseAbility();
        }
    }

    private void ManageDamageAbility()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            DamageAbility.OnPressAbility();
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            DamageAbility.OnHoldingAbility();
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            DamageAbility.OnReleaseAbility();
        }
    }
}
