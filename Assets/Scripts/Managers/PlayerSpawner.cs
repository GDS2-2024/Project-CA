using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerScore;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> characterPrefabs;

    public GameObject blankCameraPrefab; // Used to render black pixels if there are only 3 players

    private GameObject playerManager;
    private PlayerManager playerManagerScript;
    public int playerCount = 0;
    [SerializeField] private List<GameObject> players = new List<GameObject>();

    public List<GameObject> GetPlayersInGame() { return players; }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        playerManager = GameObject.Find("Player Manager");
        playerManagerScript = playerManager.GetComponent<PlayerManager>();

        playerCount = playerManagerScript.playerCount;

        SpawnPlayers();
    }

    // Instantiate players and position them at a spawn point
    private void SpawnPlayers()
    {
        for (int playerNumber = 1; playerNumber <= playerCount; playerNumber++)
        {
            // Instantiate player & add to player list
            GameObject newPlayer = Instantiate(characterPrefabs[0], spawnPoints[playerNumber - 1].transform);
            players.Add(newPlayer);

            // Setup Camera & Controller
            Camera thisCam = newPlayer.GetComponentInChildren<Camera>();
            InputDevice thisController = playerManagerScript.inputDevices[playerNumber - 1];
            PlayerController controllerScript = newPlayer.GetComponent<PlayerController>();
            controllerScript.SetController(thisController);

            SetupSplitScreen(thisCam, playerNumber);
        }
    }

    // Setup the positioning of the cameras based on the number of players
    private void SetupSplitScreen(Camera thisCam, int playerNumber)
    {
        if (playerCount == 3) { GameObject blankCam = Instantiate(blankCameraPrefab); }
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
                case 1:
                    thisCam.rect = new Rect(0f, 0f, 0.5f, 1f);
                    break;
                case 2:
                    thisCam.rect = new Rect(0.5f, 0f, 0.5f, 1f);
                    break;
            }
        }
        else
        {
            //Handle 3 or 4 player mode
            switch (playerNumber)
            {
                case 1:
                    thisCam.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                    break;
                case 2:
                    thisCam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                case 3:
                    thisCam.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                    break;
                case 4:
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
        GameObject newPlayer = Instantiate(characterPrefabs[0], spawnPoints[playerNumber].transform);
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
        Camera thisCam = newPlayer.GetComponentInChildren<Camera>();
        SetupSplitScreen(thisCam, playerNumber+1);
    }

}
