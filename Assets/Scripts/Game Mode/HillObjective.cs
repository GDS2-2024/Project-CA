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
        if (KOTHManager.hasGameFinished) { return; }
        if (numOfPlayersInHill == 1 && other.tag == "Player") { KOTHManager.AddScoreToPlayer(other.gameObject); }
        else if (numOfPlayersInHill > 1 && other.tag == "Player") { other.GetComponent<PlayerHUD>().UpdateObjectivePrompt("The Hill is contested!"); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            numOfPlayersInHill--;
        }
    }
}
