using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLava : Ability
{
    private float duration = 10f;
    public Material lavaMaterial;
    public GameObject lavaPrefab;
    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    private List<GameObject> lavaList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //StartCooldown();
    }

    public override void OnPressAbility()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(ActivateLava());
            StartCooldown();
        }
    }

    private IEnumerator ActivateLava()
    {
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");

        foreach (GameObject ground in groundObjects)
        {
            // Change material temporarily
            Renderer renderer = ground.GetComponent<Renderer>();
            if (renderer)
            {
                // Store original material
                originalMaterials[ground] = renderer.material;
                renderer.material = lavaMaterial;
                renderer.material.mainTextureScale = new Vector2(10f, 10f);
            }

            // Spawn lava prefab
            CreateLavaObject(ground);
        }
        yield return new WaitForSeconds(duration);
        ResetLava();
    }

    private void CreateLavaObject(GameObject ground)
    {
        if (lavaPrefab == null) return;

        GameObject lavaInstance = Instantiate(lavaPrefab, ground.transform.position, ground.transform.rotation);
        lavaInstance.transform.localScale = new Vector3(ground.transform.lossyScale.x, 1, ground.transform.lossyScale.z);
        lavaList.Add(lavaInstance);
    }

    private void ResetLava()
    {
        // Restore original materials
        foreach (var entry in originalMaterials)
        {
            if (entry.Key)
            {
                entry.Key.GetComponent<Renderer>().material = entry.Value;
            }
        }
        originalMaterials.Clear();

        // Delete lava objects
        foreach (GameObject lava in lavaList)
        {
            Destroy(lava);
        }
        lavaList.Clear();
    }

    public override void OnHoldingAbility()
    {
        // This ability does not need this function
    }

    public override void OnReleaseAbility()
    {
        // This ability does not need this function
    }

}
