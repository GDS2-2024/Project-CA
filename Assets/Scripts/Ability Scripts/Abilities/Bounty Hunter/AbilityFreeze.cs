using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class AbilityFreeze : Ability
{
    public GameObject mapScreenPrefab;
    public GameObject freezePrefab;
    public GameObject minimapIcon;
    public Transform playerHUDTransform;

    private bool isMapShown = false;
    private GameObject mapUI;
    private GameObject freezeObj;
    private InputDevice thisController;
    private PlayerSpawner playerSpawner;

    // Start is called before the first frame update
    void Start()
    {
        isOnCooldown = true;
        thisController = GetComponentInParent<PlayerController>().GetController();
        playerSpawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();
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
            // Show player icons on map
            if (playerSpawner)
            {
                List<GameObject> players = playerSpawner.GetPlayersInGame();
                foreach (GameObject player in players)
                {
                    Instantiate(minimapIcon, player.transform);
                }
            }
            // Pause player movement and shooting while they select where zone location
            PlayerMoveBase playerMove = GetComponentInParent<PlayerMoveBase>();
            playerMove.DisableMovement();
            playerMove.DisableCamera();
            PlayerGunHandler playerGun = GetComponentInParent<PlayerGunHandler>();
            playerGun.DisableShooting();
        }
        else
        {
            isMapShown = false;
            Destroy(mapUI);
            // Hide player icons on map
            if (playerSpawner)
            {
                List<GameObject> players = playerSpawner.GetPlayersInGame();
                foreach (GameObject player in players)
                {
                    Transform minimapIconTransform = player.transform.Find("Minimap Icon(Clone)");
                    if (minimapIconTransform) Destroy(minimapIconTransform.gameObject);
                }
            }
            // Resume player movement and shooting when map is gone
            PlayerMoveBase playerMove = GetComponentInParent<PlayerMoveBase>();
            playerMove.EnableMovement();
            playerMove.EnableCamera();
            PlayerGunHandler testShoot = GetComponentInParent<PlayerGunHandler>();
            testShoot.EnableShooting();
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
