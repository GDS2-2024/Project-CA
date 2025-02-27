using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private PlayerHUD playerHUD;
    private MatchScore matchScore;

    public struct MatchScore
    {
        [SerializeField] public float objectiveScore;
        [SerializeField] public int playerKills;
        [SerializeField] public int playerDeaths;
    }

    // Used to pass score to new player obj after respawning
    public void PassPlayerScore(MatchScore newMatchScore)
    {
        matchScore = newMatchScore;
    }
    public MatchScore GetPlayerScore()
    {
        return matchScore;
    }

    public void AddObjectiveScore(float score)
    {
        matchScore.objectiveScore += score;
        int intScore = Convert.ToInt32(matchScore.objectiveScore);
        playerHUD.UpdateObjectiveScore(intScore);
    }
    public float GetObjectiveScore() { return matchScore.objectiveScore; }

    public void AddPlayerKill() { matchScore.playerKills++; }
    public int GetPlayerKills() { return matchScore.playerKills; }

    public void AddPlayerDeath() { matchScore.playerDeaths++; }
    public int GetPlayerDeaths() { return matchScore.playerDeaths; }

    private void Start()
    {
        playerHUD = gameObject.GetComponent<PlayerHUD>();
    }

}
