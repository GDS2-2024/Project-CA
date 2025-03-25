using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnLogic : MonoBehaviour
{
    public abstract SpawnPoint GetSpawnPosition(List<SpawnPoint> spawnPoints, List<GameObject> players);
    public float minDistanceFromPlayers = 6.0f;
}

