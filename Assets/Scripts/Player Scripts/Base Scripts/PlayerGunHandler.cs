using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunHandler : MonoBehaviour
{
    // Player Components
    private PlayerHUD playerHUD;
    private PlayerStatManager playerStats;
    private PlayerMoveBase playerMovement;
    private InputDevice playerController;

    // Gun & Shooting
    public GameObject gunObject;
    public GameObject bulletPrefab;
    public Camera playerCam;
    public float shotSpeed;
    private bool shotDelay;
    private bool shootingDisabled = false;
    private bool CanShoot() => playerStats.currentAmmo > 0 && !shotDelay && !shootingDisabled && !playerStats.isPlayerReloading();

    // Zoom & Aiming
    private int normalFOV = 60;
    [Header("Zoom & Aiming")]
    public int aimedFOV;
    private float targetFOV = 60;
    private float zoomSpeedFOV = 10f;
    public float aimedSensRedution; // Higher numbers = lower sensitivity

    // Reticle Sizes
    public float normalReticleSize;
    public float zoomedReticleSize;
    private float targetReticleSize;

    // Start is called before the first frame update
    void Start()
    {
        playerHUD = GetComponent<PlayerHUD>();
        playerStats = GetComponent<PlayerStatManager>();
        playerMovement = GetComponent<PlayerMoveBase>();
        playerController = GetComponent<PlayerController>().GetController();

        shotDelay = false;
        targetReticleSize = normalReticleSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController is Keyboard)
        {
            Mouse mouse = Mouse.current;
            if (mouse.leftButton.isPressed && CanShoot()) ShootBullet();
            if (mouse.rightButton.wasPressedThisFrame) SetZoom(true);
            if (mouse.rightButton.wasReleasedThisFrame) SetZoom(false);
        }
        else if (playerController is Gamepad controller)
        {
            if (controller.rightTrigger.isPressed && CanShoot()) ShootBullet();
            if (controller.leftTrigger.wasPressedThisFrame) SetZoom(true);
            if (controller.leftTrigger.wasReleasedThisFrame) SetZoom(false);
        }

        // ADS / ZOOM
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeedFOV);
        float newReticleSize = Mathf.Lerp(playerHUD.Reticle.rectTransform.sizeDelta.x, targetReticleSize, Time.deltaTime * zoomSpeedFOV);
        playerHUD.SetReticleSize(newReticleSize);
    }


    void SetZoom(bool isAimed)
    {
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
        playerStats.ReduceAmmo();

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
}
