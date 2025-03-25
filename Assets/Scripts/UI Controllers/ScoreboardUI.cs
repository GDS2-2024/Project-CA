using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    public List<TMP_Text> PlayerNames;
    public List<TMP_Text> PlayerScores;
    public List<TMP_Text> PlayerKills;
    public List<TMP_Text> PlayerDeaths;

    public void PopulateScoreboard(List<PlayerScore> playerScores)
    {
        int index = 0;
        foreach (PlayerScore playerScore in playerScores)
        {
            PlayerNames[index].text = playerScore.gameObject.name;
            PlayerScores[index].text = ((int)playerScore.GetObjectiveScore()).ToString();
            PlayerKills[index].text = playerScore.GetPlayerKills().ToString();
            PlayerDeaths[index].text = playerScore.GetPlayerDeaths().ToString();
            index++;
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
