using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityHandler : MonoBehaviour
{
    public Ability UtilityAbility;
    public Ability DamageAbility;
    public Ability UltimateAbility;

    private PlayerHUD playerHUD;
    private bool hasUtilityAbility = true;
    private bool hasDamageAbility = true;
    private bool hasUltimateAbility = true;
    private InputDevice thisController;
    private PlayerController controllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerHUD = gameObject.GetComponent<PlayerHUD>();
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
        if (UltimateAbility == null)
        {
            Debug.Log("The Character does not have a ULTIMATE ability.");
            hasUltimateAbility = false;
        }
        controllerScript = gameObject.GetComponent<PlayerController>();
        thisController = controllerScript.GetController();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasUtilityAbility)
        {
            ManageUtilityAbility();
            playerHUD.UpdateUtilityCooldown(UtilityAbility.currentCooldownTime / UtilityAbility.cooldown);
        }
        if (hasDamageAbility)
        {
            ManageDamageAbility();
            playerHUD.UpdateDamageCooldown(DamageAbility.currentCooldownTime / DamageAbility.cooldown);
        }
        if (hasUltimateAbility)
        {
            ManageUltimateAbility();
            playerHUD.UpdateUltimateCooldown(UltimateAbility.currentCooldownTime / UltimateAbility.cooldown);
        }
    }

    private void ManageUtilityAbility()
    {
        if (thisController is Keyboard keyboard)
        {
            if (keyboard.eKey.wasPressedThisFrame) { UtilityAbility.OnPressAbility(); }
            if (keyboard.eKey.isPressed) { UtilityAbility.OnHoldingAbility(); }
            if (keyboard.eKey.wasReleasedThisFrame) { UtilityAbility.OnReleaseAbility(); }
        }
        else if (thisController is Gamepad controller)
        {
            if (controller.rightShoulder.wasPressedThisFrame) { UtilityAbility.OnPressAbility(); }
            if (controller.rightShoulder.isPressed) { UtilityAbility.OnHoldingAbility(); }
            if (controller.rightShoulder.wasReleasedThisFrame) { UtilityAbility.OnReleaseAbility(); }
        }
    }

    private void ManageDamageAbility()
    {
        if (thisController is Keyboard keyboard)
        {
            if (keyboard.qKey.wasPressedThisFrame) { DamageAbility.OnPressAbility(); }
            if (keyboard.qKey.isPressed) { DamageAbility.OnHoldingAbility(); }
            if (keyboard.qKey.wasReleasedThisFrame) { DamageAbility.OnReleaseAbility(); }
        }
        else if (thisController is Gamepad controller)
        {
            if (controller.leftShoulder.wasPressedThisFrame) { DamageAbility.OnPressAbility(); }
            if (controller.leftShoulder.isPressed) { DamageAbility.OnHoldingAbility(); }
            if (controller.leftShoulder.wasReleasedThisFrame) { DamageAbility.OnReleaseAbility(); }
        }
    }

    private void ManageUltimateAbility()
    {
        if (thisController is Keyboard keyboard)
        {
            if (keyboard.altKey.wasPressedThisFrame) { UltimateAbility.OnPressAbility(); }
            if (keyboard.altKey.isPressed) { UltimateAbility.OnHoldingAbility(); }
            if (keyboard.altKey.wasReleasedThisFrame) { UltimateAbility.OnReleaseAbility(); }
        }
        else if (thisController is Gamepad controller)
        {
            if (controller.buttonNorth.wasPressedThisFrame) { UltimateAbility.OnPressAbility(); }
            if (controller.buttonNorth.isPressed) { UltimateAbility.OnHoldingAbility(); }
            if (controller.buttonNorth.wasReleasedThisFrame) { UltimateAbility.OnReleaseAbility(); }
        }
    }
}
