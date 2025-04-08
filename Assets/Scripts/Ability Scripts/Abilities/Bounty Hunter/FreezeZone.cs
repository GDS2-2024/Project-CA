using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeZone : MonoBehaviour
{
    private float zoneDuration = 10;
    private float freezeDuration = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMoveBase move = other.GetComponent<PlayerMoveBase>();
            if (move) move.TempSlowMovement(freezeDuration);
        }
    }

    public void StartFreeze()
    {
        StartCoroutine(Duration());
    }

    private IEnumerator Duration()
    {
        yield return new WaitForSeconds(zoneDuration);
        Destroy(this.gameObject);
    }

}
