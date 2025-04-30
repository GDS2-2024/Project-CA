using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeZone : MonoBehaviour
{
    private float zoneDuration = 10;
    private List<GameObject> frozenPlayers = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMoveBase move = other.GetComponent<PlayerMoveBase>();
            if (move) move.EnableSlowMovement();
            frozenPlayers.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMoveBase move = other.GetComponent<PlayerMoveBase>();
            if (move) move.DisableSlowMovement();
            frozenPlayers.Remove(other.gameObject);
        }
    }

    public void StartFreeze()
    {
        StartCoroutine(Duration());
    }

    private IEnumerator Duration()
    {
        yield return new WaitForSeconds(zoneDuration);
        foreach (GameObject player in frozenPlayers)
        {
            PlayerMoveBase move = player.GetComponent<PlayerMoveBase>();
            if (move) move.DisableSlowMovement();
        }
        Destroy(this.gameObject);
    }

}
