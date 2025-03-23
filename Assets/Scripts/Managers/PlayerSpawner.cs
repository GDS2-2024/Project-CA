using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerScore;

public class PlayerSpawner : MonoBehaviour
{
    // Spawning
    [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    private float spawnTimer = 5.0f;
    public SpawnLogic gameModeSpawnLogic;

    // Players
    private PlayerManager playerManager;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    public GameObject blankCameraPrefab;
    public int playerCount = 0;
    public List<GameObject> GetPlayersInGame() { return players; }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        playerCount = playerManager.playerCount;
        spawnPoints = gameObject.GetComponentsInChildren<SpawnPoint>().ToList<SpawnPoint>();
        if (gameModeSpawnLogic == null)
        {
            Debug.LogError("Please assign a gamemode specific spawn logic!");
            return;
        }
        if (spawnPoints.Count < 4) { Debug.LogError("There is not enough unique spawn points for 4 players."); }
        else SpawnPlayers();
    }

    // Instantiate players and position them at a spawn point
    private void SpawnPlayers()
    {
        for (int playerNumber = 0; playerNumber < playerCount; playerNumber++)
        {
            // Instantiate new player object and insert at the same position in list
            Transform randomSpawn = GetSpawnpoint().transform;
            GameObject newPlayer = Instantiate(playerManager.GetSelectedCharacter(playerNumber),
                    randomSpawn.position, randomSpawn.rotation);
            players.Insert(playerNumber, newPlayer);

            // Setup Camera & Controller
            PlayerMoveBase playerMove = newPlayer.GetComponent<PlayerMoveBase>();
            Quaternion initialRotation = randomSpawn.rotation;
            playerMove.InitializeCameraRotation(initialRotation.eulerAngles.y, initialRotation.eulerAngles.x);
            Camera thisCam = newPlayer.GetComponentInChildren<Camera>();
            InputDevice thisController = playerManager.inputDevices[playerNumber];
            PlayerController controllerScript = newPlayer.GetComponent<PlayerController>();
            controllerScript.SetController(thisController);

            SetupSplitScreen(thisCam, playerNumber);
        }
    }

    // Setup the positioning of the cameras based on the number of players
    private void SetupSplitScreen(Camera thisCam, int playerNumber)
    {
        if (playerCount == 1)
        {
            //Handle 1 player mode
            thisCam.rect = new Rect(0f, 0f, 1f, 1f);
        }
        else if (playerCount == 2)
        {
            //Handle 2 player mode
            switch (playerNumber)
            {
                case 0:
                    thisCam.rect = new Rect(0f, 0f, 0.5f, 1f);
                    break;
                case 1:
                    thisCam.rect = new Rect(0.5f, 0f, 0.5f, 1f);
                    break;
            }
        }
        else
        {
            //Handle 3 or 4 player mode
            switch (playerNumber)
            {
                case 0:
                    thisCam.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                    break;
                case 1:
                    thisCam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                case 2:
                    thisCam.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                    if (playerCount == 3) { GameObject blankCam = Instantiate(blankCameraPrefab); }
                    break;
                case 3:
                    thisCam.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                    break;
            }
        }
    }

    // Respawn a specified player given their InputDevice and Player Object 
    public void RespawnPlayer(InputDevice playerController, GameObject playerObject)
    {
        // Remove current player object from list
        int playerNumber = players.IndexOf(playerObject);
        players.Remove(playerObject);

        // Instantiate new player object and insert at the same position in list
        Transform randomSpawn = GetSpawnpoint().transform;
        GameObject newPlayer = Instantiate(playerManager.GetSelectedCharacter(playerNumber),
                randomSpawn.position, randomSpawn.rotation);
        players.Insert(playerNumber, newPlayer);

        // Pass player score to new player object & update UI
        MatchScore oldMatchScore = playerObject.GetComponent<PlayerScore>().GetPlayerScore();
        PlayerScore newPlayerScore = newPlayer.GetComponent<PlayerScore>();
        newPlayerScore.PassPlayerScore(oldMatchScore);
        int intScore = Convert.ToInt32(newPlayerScore.GetObjectiveScore());
        newPlayer.GetComponent<PlayerHUD>().UpdateObjectiveScore(intScore);

        // Detroy old player object
        Destroy(playerObject);

        // Setup Controller
        PlayerController controllerScript = newPlayer.GetComponent<PlayerController>();
        controllerScript.SetController(playerController);

        // Setup Camera
        PlayerMoveBase playerMove = newPlayer.GetComponent<PlayerMoveBase>();
        Quaternion initialRotation = randomSpawn.rotation;
        playerMove.InitializeCameraRotation(initialRotation.eulerAngles.y, initialRotation.eulerAngles.x);
        Camera thisCam = newPlayer.GetComponentInChildren<Camera>();
        SetupSplitScreen(thisCam, playerNumber);
    }

    private SpawnPoint GetSpawnpoint()
    {
        SpawnPoint chosenSpawn = gameModeSpawnLogic.GetSpawnPosition(spawnPoints, players);
        StartCoroutine(ReserveSpawnpoint(chosenSpawn));

        return chosenSpawn;
    }

    private IEnumerator ReserveSpawnpoint(SpawnPoint spawnpoint) 
    {
        // Prevent multiple players from spawning at the same spawn point
        spawnpoint.spawnAvailable = false;
        yield return new WaitForSeconds(spawnTimer);
        spawnpoint.spawnAvailable = true;
    }



}
