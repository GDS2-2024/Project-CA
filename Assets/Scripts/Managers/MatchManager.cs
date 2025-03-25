using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private ScoreBoardData scoreBoardData;
    private ScoreboardUI scoreBoardUI;

    // Start is called before the first frame update
    void Start()
    {
        GameObject sbManager = GameObject.Find("Scoreboard Manager");
        if (sbManager == null)
        {
            Debug.LogError("Scoreboard Manager GameObject not found in the scene!");
        }

        scoreBoardData = sbManager?.GetComponent<ScoreBoardData>();
        if (scoreBoardData == null)
        {
            Debug.LogError("ScoreBoardData component is missing on Scoreboard Manager!");
        }

        scoreBoardUI = scoreBoardData?.GetComponent<ScoreboardUI>();
        if (scoreBoardUI == null)
        {
            Debug.LogError("ScoreboardUI component is missing on Scoreboard Manager!");
        }

        scoreBoardData = GameObject.Find("Scoreboard Manager").GetComponent<ScoreBoardData>();
        scoreBoardUI = scoreBoardData.GetComponent<ScoreboardUI>();
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
        PlayerScore winningPlayer = scoreBoardData.GetHighestPlayerScore();
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
        GameObject winningPlayer = scoreBoardData.GetHighestPlayerScore().gameObject;
        Debug.Log("Winning player is: " + winningPlayer);
        scoreBoardUI.PopulateScoreboard(scoreBoardData.GetAllPlayerScores());
        Invoke("LoadMenu", 10.0f);
    }

    private void LoadMenu()
    {
        SceneManagement sceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManagement>();
        sceneManager.ReturnToMenu();
    }

}
