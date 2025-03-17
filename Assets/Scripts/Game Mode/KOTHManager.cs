using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOTHManager : MonoBehaviour
{
    public bool hasGameFinished = false;
    [SerializeField] private float remainingGameTime;
    [SerializeField] private float remainingHillTime;
    [SerializeField] private int scoreToWin;
    public List<GameObject> hills;
    public GameObject activeHill;
    private int hillIndex = -1;

    private List<GameObject> playerList;
    // Used to retrieve player list:
    private GameObject playerSpawner;
    private PlayerSpawner playerSpawnerScript;

    private GameObject scoreBoardObject;
    private ScoreBoardManager scoreBoardManager;

    private void Awake()
    {
        activeHill = hills[0];
    }

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
        InvokeRepeating("UpdateAllPlayerTimerHUDs", 0.0f, 1.0f);
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
        if (remainingGameTime != 360) { TellPlayersNewHill(); } // ignore at game start
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

    // Add text to HUD for all players telling them that the hill has moved.
    private void TellPlayersNewHill()
    {
        Debug.Log("Telling players the Hill has moved.");
        foreach (GameObject player in playerList)
        {
            PlayerHUD hud = player.GetComponent<PlayerHUD>();
            hud.UpdateObjectivePrompt("The Hill has moved!", 5.0f);
        }
    }

    public void AddScoreToPlayer(GameObject player)
    {
        PlayerScore playerScore = player.GetComponent<PlayerScore>();
        if (playerScore.enabled == true) { playerScore.AddObjectiveScore(Time.deltaTime); }
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
