using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRamp : MonoBehaviour
{
    public Vector3 GravityForce;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            PlayerMoveBase playerMove = other.GetComponent<PlayerMoveBase>();
            playerMove.TempDisableMovement(3.0f); // Hardcoded timer, need a better implementation to disable until player touches the ground again

            Vector3 forceDirection = transform.TransformDirection(GravityForce);
            rb.AddForce(forceDirection, ForceMode.Impulse);
        }

    }
}
