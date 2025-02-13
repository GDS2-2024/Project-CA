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

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("Player Manager");
        playerManagerScript = playerManager.GetComponent<PlayerManager>();

        playerCount = playerManagerScript.playerCount;

        //spawn characters at their spawn points
        for (int i = 1; i <= playerCount; i++)
        {
            newPlayer = Instantiate(playerPrefab, spawnPoints[i].transform);
            players.Add(newPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
