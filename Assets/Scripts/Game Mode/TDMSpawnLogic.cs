using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TDMSpawnLogic : SpawnLogic
{
    public override SpawnPoint GetSpawnPosition(List<SpawnPoint> spawnPoints)
    {
        // Pick a random spawn point
        List<SpawnPoint> availableSpawns = spawnPoints.Where(sp => sp.spawnAvailable).ToList();
        if (availableSpawns.Count == 0)
        {
            Debug.LogWarning("No available spawn points!");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, availableSpawns.Count);
        SpawnPoint chosenSpawn = availableSpawns[randomIndex];
        return chosenSpawn;
    }
}
