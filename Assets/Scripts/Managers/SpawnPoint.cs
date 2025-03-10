using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
