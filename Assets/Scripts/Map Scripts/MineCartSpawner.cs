using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCartSpawner : MonoBehaviour
{

    public float spawnCD = 5.0f;
    public float startingSpawn;
    public GameObject healthCart;
    public GameObject damageCart;
    public List<GameObject> startingPath = new List<GameObject>();

    private float currentSpawn = 0.0f;
    private int randCart;
    private GameObject newCart;
    private MineCartMove cartMoveScript;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawn = startingSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawn += 1 * Time.deltaTime;

        if (currentSpawn >= spawnCD)
        {
            spawnCart();
            currentSpawn = 0.0f;
        }
    }

    void spawnCart()
    {
        randCart = Random.Range(1, 3);

        switch (randCart)
        {
            case 1:
                newCart = Instantiate(healthCart, gameObject.transform);
                cartMoveScript = newCart.GetComponent<MineCartMove>();
                foreach (GameObject rail in startingPath)
                {
                    cartMoveScript.rails.Add(rail);
                }
                break;
            case 2:
                newCart = Instantiate(damageCart, gameObject.transform);
                cartMoveScript = newCart.GetComponent<MineCartMove>();
                foreach(GameObject rail in startingPath)
                {
                    cartMoveScript.rails.Add(rail);
                }
                break;
        }
    }
}
