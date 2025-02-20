using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOTHManager : MonoBehaviour
{
    public bool hasGameFinished = false;
    [SerializeField] private float remainingGameTime;
    [SerializeField] private float remainingHillTime;
    [SerializeField] private int scoreToWin;
    [SerializeField] private List<GameObject> hills;
    [SerializeField] private GameObject activeHill;
    private int hillIndex = -1;

    private List<GameObject> playerList;
    // Used to retrieve player list:
    private GameObject playerSpawner;
    private PlayerSpawner playerSpawnerScript;

    private GameObject scoreBoardObject;
    private ScoreBoardManager scoreBoardManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreBoardObject = GameObject.Find("Scoreboard Manager");
        scoreBoardManager = scoreBoardObject.GetComponent<ScoreBoardManager>();

        playerSpawner = GameObject.Find("Player Spawner");
        playerSpawnerScript = playerSpawner.GetComponent<PlayerSpawner>();
        playerList = playerSpawnerScript.GetPlayersInGame();
        SwitchToNextHill();
        InvokeRepeating("CheckIfPlayerHasWon", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasGameFinished)
        {
            UpdateGameTimer();
            UpdateHillTimer();
        }      
    }

    private void SwitchToNextHill()
    {
        Debug.Log("Loading Next Hill...");
        if (hills.Count == 0)
        {
            Debug.Log("ERROR: Objective Hills List is empty.");
            return;
        }

        // Hide previous hill
        if (activeHill != null) activeHill.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        hillIndex++;
        // Loop to start of list
        if (hillIndex >= hills.Count) { hillIndex = 0; }                               
        
        // Assign and show new hill
        activeHill = hills[hillIndex];
        activeHill.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

    }

    private void UpdateGameTimer()
    {
        remainingGameTime -= Time.deltaTime;
        if (remainingGameTime < 0)
        {
            TimerEnd();
        }
    }

    private void UpdateHillTimer()
    {
        remainingHillTime -= Time.deltaTime;
        if (remainingHillTime < 0)
        {
            remainingHillTime = 60;
            SwitchToNextHill();
        }
    }

    public void AddScoreToPlayer(GameObject player)
    {
        player.GetComponent<PlayerScore>().AddObjectiveScore(Time.deltaTime);
    }

    private void TimerEnd()
    {
        Debug.Log("Game Timer ended, checking who won.");
        hasGameFinished = true;
        CancelInvoke("CheckIfPlayerHasWon");
        GameObject winningPlayer = scoreBoardManager.GetHighestPlayerScore().gameObject;
        Debug.Log("Winning player is: " + winningPlayer);
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

}
