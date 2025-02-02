using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameModeController : MonoBehaviour
{
    private GameObject playerManager;
    private PlayerManager playerManagerScript;
    private int[] playerHovers = { 1, 1, 1, 1 };
    private int playerCountCurrent = 0;
    private Button selectedBtn;
    private string[] gameModes = { "Death Match", "King of the Hill", "Life Steal" };
    private bool[] playersSelected = { false, false, false, false };
    private bool allSelected = false;
    private bool validDraw = false;
    private GameObject sceneManager;
    private SceneManagement sceneManagement;

    public List<Button> buttons = new List<Button>();
    public List<Text> playersJoined = new List<Text>();
    public List<Text> votes = new List<Text>();
    public Button drawBtn;
    public string chosenMode;
    public Text chosenModeTxt;
    public float startGameTimer;
    public Text timerTxt;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("Player Manager");
        sceneManager = GameObject.Find("SceneManager");
        sceneManagement = sceneManager.GetComponent<SceneManagement>();
        playerManagerScript = playerManager.GetComponent<PlayerManager>();
        playerManagerScript.inputDevices.Clear();
        playerManagerScript.p1Controller = null;
        playerManagerScript.p2Controller = null;
        playerManagerScript.p3Controller = null;
        playerManagerScript.p4Controller = null;
        playerManagerScript.playerCount = 0;
        drawBtn.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckNewPlayerAdded();

        CheckAllPlayersSelected();

        if (playerManagerScript.inputDevices.Count > 0)
        {
            for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
            {
                if (playerManagerScript.inputDevices[i] is Keyboard keyboard)
                {
                    if (keyboard.sKey.wasPressedThisFrame && !playersSelected[i])
                    {
                        ScrollDown(i);
                    }
                    else if (keyboard.wKey.wasPressedThisFrame && !playersSelected[i])
                    {
                        ScrollUp(i);
                    }
                    else if (keyboard.enterKey.wasPressedThisFrame)
                    {
                        ToggleSelectGameMode(i);
                    }
                    else if (keyboard.qKey.wasPressedThisFrame && allSelected && !validDraw)
                    {
                        drawBtn.onClick.Invoke();
                    }
                }
                else if (playerManagerScript.inputDevices[i] is Gamepad gamepad)
                {
                    if (gamepad.leftStick.down.wasPressedThisFrame && !playersSelected[i])
                    {
                        ScrollDown(i);
                    }
                    else if (gamepad.leftStick.up.wasPressedThisFrame && !playersSelected[i])
                    {
                        ScrollUp(i);
                    }
                    else if (gamepad.buttonSouth.wasPressedThisFrame)
                    {
                        ToggleSelectGameMode(i);
                    }
                    else if (gamepad.buttonNorth.wasPressedThisFrame && allSelected && !validDraw)
                    {
                        drawBtn.onClick.Invoke();
                    }
                }
            }
        }

        if (validDraw)
        {
            startGameTimer -= Time.deltaTime;
            timerTxt.text = Mathf.Round(startGameTimer).ToString();
            if (startGameTimer <= 0)
            {
                sceneManagement.LoadDeathMatch();
            }
        }
    }

    void CheckNewPlayerAdded()
    {
        if (playerManagerScript.playerCount > playerCountCurrent)
        {
            switch (playerCountCurrent)
            {
                case 0:
                    GameObject newP1Icon = buttons[playerHovers[0]].transform.Find("P1 Txt")?.gameObject;
                    newP1Icon.SetActive(true);
                    DisplayControls(0);
                    playerCountCurrent++;
                    break;
                case 1:
                    GameObject newP2Icon = buttons[playerHovers[1]].transform.Find("P2 Txt")?.gameObject;
                    newP2Icon.SetActive(true);
                    DisplayControls(1);
                    playerCountCurrent++;
                    break;
                case 2:
                    GameObject newP3Icon = buttons[playerHovers[2]].transform.Find("P3 Txt")?.gameObject;
                    newP3Icon.SetActive(true);
                    DisplayControls(2);
                    playerCountCurrent++;
                    break;
                case 3:
                    GameObject newP4Icon = buttons[playerHovers[3]].transform.Find("P4 Txt")?.gameObject;
                    newP4Icon.SetActive(true);
                    DisplayControls(3);
                    playerCountCurrent++;
                    break;
            }
        }
    }

    void CheckAllPlayersSelected()
    {
        for (int i = 0; i < playerCountCurrent; i++)
        {
            if (!playersSelected[i])
            {
                allSelected = false;
                drawBtn.interactable = false;
            }
        }
        if (playerCountCurrent > 0)
        {
            allSelected = true;
            drawBtn.interactable = true;
        }
    }

    void DisplayControls(int playerNo)
    {
        if (playerManagerScript.inputDevices[playerNo] is Keyboard)
        {
            playersJoined[playerNo].text = "Select with Enter";
        }
        else if (playerManagerScript.inputDevices[playerNo] is Gamepad)
        {
            playersJoined[playerNo].text = "Select with Button South";
        }
    }

    void ScrollDown(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                GameObject oldP1Icon = buttons[playerHovers[playerNo]].transform.Find("P1 Txt")?.gameObject;
                oldP1Icon.SetActive(false);
                if (playerHovers[playerNo] + 1 < buttons.Count)
                {
                    playerHovers[playerNo]++;
                }
                GameObject newP1Icon = buttons[playerHovers[playerNo]].transform.Find("P1 Txt")?.gameObject;
                newP1Icon.SetActive(true);
                break;
            case 1:
                GameObject oldP2Icon = buttons[playerHovers[playerNo]].transform.Find("P2 Txt")?.gameObject;
                oldP2Icon.SetActive(false);
                if (playerHovers[playerNo] + 1 < buttons.Count)
                {
                    playerHovers[playerNo]++;
                }
                GameObject newP2Icon = buttons[playerHovers[playerNo]].transform.Find("P2 Txt")?.gameObject;
                newP2Icon.SetActive(true);
                break;
            case 2:
                GameObject oldP3Icon = buttons[playerHovers[playerNo]].transform.Find("P3 Txt")?.gameObject;
                oldP3Icon.SetActive(false);
                if (playerHovers[playerNo] + 1 < buttons.Count)
                {
                    playerHovers[playerNo]++;
                }
                GameObject newP3Icon = buttons[playerHovers[playerNo]].transform.Find("P3 Txt")?.gameObject;
                newP3Icon.SetActive(true);
                break;
            case 3:
                GameObject oldP4Icon = buttons[playerHovers[playerNo]].transform.Find("P4 Txt")?.gameObject;
                oldP4Icon.SetActive(false);
                if (playerHovers[playerNo] + 1 < buttons.Count)
                {
                    playerHovers[playerNo]++;
                }
                GameObject newP4Icon = buttons[playerHovers[playerNo]].transform.Find("P4 Txt")?.gameObject;
                newP4Icon.SetActive(true);
                break;
        }
    }

    void ScrollUp(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                GameObject oldP1Icon = buttons[playerHovers[playerNo]].transform.Find("P1 Txt")?.gameObject;
                oldP1Icon.SetActive(false);
                if (playerHovers[playerNo] - 1 >= 0)
                {
                    playerHovers[playerNo]--;
                }
                GameObject newP1Icon = buttons[playerHovers[playerNo]].transform.Find("P1 Txt")?.gameObject;
                newP1Icon.SetActive(true);
                break;
            case 1:
                GameObject oldP2Icon = buttons[playerHovers[playerNo]].transform.Find("P2 Txt")?.gameObject;
                oldP2Icon.SetActive(false);
                if (playerHovers[playerNo] - 1 >= 0)
                {
                    playerHovers[playerNo]--;
                }
                GameObject newP2Icon = buttons[playerHovers[playerNo]].transform.Find("P2 Txt")?.gameObject;
                newP2Icon.SetActive(true);
                break;
            case 2:
                GameObject oldP3Icon = buttons[playerHovers[playerNo]].transform.Find("P3 Txt")?.gameObject;
                oldP3Icon.SetActive(false);
                if (playerHovers[playerNo] - 1 >= 0)
                {
                    playerHovers[playerNo]--;
                }
                GameObject newP3Icon = buttons[playerHovers[playerNo]].transform.Find("P3 Txt")?.gameObject;
                newP3Icon.SetActive(true);
                break;
            case 3:
                GameObject oldP4Icon = buttons[playerHovers[playerNo]].transform.Find("P4 Txt")?.gameObject;
                oldP4Icon.SetActive(false);
                if (playerHovers[playerNo] - 1 >= 0)
                {
                    playerHovers[playerNo]--;
                }
                GameObject newP4Icon = buttons[playerHovers[playerNo]].transform.Find("P4 Txt")?.gameObject;
                newP4Icon.SetActive(true);
                break;
        }
    }
    
    void ToggleSelectGameMode(int playerNo)
    {
        //If the player hasn't already selected or if they are trying to click the back button then Invoke the buttons actions
        if (!playersSelected[playerNo] || playerHovers[playerNo] == 0)
        {
            selectedBtn = buttons[playerHovers[playerNo]].GetComponent<Button>();
            selectedBtn.onClick.Invoke();
            playersSelected[playerNo] = true;
        }
        else if (playersSelected[playerNo])
        {
            for (int i = votes.Count - 1; i >= 0; i--)
            {
                Text vote = votes[i];
                if (vote.text == gameModes[playerHovers[playerNo] - 1])
                {
                    for (int j = i; j < votes.Count - 1; j++)
                    {
                        votes[j].text = votes[j + 1].text;
                    }

                    votes[votes.Count - 1].text = "-";

                    playersSelected[playerNo] = false;
                    return;
                }
            }
        }
    }

    public void AddDeathMatch(int playerNo)
    {
        for (int i = 0; i < votes.Count; i++)
        {
            if (votes[i].text == "-")
            {
                votes[i].text = gameModes[0];
                return;
            }
        }
    }

    public void AddKingOfTheHill()
    {
        for (int i = 0; i < votes.Count; i++)
        {
            if (votes[i].text == "-")
            {
                votes[i].text = gameModes[1];
                return;
            }
        }
    }

    public void AddLifeSteal()
    {
        for (int i = 0; i < votes.Count; i++)
        {
            if (votes[i].text == "-")
            {
                votes[i].text = gameModes[2];
                return;
            }
        }
    }

    public void Draw()
    {
        int randomDraw = Random.Range(0, votes.Count);

        while (validDraw == false)
        {
            if (votes[randomDraw].text == "-")
            {
                randomDraw = Random.Range(0, votes.Count);
            }
            else
            {
                validDraw = true;
            }
        }
        chosenMode = votes[randomDraw].text;

        chosenModeTxt.text = chosenMode;
    }
}
