using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCartMove : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 5.0f;
    public List<GameObject> rails = new List<GameObject>();
    public GameObject lastRail;

    private Vector3 moveDir;
    private float threshold = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rails.Count > 0)
        {
            moveDir = rails[0].transform.position - gameObject.transform.position;
            rb.velocity = moveDir * speed;

            if (Vector3.Distance(gameObject.transform.position, rails[0].transform.position) <= threshold)
            {
                if (rails.Count > 1)
                {
                    lastRail = rails[0];
                }
                rails.RemoveAt(0);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cart" || collision.gameObject.tag == "Wall")
        {
            ExplodeCart();
        }
    }

    void ExplodeCart()
    {
        Destroy(gameObject);
    }
}
