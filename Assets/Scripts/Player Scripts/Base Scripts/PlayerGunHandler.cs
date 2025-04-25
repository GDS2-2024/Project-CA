using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunHandler : MonoBehaviour
{
    // Player Components
    private PlayerHUD playerHUD;
    private PlayerMoveBase playerMovement;
    private InputDevice playerController;

    // Gun & Shooting
    public GameObject gunObject;
    public GameObject bulletPrefab;
    public Camera playerCam;
    [Header("Gun Stats")]
    public float shotSpeed;
    private bool shotDelay;
    private bool shootingDisabled = false;

    // Ammo & Reloading
    public int maxAmmoInClip;
    private int currentAmmo;
    public float reloadTime;
    private bool isReloading;
    private float reloadDurationTimer;
    private bool CanShoot() => currentAmmo > 0 && !shotDelay && !shootingDisabled && !isReloading;

    // Zoom & Aiming
    [Header("Zoom")]
    public int zoomedFOV;
    private int normalFOV = 60;
    private float targetFOV = 60;
    private float zoomSpeed = 10f;
    public float zoomSensRedution; // Higher numbers = lower sensitivity

    // Reticle Sizes
    private float normalReticleSize;
    public float zoomedReticleSize;
    private float targetReticleSize;

    // Start is called before the first frame update
    void Start()
    {
        // Get Player Components
        playerHUD = GetComponent<PlayerHUD>();
        playerMovement = GetComponent<PlayerMoveBase>();
        playerController = GetComponent<PlayerController>().GetController();

        // Setup Ammo
        playerHUD = gameObject.GetComponent<PlayerHUD>();
        if (playerHUD) { playerHUD.UpateAmmoUI(currentAmmo); }

        shotDelay = false;
        targetReticleSize = normalReticleSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading) { ManageReloadCountdown(); }

        if (playerController is Keyboard keyboard)
        {
            Mouse mouse = Mouse.current;
            if (mouse.leftButton.isPressed && CanShoot()) ShootBullet();
            if (mouse.rightButton.wasPressedThisFrame) SetZoom(true);
            if (mouse.rightButton.wasReleasedThisFrame) SetZoom(false);
            if (keyboard.rKey.wasPressedThisFrame) StartCoroutine(Reload());
        }
        else if (playerController is Gamepad controller)
        {
            if (controller.rightTrigger.isPressed && CanShoot()) ShootBullet();
            if (controller.leftTrigger.wasPressedThisFrame) SetZoom(true);
            if (controller.leftTrigger.wasReleasedThisFrame) SetZoom(false);
            if (controller.buttonWest.wasPressedThisFrame) StartCoroutine(Reload());
        }

        // ADS / ZOOM
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        float newReticleSize = Mathf.Lerp(playerHUD.Reticle.rectTransform.sizeDelta.x, targetReticleSize, Time.deltaTime * zoomSpeed);
        playerHUD.SetReticleSize(newReticleSize);
    }

    void SetZoom(bool isAimed)
    {
        targetFOV = isAimed ? zoomedFOV : normalFOV;

        //Reduce aiming sensitivity when zoomed
        float sensMultiplier = isAimed ? 1f / zoomSensRedution : zoomSensRedution;
        playerMovement.controlXSens *= sensMultiplier;
        playerMovement.controlYSens *= sensMultiplier;
        playerMovement.mouseSens *= sensMultiplier;

        //Shrink reticle when zoomed
        targetReticleSize = isAimed ? zoomedReticleSize : normalReticleSize;
    }

    void ShootBullet()
    {        
        ReduceAmmo();

        // Perform a raycast from the center of the camera
        RaycastHit hit;
        Vector3 target;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 100f)) { target = hit.point; }
        else { target = playerCam.transform.position + playerCam.transform.forward * 100f; }

        //calculates the direction of the bullet by subtracting vectors
        Vector3 spawnPos = gunObject.transform.position;
        Vector3 direction = (target - spawnPos).normalized;

        // Instantiates bullet and shoots it
        GameObject newBullet = Instantiate(bulletPrefab, spawnPos, transform.rotation);
        newBullet.GetComponent<TestProjectile>().Shoot(direction, gameObject);

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
    public void TempDisableShooting(float duration) { StartCoroutine(DisableShooting(duration)); }
    private IEnumerator DisableShooting(float duration)
    {
        shootingDisabled = true;
        yield return new WaitForSeconds(duration);
        shootingDisabled = false;
    }
    public void DisableShooting() { shootingDisabled = true; }
    public void EnableShooting() { shootingDisabled = false; }

    // Reloading
    public bool isPlayerReloading() { return isReloading; }

    public void ReduceAmmo()
    {
        currentAmmo -= 1;
        if (playerHUD) { playerHUD.UpateAmmoUI(currentAmmo); }
    }

    public IEnumerator Reload()
    {
        // Start reload animation here
        isReloading = true;
        reloadDurationTimer = 0;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = maxAmmoInClip;
        if (playerHUD) { playerHUD.UpateAmmoUI(currentAmmo); }
    }

    public void CancelReload()
    {
        // Stop reload animation here
        isReloading = false;
        StopCoroutine(Reload());
    }

    public void ManageReloadCountdown()
    {
        reloadDurationTimer += Time.deltaTime;
        playerHUD.UpdateReloadCooldown(1 - (reloadDurationTimer / reloadTime));
    }
}
