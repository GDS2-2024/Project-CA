using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Handles rotation of active hill
/// </summary>
public class KOTHManager : MonoBehaviour
{
    [SerializeField] private float remainingHillTime;
    public List<GameObject> hills;
    public GameObject activeHill;
    private int hillIndex = -1;

    private List<GameObject> playerList;
    private PlayerSpawner playerSpawner;
    private MatchManager matchManager;

    private void Awake()
    {
        ShuffleHillOrder();
        activeHill = hills[0];
    }

    // Randomises the orderr of the hills every game
    private void ShuffleHillOrder()
    {
        for (int i = 0; i < hills.Count; i++)
        {
            int randomIndex = Random.Range(i, hills.Count);
            GameObject temp = hills[i];
            hills[i] = hills[randomIndex];
            hills[randomIndex] = temp;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();
        playerList = playerSpawner.GetPlayersInGame();
        matchManager = GameObject.Find("Match Manager").GetComponent<MatchManager>();

        SwitchToNextHill();
        EnableObjectiveMarker();
    }

    // Update is called once per frame
    void Update()
    {
        if (!matchManager.hasGameFinished)
        {
            UpdateHillTimer();
            UpdateObjectiveDirection();
        }      
    }

    private void SwitchToNextHill()
    {
        if (hills.Count == 0)
        {
            Debug.Log("ERROR: Objective Hills List is empty.");
            return;
        }

        // Hide previous hill
        if (activeHill != null) activeHill.GetComponent<HillObjective>().SetVisibility(false);

        hillIndex++;
        // Loop to start of list
        if (hillIndex >= hills.Count) { hillIndex = 0; }                               
        
        // Assign and show new hill
        activeHill = hills[hillIndex];
        activeHill.GetComponent<HillObjective>().SetVisibility(true);
        if (matchManager.remainingGameTime != 360) { TellPlayersNewHill(); } // ignore at game start
        EnableObjectiveMarker();
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

    // Add text to HUD for all players telling them that the hill has moved.
    private void TellPlayersNewHill()
    {
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

    // Updates direction to hill in player's compass
    private void UpdateObjectiveDirection()
    {
        foreach (GameObject player in playerList)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 hillPosition = activeHill.transform.position;

            // Get the direction to the hill
            Vector3 directionToHill = hillPosition - playerPosition;
            directionToHill.y = 0;

            // Get the player's forward direction on the horizontal plane
            Vector3 playerForward = player.transform.forward;
            playerForward.y = 0;

            // Compute the angle between the player's forward direction and the hill
            float angle = Vector3.SignedAngle(playerForward, directionToHill, Vector3.up);

            PlayerHUD hud = player.GetComponent<PlayerHUD>();
            hud.SetCompassObjective(activeHill.transform.position);
        }
    }

    // Enables the objective marker on the compass (disabled by default for other gamemodes)
    private void EnableObjectiveMarker()
    {
        foreach (GameObject player in playerList)
        {
            PlayerHUD hud = player.GetComponent<PlayerHUD>();
            hud.SetActiveCompassObjective(true);
        }
    }
}
