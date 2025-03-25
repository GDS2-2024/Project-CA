using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCartMove : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 10.0f;
    public List<GameObject> rails = new List<GameObject>();
    public GameObject lastRail;
    public GameObject cornerPrefab;

    private Vector3 moveDir;
    private float threshold = 2f;
    private float smoothing = 10f;
    private float rotationSpeed = 5f;
    private Quaternion targetRot;
    private MineCartExplosion cartExplosionScript;

    // Start is called before the first frame update
    void Start()
    {
        cartExplosionScript = gameObject.GetComponent<MineCartExplosion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rails.Count > 0)
        {
            Vector3 targetPos = rails[0].transform.position;
            Vector3 dir = targetPos - transform.position;
            if (rails[0] == cornerPrefab)
            {
                targetRot = rails[1].transform.rotation;
            }
            else
            {
                targetRot = rails[0].transform.rotation;
            }


            if (dir.magnitude <= threshold)
            {
                if (rails.Count > 1)
                {
                    lastRail = rails[0];
                }
                rails.RemoveAt(0);
            }
            else
            {
                moveDir = Vector3.Lerp(moveDir, dir.normalized, Time.deltaTime * smoothing);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDir * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cart" || collision.gameObject.tag == "Wall")
        {
            cartExplosionScript.ExplodeCart();
        }
    }
}
