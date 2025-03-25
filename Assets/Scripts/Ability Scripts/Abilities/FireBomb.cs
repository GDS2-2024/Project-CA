using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBomb : MonoBehaviour
{
    private float damagePerSecond = 20f;
    private float bombDuration = 10f;
    private float lifetime = 0;
    private GameObject firingPlayer; // the player who spawned this fireobmb

    public void SetFiringPlayer(GameObject player) { firingPlayer = player; }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime >= bombDuration) { Destroy(gameObject); }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") { return; }
        if (other.gameObject != firingPlayer) // Player can't damage themselves with firebomb
        {
            PlayerStatManager statScript = other.gameObject.GetComponent<PlayerStatManager>();
            statScript.TakeDamage(damagePerSecond * Time.deltaTime, firingPlayer);
        }
    }

}
