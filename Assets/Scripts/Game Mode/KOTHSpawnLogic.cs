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

    public override SpawnPoint GetSpawnPosition(List<SpawnPoint> spawnPoints, List<GameObject> players)
    {
        // Check there is enough available spawns (not already in use)
        var availableSpawns = spawnPoints.Where(spawnPoint => spawnPoint.spawnAvailable).ToList();
        if (availableSpawns.Count == 0) return null;

        // Filter out spawn points that are too close to any player
        availableSpawns = availableSpawns
            .Where(spawn => !players.Any(player => Vector3.Distance(spawn.transform.position, player.transform.position) < minDistanceFromPlayers))
            .ToList();

        // Get the 2nd closest available spawn to the hill
        if (!KothManager) { KothManager = gameObject.GetComponent<KOTHManager>(); }
        if (!KothManager.activeHill) { KothManager.activeHill = KothManager.hills[0]; }
        Vector3 hillPosition = KothManager.activeHill.gameObject.transform.position;

        List<SpawnPoint> sortedSpawns = availableSpawns
        .OrderBy(spawnPoint => Vector3.Distance(spawnPoint.transform.position, hillPosition))
        .ToList();
        if (availableSpawns.Count == 0) {
            int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            return spawnPoints[randomIndex];
        }
        return sortedSpawns[1];
    }
}
