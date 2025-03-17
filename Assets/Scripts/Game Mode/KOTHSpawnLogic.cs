using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KOTHSpawnLogic : SpawnLogic
{
    private KOTHManager KothManager;

    private void Awake()
    {
        KothManager = gameObject.GetComponent<KOTHManager>();
    }

    public override SpawnPoint GetSpawnPosition(List<SpawnPoint> spawnPoints)
    {
        // Check there is enough available spawns (not already in use)
        var availableSpawns = spawnPoints.Where(spawnPoint => spawnPoint.spawnAvailable).ToList();
        if (availableSpawns.Count == 0) return null;

        // Get the 2nd closest available spawn to the hill
        if (!KothManager) { KothManager = gameObject.GetComponent<KOTHManager>(); }
        if (!KothManager.activeHill) { KothManager.activeHill = KothManager.hills[0]; }
        Vector3 hillPosition = KothManager.activeHill.gameObject.transform.position;

        List<SpawnPoint> sortedSpawns = availableSpawns
        .OrderBy(spawnPoint => Vector3.Distance(spawnPoint.transform.position, hillPosition))
        .ToList();
        return sortedSpawns[1];
    }
}
