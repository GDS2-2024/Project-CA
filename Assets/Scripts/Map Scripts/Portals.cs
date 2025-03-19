using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals : MonoBehaviour
{

    public GameObject oppositePortal;

    private Vector3 tpPos;
    private float offset = 4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tpPos = oppositePortal.transform.position;
            tpPos.z += offset;
            tpPos.y += 2f;
            other.gameObject.transform.position = tpPos;
        }
    }
}
