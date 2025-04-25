using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityHandler : MonoBehaviour
{
    public Ability LeftAbility;
    public Ability RightAbility;
    public Ability UltimateAbility;

    private PlayerHUD playerHUD;
    private bool hasLeftAbility = true;
    private bool hasRightAbility = true;
    private bool hasUltimateAbility = true;
    public bool HasLeftAbility() { return hasLeftAbility; }
    public bool HasRightAbility() { return hasRightAbility; }
    public bool HasUltimateAbility() { return hasUltimateAbility; }
    
    private InputDevice thisController;
    private PlayerController controllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerHUD = gameObject.GetComponent<PlayerHUD>();
        if (LeftAbility == null)
        {
            Debug.Log("The Character does not have a LEFT ability.");
            hasLeftAbility = false;
        }
        if (RightAbility == null)
        {
            Debug.Log("The Character does not have a RIGHT ability.");
            hasRightAbility = false;
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
        if (hasRightAbility)
        {
            ManageRightAbility();
            playerHUD.RightAbilityCooldown(RightAbility.currentCooldownTime / RightAbility.cooldown);
        }
        if (hasLeftAbility)
        {
            ManageLeftAbility();
            playerHUD.LeftAbilityCooldown(LeftAbility.currentCooldownTime / LeftAbility.cooldown);
        }
        if (hasUltimateAbility)
        {
            ManageUltimateAbility();
            playerHUD.UltimateAbilityCooldown(UltimateAbility.currentCooldownTime / UltimateAbility.cooldown);
        }
    }

    private void ManageRightAbility()
    {
        if (thisController is Keyboard keyboard)
        {
            if (keyboard.eKey.wasPressedThisFrame) { RightAbility.OnPressAbility(); }
            if (keyboard.eKey.isPressed) { RightAbility.OnHoldingAbility(); }
            if (keyboard.eKey.wasReleasedThisFrame) { RightAbility.OnReleaseAbility(); }
        }
        else if (thisController is Gamepad controller)
        {
            if (controller.rightShoulder.wasPressedThisFrame) { RightAbility.OnPressAbility(); }
            if (controller.rightShoulder.isPressed) { RightAbility.OnHoldingAbility(); }
            if (controller.rightShoulder.wasReleasedThisFrame) { RightAbility.OnReleaseAbility(); }
        }
    }

    private void ManageLeftAbility()
    {
        if (thisController is Keyboard keyboard)
        {
            if (keyboard.qKey.wasPressedThisFrame) { LeftAbility.OnPressAbility(); }
            if (keyboard.qKey.isPressed) { LeftAbility.OnHoldingAbility(); }
            if (keyboard.qKey.wasReleasedThisFrame) { LeftAbility.OnReleaseAbility(); }
        }
        else if (thisController is Gamepad controller)
        {
            if (controller.leftShoulder.wasPressedThisFrame) { LeftAbility.OnPressAbility(); }
            if (controller.leftShoulder.isPressed) { LeftAbility.OnHoldingAbility(); }
            if (controller.leftShoulder.wasReleasedThisFrame) { LeftAbility.OnReleaseAbility(); }
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
