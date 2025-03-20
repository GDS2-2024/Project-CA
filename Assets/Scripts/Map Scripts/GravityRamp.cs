using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRamp : MonoBehaviour
{
    private Vector3 GravityForce = new Vector3(50, 0, 0);
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            PlayerMoveBase playerMove = other.GetComponent<PlayerMoveBase>();
            playerMove.TempDisableMovement(0.2f);

            Vector3 forceDirection = transform.TransformDirection(GravityForce);
            rb.AddForce(forceDirection, ForceMode.Impulse);
        }

    }
}
