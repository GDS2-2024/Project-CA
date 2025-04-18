using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestShoot : MonoBehaviour
{
    private GameObject newBullet;
    private TestProjectile projectileScript;
    private RaycastHit hit;
    private Vector3 target;
    private PlayerStatManager statScript;
    private PlayerMoveBase playerMovement;
    private bool shotDelay;
    private InputDevice thisController;
    private PlayerController controllerScript;
    private PlayerHUD playerHUD;
    private bool shootingDisabled = false;

    public GameObject bullet;
    public Vector3 spawnPos;
    public Camera playerCam;
    public GameObject gun;
    public float shotSpeed;

    private int normalFOV = 60;
    public int aimedFOV;
    public float aimedSensRedution; // Higher numbers = lower sensitivity
    private float zoomSpeed = 10f;
    private float targetFOV = 60;

    public float normalReticleSize;
    public float zoomedReticleSize;
    private float targetReticleSize;

    // Start is called before the first frame update
    void Start()
    {
        statScript = gameObject.GetComponent<PlayerStatManager>();
        playerMovement = gameObject.GetComponent<PlayerMoveBase>();
        shotDelay = false;
        controllerScript = gameObject.GetComponent<PlayerController>();
        thisController = controllerScript.GetController();
        playerHUD = GetComponent<PlayerHUD>();
        targetReticleSize = normalReticleSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (thisController is Keyboard)
        {
            Mouse mouse = Mouse.current;
            if (mouse.leftButton.isPressed && statScript.currentAmmo > 0 && !shotDelay) ShootBullet();
            if (mouse.rightButton.wasPressedThisFrame) SetZoom(true);
            if (mouse.rightButton.wasReleasedThisFrame) SetZoom(false);
        }
        else if (thisController is Gamepad controller)
        {
            if (controller.rightTrigger.isPressed && statScript.currentAmmo > 0 && !shotDelay) ShootBullet();
            if (controller.leftTrigger.wasPressedThisFrame) SetZoom(true);
            if (controller.leftTrigger.wasReleasedThisFrame) SetZoom(false);
        }

        // ADS / ZOOM
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        float newReticleSize = Mathf.Lerp(playerHUD.Reticle.rectTransform.sizeDelta.x, targetReticleSize, Time.deltaTime * zoomSpeed);
        playerHUD.SetReticleSize(newReticleSize);
    }

    void SetZoom(bool isAimed)
    {
        if (shootingDisabled) { return; }
        targetFOV = isAimed ? aimedFOV : normalFOV;

        //Reduce aiming sensitivity when zoomed
        float sensMultiplier = isAimed ? 1f / aimedSensRedution : aimedSensRedution;
        playerMovement.controlXSens *= sensMultiplier;
        playerMovement.controlYSens *= sensMultiplier;
        playerMovement.mouseSens *= sensMultiplier;

        //Shrink reticle when zoomed
        targetReticleSize = isAimed ? zoomedReticleSize : normalReticleSize;
    }

    void ShootBullet()
    {
        if (shootingDisabled) { return; }
        if (statScript.isPlayerReloading()) { return; }
        
        //Reduce Player ammo
        statScript.ReduceAmmo();

        // Perform a raycast from the center of the camera
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 100f))
        {
            // If the ray hits something, set the target point to the hit position
            target = hit.point;
        }
        else
        {
            // If no hit, set the target point far in front of the camera
            target = playerCam.transform.position + playerCam.transform.forward * 100f;
        }

        //Spawns the bullet at the gun location and passes the direction information to the bullet
        spawnPos = gun.transform.position;

        //calculates the direction of the bullet by subtracting vectors
        Vector3 direction = (target - spawnPos).normalized;

        newBullet = Instantiate(bullet, spawnPos, transform.rotation);
        //Gets the projectile script of the just created bullet
        projectileScript = newBullet.GetComponent<TestProjectile>();
        projectileScript.Shoot(direction, gameObject);

        //Stops all bullets from firing at once, gives a variable fire rate
        StartCoroutine(ShotTimer());
    }

    IEnumerator ShotTimer()
    {
        shotDelay = true;
        yield return new WaitForSeconds(shotSpeed);
        shotDelay = false;
    }

    // Disable/Enable Shooting
    public void TempDisableShooting(float duration)
    {
        StartCoroutine(DisableShooting(duration));
    }

    public void DisableShooting() { shootingDisabled = true; }

    public void EnableShooting() { shootingDisabled = false; }

    private IEnumerator DisableShooting(float duration)
    {
        shootingDisabled = true;
        yield return new WaitForSeconds(duration);
        shootingDisabled = false;
    }
}
