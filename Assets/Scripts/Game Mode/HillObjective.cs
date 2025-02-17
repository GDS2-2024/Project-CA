using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillObjective : MonoBehaviour
{
    [SerializeField] private int numOfPlayersInHill = 0;
    [SerializeField] private KOTHManager KOTHManager;

    // Start is called before the first frame update
    void Start()
    {
        if (KOTHManager == null) { Debug.Log("ERROR: Hill has no KOTHManager"); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            numOfPlayersInHill++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Currently calling AddScoreToPlayer() for every frame they are in the hill
        // TO DO: Only add to score every 1 second.
        if (numOfPlayersInHill == 1 && other.tag == "Player") { KOTHManager.AddScoreToPlayer(other.gameObject); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            numOfPlayersInHill--;
        }
    }
}
