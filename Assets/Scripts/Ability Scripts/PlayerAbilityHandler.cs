using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityHandler : MonoBehaviour
{
    public Ability EquippedAbility;

    // Start is called before the first frame update
    void Start()
    {
        if (EquippedAbility == null)
        {
            Debug.Log("The Character does not have an ability.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EquippedAbility.OnPressAbility();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            EquippedAbility.OnHoldingAbility();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            EquippedAbility.OnReleaseAbility();
        }
    }
}
