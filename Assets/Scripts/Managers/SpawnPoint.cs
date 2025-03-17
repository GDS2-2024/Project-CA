using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPoint : MonoBehaviour
{
    public bool spawnAvailable = true;

    void OnDrawGizmos()
    {
        //Sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.75f);

        //Arrow
        Handles.color = Color.green;
        Handles.DrawAAPolyLine(5, transform.position, transform.position + transform.forward * 2);
    }
}
