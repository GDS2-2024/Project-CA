using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOTHManager : MonoBehaviour
{
    [SerializeField] private float remainingGameTime;
    [SerializeField] private float remainingHillTime;
    [SerializeField] private List<GameObject> hills;
    [SerializeField] private GameObject activeHill;
    private int hillIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        SwitchToNextHill();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameTimer();
        UpdateHillTimer();
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
            EndGame();
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

    private void EndGame()
    {
        Debug.Log("Timer ended, Game Over!");
    }

}
