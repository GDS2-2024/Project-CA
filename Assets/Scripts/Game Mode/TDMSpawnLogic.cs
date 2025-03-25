using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TDMSpawnLogic : SpawnLogic
{
    public override SpawnPoint GetSpawnPosition(List<SpawnPoint> spawnPoints, List<GameObject> players)
    {
        List<SpawnPoint> availableSpawns = spawnPoints.Where(sp => sp.spawnAvailable).ToList();

        // Filter out spawn points that are too close to any player
        availableSpawns = availableSpawns
            .Where(spawn => !players.Any(player => Vector3.Distance(spawn.transform.position, player.transform.position) < minDistanceFromPlayers))
            .ToList();

        // Fallback to all spawns if no available spawns
        if (availableSpawns.Count == 0)
        {
            int randomIndex1 = UnityEngine.Random.Range(0, spawnPoints.Count);
            return spawnPoints[randomIndex1];
        }

        // Random spawn that isn't too close to another player
        int randomIndex = UnityEngine.Random.Range(0, availableSpawns.Count);
        SpawnPoint chosenSpawn = availableSpawns[randomIndex];
        
        return chosenSpawn;
    }
}
