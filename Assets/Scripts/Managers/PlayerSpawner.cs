using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public GameObject playerPrefab;

    private GameObject playerManager;
    private PlayerManager playerManagerScript;
    public int playerCount = 0;
    private List<GameObject> players = new List<GameObject>();
    private GameObject newPlayer;
    private Camera thisCam;
    private PlayerController controllerScript;
    private InputDevice thisController;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("Player Manager");
        playerManagerScript = playerManager.GetComponent<PlayerManager>();

        playerCount = playerManagerScript.playerCount;

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

            if (playerCount <= 2)
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
