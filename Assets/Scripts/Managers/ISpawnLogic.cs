using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnLogic : MonoBehaviour
{
    public abstract SpawnPoint GetSpawnPosition(List<SpawnPoint> spawnPoints);
}

