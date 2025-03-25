using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreBoardData : MonoBehaviour
{
    private List<GameObject> playerList;
    [SerializeField] private List<PlayerScore> playerScores;

    // Used to retrieve player list
    private GameObject playerSpawner;
    private PlayerSpawner playerSpawnerScript;

    // Start is called before the first frame update
    void Start()
    {
        // Get a list of players and populate the scoreboard with player scores
        playerSpawner = GameObject.Find("Player Spawner");
        playerSpawnerScript = playerSpawner.GetComponent<PlayerSpawner>();
        playerList = playerSpawnerScript.GetPlayersInGame();
        foreach (GameObject player in playerList)
        {
            playerScores.Add(player.GetComponent<PlayerScore>());
        }
    }

    public PlayerScore GetHighestPlayerScore()
    {
        RefreshPlayerList();
        float currentBestScore = 0;
        PlayerScore currentBestPlayer = null;
        foreach (PlayerScore playerScore in playerScores)
        {
            if (playerScore.GetObjectiveScore() >= currentBestScore)
            {
                currentBestScore = playerScore.GetObjectiveScore();
                currentBestPlayer = playerScore;
            }
        }
        return currentBestPlayer;
    }

    public List<PlayerScore> GetAllPlayerScores()
    {
        RefreshPlayerList();
        return playerScores;
    }

    private void RefreshPlayerList()
    {
        if (playerSpawnerScript == null) return;

        // Remove null entries (players who were destroyed)
        playerScores.RemoveAll(player => player == null);

        // Get the updated player list from the spawner
        playerList = playerSpawnerScript.GetPlayersInGame();

        foreach (GameObject player in playerList)
        {
            PlayerScore playerScore = player.GetComponent<PlayerScore>();
            if (playerScore != null && !playerScores.Contains(playerScore))
            {
                playerScores.Add(playerScore);
            }
        }
        playerScores = playerScores.OrderByDescending(player => player.GetObjectiveScore()).ToList();
    }
}
