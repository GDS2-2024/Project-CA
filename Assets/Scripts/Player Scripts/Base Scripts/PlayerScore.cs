using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private float objectiveScore;
    [SerializeField] private int playerKills;
    [SerializeField] private int playerDeaths;

    public void AddObjectiveScore(float score) { objectiveScore += score; }
    public float GetObjectiveScore() { return objectiveScore; }

    public void AddPlayerKill() { playerKills++; }
    public int GetPlayerKills() { return playerKills; }

    public void AddPlayerDeath() { playerDeaths++; }
    public int GetPlayerDeaths() { return playerDeaths; }

}
