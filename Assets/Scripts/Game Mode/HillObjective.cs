using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a single hill objective for KOTH, keeps track of players in hill and scores accordingly 
/// </summary>
public class HillObjective : MonoBehaviour
{
    [SerializeField] private int numOfPlayersInHill = 0;
    [SerializeField] private KOTHManager KOTHManager;
    private MatchManager matchManager;

    private Collider hillCollider;
    private MeshRenderer hillRenderer;

    private void Awake()
    {
        hillCollider = GetComponent<Collider>();
        hillRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        matchManager = GameObject.Find("Match Manager").GetComponent<MatchManager>();
        if (KOTHManager == null) { Debug.Log("ERROR: Hill has no KOTHManager"); }
        StartCoroutine(CheckPlayersInHill());
    }

    private IEnumerator CheckPlayersInHill()
    {
        while (!matchManager.hasGameFinished)
        {
            yield return new WaitForSeconds(0.1f);
            if (this.gameObject == KOTHManager.activeHill) { UpdatePlayersInHill(); }
        }
    }

    private void UpdatePlayersInHill()
    {
        Collider[] colliders = Physics.OverlapBox(hillCollider.bounds.center, hillCollider.bounds.extents, Quaternion.identity);
        numOfPlayersInHill = colliders.Count(collider => collider.CompareTag("Player"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (matchManager.hasGameFinished) { return; }
        if (this.gameObject != KOTHManager.activeHill) { return; }
        if (other.tag == "Player") { other.GetComponent<PlayerHUD>().SetActiveCompassObjective(false); }
    }

    private void OnTriggerStay(Collider other)
    {
        if (matchManager.hasGameFinished) { return; }
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
        if (matchManager.hasGameFinished) { return; }
        if (this.gameObject != KOTHManager.activeHill) { return; }
        if (other.tag == "Player")
        {
            PlayerHUD hud = other.GetComponent<PlayerHUD>();
            hud.ClearObjectivePrompt();
            hud.SetActiveCompassObjective(true);
        }
    }

    public void SetVisibility(bool enabled)
    {
        hillRenderer.enabled = enabled;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 size = new Vector3(15, 10, 15);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
