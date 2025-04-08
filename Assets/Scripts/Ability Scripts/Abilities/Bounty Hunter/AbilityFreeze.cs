using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class AbilityFreeze : Ability
{
    public GameObject mapScreenPrefab;
    public GameObject freezePrefab;
    public Transform playerHUDTransform;

    private bool isMapShown = false;
    private GameObject mapUI;
    private GameObject freezeObj;
    private InputDevice thisController;

    // Start is called before the first frame update
    void Start()
    {
        isOnCooldown = true;
        thisController = GetComponentInParent<PlayerController>().GetController();
    }

    public override void OnPressAbility()
    {
        if (isOnCooldown) { return; }

        if (!isMapShown) { ShowMap(true); }
        else { ActivateFreezeArea(); }
    }

    private void ShowMap(bool visible)
    {
        if (visible)
        {
            isMapShown = true;
            mapUI = Instantiate(mapScreenPrefab, playerHUDTransform);
            freezeObj = Instantiate(freezePrefab, Vector3.zero, Quaternion.identity);
            // Pause player movement while they select where zone location
            PlayerMoveBase playerMove = GetComponentInParent<PlayerMoveBase>();
            playerMove.DisableMovement();
            playerMove.DisableCamera();
        }
        else
        {
            isMapShown = false;
            Destroy(mapUI);
            // Resume player movement when map is gone
            PlayerMoveBase playerMove = GetComponentInParent<PlayerMoveBase>();
            playerMove.EnableMovement();
            playerMove.EnableCamera();
        }
    }

    private void ActivateFreezeArea()
    {
        ShowMap(false);
        freezeObj.GetComponent<Collider>().enabled = true;
        freezeObj.GetComponent<FreezeZone>().StartFreeze();
        StartCooldown();
    }

    protected override void Update()
    {
        base.Update();

        if (!isMapShown) { return; }

        float inputX = 0;
        float inputY = 0;

        //Gets input from the input device
        if (thisController is Keyboard)
        {
            Mouse mouse = Mouse.current;
            inputX = mouse.delta.x.ReadValue() * 0.1f;
            inputY = mouse.delta.y.ReadValue() * 0.1f;
        }
        else if (thisController is Gamepad controller)
        {
            inputX = controller.leftStick.ReadValue().x * Time.deltaTime * 200;
            inputY = controller.leftStick.ReadValue().y * Time.deltaTime * 200;
        }

        freezeObj.transform.position = new Vector3(freezeObj.transform.position.x+ inputX, 0, freezeObj.transform.position.z + inputY);
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
