using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HillObjective : MonoBehaviour
{
    [SerializeField] private int numOfPlayersInHill = 0;
    [SerializeField] private KOTHManager KOTHManager;
    //private int playerLayerMask;
    private Collider hillCollider;

    // Start is called before the first frame update
    void Start()
    {
        if (KOTHManager == null) { Debug.Log("ERROR: Hill has no KOTHManager"); }
        hillCollider = GetComponent<Collider>();
        //playerLayerMask = LayerMask.GetMask("Player");
        StartCoroutine(CheckPlayersInHill());
    }

    private IEnumerator CheckPlayersInHill()
    {
        while (!KOTHManager.hasGameFinished)
        {
            yield return new WaitForSeconds(0.25f);
            if (this.gameObject == KOTHManager.activeHill) { UpdatePlayersInHill(); }
        }
    }

    private void UpdatePlayersInHill()
    {
        Collider[] colliders = Physics.OverlapBox(hillCollider.bounds.center, hillCollider.bounds.extents, Quaternion.identity);
        numOfPlayersInHill = colliders.Count(collider => collider.CompareTag("Player"));
    }

    private void OnTriggerStay(Collider other)
    {
        if (KOTHManager.hasGameFinished) { return; }
        if (this.gameObject != KOTHManager.activeHill) { return; }
        if (numOfPlayersInHill == 1 && other.tag == "Player")
        {
            KOTHManager.AddScoreToPlayer(other.gameObject);
            other.GetComponent<PlayerHUD>().ClearObjectivePrompt();
        }
        else if (numOfPlayersInHill > 1 && other.tag == "Player")
        {
            other.GetComponent<PlayerHUD>().UpdateObjectivePrompt("The Hill is contested!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (KOTHManager.hasGameFinished) { return; }
        if (this.gameObject != KOTHManager.activeHill) { return; }
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerHUD>().ClearObjectivePrompt();
        }
    }
}
