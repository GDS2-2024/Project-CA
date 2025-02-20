using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerScore GetHighestPlayerScore()
    {
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
}
