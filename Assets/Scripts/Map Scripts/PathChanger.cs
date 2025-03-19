using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathChanger : MonoBehaviour
{

    public List<GameObject> path1 = new List<GameObject>();
    public List<GameObject> path2 = new List<GameObject>();
    public List<GameObject> path3 = new List<GameObject>();

    private Dictionary<int, List<GameObject>> paths = new Dictionary<int, List<GameObject>>();
    private MineCartMove cartMoveScript;
    private string fromPath;
    private int randPath;
    private bool validPath = false;

    // Start is called before the first frame update
    void Start()
    {
        paths[0] = path1;
        paths[1] = path2;
        paths[2] = path3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cart")
        {
            cartMoveScript = other.GetComponent<MineCartMove>();

            while (!validPath)
            {
                randPath = Random.Range(0, 3);

                if (cartMoveScript.lastRail == paths[randPath][0])
                {
    
                    validPath = false;
                }
                else
                {
                    foreach (GameObject rail in paths[randPath])
                    {
                        cartMoveScript.rails.Add(rail);
                    }
                    validPath = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cart")
        {
            validPath = false;
        }
    }
}
