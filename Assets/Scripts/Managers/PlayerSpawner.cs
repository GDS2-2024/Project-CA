using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("Player Manager");
        playerManagerScript = playerManager.GetComponent<PlayerManager>();

        playerCount = playerManagerScript.playerCount;

        //spawn characters at their spawn points
        for (int i = 0; i <= playerCount; i++)
        {
            newPlayer = Instantiate(playerPrefab, spawnPoints[i].transform);
            players.Add(newPlayer);

            //Handle Split Screen
            thisCam = newPlayer.GetComponentInChildren<Camera>();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
