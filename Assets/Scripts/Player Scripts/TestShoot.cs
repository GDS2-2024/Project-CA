using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShoot : MonoBehaviour
{

    private GameObject newBullet;
    private TestProjectile projectileScript;
    private RaycastHit hit;
    private Vector3 target;

    public GameObject bullet;
    public Vector3 spawnPos;
    public Camera playerCam;
    public GameObject gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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

            spawnPos = gun.transform.position;

            Vector3 direction = (target - spawnPos).normalized;

            newBullet = Instantiate(bullet, spawnPos, transform.rotation);
            projectileScript = newBullet.GetComponent<TestProjectile>();
            projectileScript.Shoot(direction);
        }
    }
}
