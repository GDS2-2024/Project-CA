using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public GameObject playerPrefab;
    public GameObject blankCameraPrefab; // Used to render black pixels if there are only 3 players

    private GameObject playerManager;
    private PlayerManager playerManagerScript;
    public int playerCount = 0;
    private List<GameObject> players = new List<GameObject>();
    private GameObject newPlayer;
    private Camera thisCam;
    private PlayerController controllerScript;
    private InputDevice thisController;

    public List<GameObject> GetPlayersInGame() { return players; }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        playerManager = GameObject.Find("Player Manager");
        playerManagerScript = playerManager.GetComponent<PlayerManager>();

        playerCount = playerManagerScript.playerCount;
        if (playerCount == 3) { GameObject blankCam = Instantiate(blankCameraPrefab); }

        //spawn characters at their spawn points
        for (int i = 1; i <= playerCount; i++)
        {
            newPlayer = Instantiate(playerPrefab, spawnPoints[i - 1].transform);
            players.Add(newPlayer);

            //Handle Split Screen
            thisCam = newPlayer.GetComponentInChildren<Camera>();
            controllerScript = newPlayer.GetComponent<PlayerController>();
            thisController = playerManagerScript.inputDevices[i - 1];
            controllerScript.SetController(thisController);

            if (playerCount == 1)
            {
                //Handle 1 player mode
                thisCam.rect = new Rect(0f, 0f, 1f, 1f);
            }
            else if (playerCount == 2)
            {
                //Handle 2 player mode
                switch (i)
                {
                    case 1:
                        thisCam.rect = new Rect(0f, 0f, 0.5f, 1f);
                        break;
                    case 2:
                        thisCam.rect = new Rect(0.5f, 0f, 0.5f, 1f);
                        break;
                }
            } else
            {
                //Handle 3 or 4 player mode
                switch (i)
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
    }
}
