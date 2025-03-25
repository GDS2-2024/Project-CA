using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the game timer, handles game win logic
/// </summary>
public class MatchManager : MonoBehaviour
{
    public bool hasGameFinished = false;
    public float remainingGameTime;
    public int scoreToWin;

    private List<GameObject> playerList;
    private PlayerSpawner playerSpawner;
    private ScoreBoardManager scoreBoardManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreBoardManager = GameObject.Find("Scoreboard Manager").GetComponent<ScoreBoardManager>();
        playerSpawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();
        playerList = playerSpawner.GetPlayersInGame();

        InvokeRepeating("CheckIfPlayerHasWon", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameTimer();
    }

    private void UpdateGameTimer()
    {
        remainingGameTime -= Time.deltaTime;
        if (remainingGameTime < 0)
        {
            HandleGameEnd();
        }
        UpdateAllPlayerTimerHUDs();
    }

    // Updates the game timer on the players HUDs
    private void UpdateAllPlayerTimerHUDs()
    {
        foreach (GameObject player in playerList)
        {
            PlayerHUD hud = player.GetComponent<PlayerHUD>();

            int minutes = Mathf.FloorToInt(remainingGameTime / 60); // Get minutes
            int seconds = Mathf.FloorToInt(remainingGameTime % 60); // Get remaining seconds

            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            hud.UpdateGameTimer(formattedTime);
        }
    }

    private void CheckIfPlayerHasWon()
    {
        PlayerScore winningPlayer = scoreBoardManager.GetHighestPlayerScore();
        if (winningPlayer.GetObjectiveScore() >= scoreToWin)
        {
            Debug.Log("Player has reached score to win.");
            CancelInvoke("CheckIfPlayerHasWon");
            hasGameFinished = true;
        }
    }
    private void HandleGameEnd()
    {
        hasGameFinished = true;
        CancelInvoke("CheckIfPlayerHasWon");
        GameObject winningPlayer = scoreBoardManager.GetHighestPlayerScore().gameObject;
        Debug.Log("Winning player is: " + winningPlayer);
        // TO DO: show a scoreboard then load back to main menu
    }
}
