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
    private bool shotDelay;
    private InputDevice thisController;
    private PlayerController controllerScript;

    public GameObject bullet;
    public Vector3 spawnPos;
    public Camera playerCam;
    public GameObject gun;
    public float shotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        statScript = gameObject.GetComponent<PlayerStatManager>();
        shotDelay = false;
        controllerScript = gameObject.GetComponent<PlayerController>();
        thisController = controllerScript.GetController();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisController is Keyboard)
        {
            Mouse mouse = Mouse.current;
            if (mouse.leftButton.isPressed && statScript.currentAmmo > 0 && !shotDelay)
            {
                ShootBullet();
            }
        }
        else if (thisController is Gamepad controller)
        {
            if (controller.rightTrigger.isPressed && statScript.currentAmmo > 0 && !shotDelay)
            {
                ShootBullet();
            }
        }
    }

    void ShootBullet()
    {
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
        projectileScript.Shoot(direction);

        //Stops all bullets from firing at once, gives a variable fire rate
        StartCoroutine(ShotTimer());
    }

    IEnumerator ShotTimer()
    {
        shotDelay = true;
        yield return new WaitForSeconds(shotSpeed);
        shotDelay = false;
    }
}
