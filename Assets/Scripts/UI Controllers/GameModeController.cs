using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameModeController : MonoBehaviour
{
    private GameObject playerManager;
    private PlayerManager playerManagerScript;
    private int[] playerHovers = { 0, 0, 0, 0 };
    public List<GameObject> allPlayerHoverObjs;
    private GameObject selectedBtn;
    private string[] gameModes = { "Death Match", "King of the Hill", "Life Steal" };
    private bool[] playersSelected = { false, false, false, false };
    public bool allSelected = false;
    private bool validDraw = false;
    private GameObject sceneManager;
    private SceneManagement sceneManagement;

    public List<GameObject> gameModeButtons;
    public List<string> votes;
    public GameObject PlayerVotesObj;
    public List<TMP_Text> playerVotesText;
    public string chosenMode;
    public TMP_Text chosenModeTxt;
    public float startGameTimer;
    public TMP_Text timerTxt;

    private List<float> holdTime = new List<float>();
    const float requiredHoldDuration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("Player Manager");
        sceneManager = GameObject.Find("SceneManager");
        sceneManagement = sceneManager.GetComponent<SceneManagement>();
        playerManagerScript = playerManager.GetComponent<PlayerManager>();
        SetupPlayerIcons(); 

        // Initialise hold times
        for (int i = 0; i < 4; i++)
        {
            holdTime.Add(0f);
        }
    }

    public void ResetGameModeMenu()
    {
        playersSelected = new bool[] { false, false, false, false };
        playerHovers = new int[] { 0, 0, 0, 0 };
        allSelected = false;
        validDraw = false;
        startGameTimer = 3;
        timerTxt.text = "";
        chosenModeTxt.text = "A random gamemode will be chosen based on votes";
        PlayerVotesObj.SetActive(true);
        for (int i = 0; i < votes.Count; i++)
        {
            votes[i] = "-";
        }
        for (int i = 0; i < playerVotesText.Count; i++)
        {
            int playerNum = i + 1;
            playerVotesText[i].text = "Player " + playerNum + " - ";
        }
        foreach (GameObject obj in allPlayerHoverObjs)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckAllPlayersSelected();
        ProcessPlayerInputs();
        HandleGameStart();
    }

    void ProcessPlayerInputs()
    {
        for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
        {
            var device = playerManagerScript.inputDevices[i];
            if (device is Keyboard keyboard) { HandleKeyboardInput(keyboard, i); }
            else if (device is Gamepad gamepad) { HandleGamepadInput(gamepad, i); }
        }
    }

    void HandleKeyboardInput(Keyboard keyboard, int playerIndex)
    {
        if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame) { ScrollDown(playerIndex); }
        else if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame) { ScrollUp(playerIndex); }
        else if (keyboard.enterKey.wasPressedThisFrame) { ToggleSelectGameMode(playerIndex); }
    }

    void HandleGamepadInput(Gamepad gamepad, int playerIndex)
    {
        if (gamepad.leftStick.down.wasPressedThisFrame || gamepad.dpad.down.wasPressedThisFrame) { ScrollDown(playerIndex); }
        else if (gamepad.leftStick.up.wasPressedThisFrame || gamepad.dpad.up.wasPressedThisFrame) { ScrollUp(playerIndex); }
        else if (gamepad.buttonSouth.wasPressedThisFrame) { ToggleSelectGameMode(playerIndex); }
    }

    void HandleGameStart()
    {
        if (!validDraw) return;

        startGameTimer -= Time.deltaTime;
        timerTxt.text = Mathf.Round(startGameTimer).ToString();

        if (startGameTimer > 0) return;

        switch (chosenMode)
        {
            case "Death Match":
                sceneManagement.LoadDeathMatch();
                break;
            case "King of the Hill":
                sceneManagement.LoadKingOfTheHill();
                break;
        }
    }

    public void SetupPlayerIcons()
    {
        if (!playerManagerScript) { return; }
        for (int i = 1; i <= playerManagerScript.playerCount; i++)
        {
            switch (i)
            {
                case 1:
                    GameObject newP1Icon = gameModeButtons[playerHovers[0]].transform.Find("P1 Txt")?.gameObject;
                    newP1Icon.SetActive(true);
                    break;
                case 2:
                    GameObject newP2Icon = gameModeButtons[playerHovers[1]].transform.Find("P2 Txt")?.gameObject;
                    newP2Icon.SetActive(true);
                    break;
                case 3:
                    GameObject newP3Icon = gameModeButtons[playerHovers[2]].transform.Find("P3 Txt")?.gameObject;
                    newP3Icon.SetActive(true);
                    break;
                case 4:
                    GameObject newP4Icon = gameModeButtons[playerHovers[3]].transform.Find("P4 Txt")?.gameObject;
                    newP4Icon.SetActive(true);
                    break;
            }
        }
    }

    void CheckAllPlayersSelected()
    {
        for (int i = 0; i < playerManagerScript.playerCount; i++)
        {
            if (!playersSelected[i])
            {
                allSelected = false;
                return;
            }
        }
        if (playerManagerScript.playerCount > 0)
        {
            allSelected = true;
        }
    }

    void ScrollDown(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                GameObject oldP1Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P1 Txt")?.gameObject;
                oldP1Icon.SetActive(false);
                if (playerHovers[playerNo] + 1 < gameModeButtons.Count)
                {
                    playerHovers[playerNo]++;
                }
                GameObject newP1Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P1 Txt")?.gameObject;
                newP1Icon.SetActive(true);
                break;
            case 1:
                GameObject oldP2Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P2 Txt")?.gameObject;
                oldP2Icon.SetActive(false);
                if (playerHovers[playerNo] + 1 < gameModeButtons.Count)
                {
                    playerHovers[playerNo]++;
                }
                GameObject newP2Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P2 Txt")?.gameObject;
                newP2Icon.SetActive(true);
                break;
            case 2:
                GameObject oldP3Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P3 Txt")?.gameObject;
                oldP3Icon.SetActive(false);
                if (playerHovers[playerNo] + 1 < gameModeButtons.Count)
                {
                    playerHovers[playerNo]++;
                }
                GameObject newP3Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P3 Txt")?.gameObject;
                newP3Icon.SetActive(true);
                break;
            case 3:
                GameObject oldP4Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P4 Txt")?.gameObject;
                oldP4Icon.SetActive(false);
                if (playerHovers[playerNo] + 1 < gameModeButtons.Count)
                {
                    playerHovers[playerNo]++;
                }
                GameObject newP4Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P4 Txt")?.gameObject;
                newP4Icon.SetActive(true);
                break;
        }
    }

    void ScrollUp(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                GameObject oldP1Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P1 Txt")?.gameObject;
                oldP1Icon.SetActive(false);
                if (playerHovers[playerNo] - 1 >= 0)
                {
                    playerHovers[playerNo]--;
                }
                GameObject newP1Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P1 Txt")?.gameObject;
                newP1Icon.SetActive(true);
                break;
            case 1:
                GameObject oldP2Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P2 Txt")?.gameObject;
                oldP2Icon.SetActive(false);
                if (playerHovers[playerNo] - 1 >= 0)
                {
                    playerHovers[playerNo]--;
                }
                GameObject newP2Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P2 Txt")?.gameObject;
                newP2Icon.SetActive(true);
                break;
            case 2:
                GameObject oldP3Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P3 Txt")?.gameObject;
                oldP3Icon.SetActive(false);
                if (playerHovers[playerNo] - 1 >= 0)
                {
                    playerHovers[playerNo]--;
                }
                GameObject newP3Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P3 Txt")?.gameObject;
                newP3Icon.SetActive(true);
                break;
            case 3:
                GameObject oldP4Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P4 Txt")?.gameObject;
                oldP4Icon.SetActive(false);
                if (playerHovers[playerNo] - 1 >= 0)
                {
                    playerHovers[playerNo]--;
                }
                GameObject newP4Icon = gameModeButtons[playerHovers[playerNo]].transform.Find("P4 Txt")?.gameObject;
                newP4Icon.SetActive(true);
                break;
        }
    }
    
    void ToggleSelectGameMode(int playerNo)
    {
        selectedBtn = gameModeButtons[playerHovers[playerNo]];
        int buttonIndex = gameModeButtons.IndexOf(selectedBtn);

        switch (buttonIndex)
        {
            case 0:
                ToggleDeathMatch(playerNo);
                break;
            case 1:
                ToggleKingOfTheHill(playerNo);
                break;
            case 2:
                ToggleLifeSteal(playerNo);
                break;
            case 3:
                // Toggle another mode
                break;
            default:
                Debug.LogWarning("Invalid game mode selection");
                break;
        }
    }

    public void ToggleDeathMatch(int playerNo)
    {
        int textPlayerNo = playerNo + 1;
        if (votes[playerNo] == "-")
        {
            votes[playerNo] = gameModes[0];
            playersSelected[playerNo] = true;
            playerVotesText[playerNo].text = "Player " + textPlayerNo + " - " + votes[playerNo];
        }
        else if (votes[playerNo] == gameModes[0])
        {
            votes[playerNo] = "-";
            playersSelected[playerNo] = false;
            playerVotesText[playerNo].text = "Player " + textPlayerNo + " - ";
        }
    }

    public void ToggleKingOfTheHill(int playerNo)
    {
        int textPlayerNo = playerNo + 1;
        if (votes[playerNo] == "-")
        {
            votes[playerNo] = gameModes[1];
            playersSelected[playerNo] = true;
            playerVotesText[playerNo].text = "Player " + textPlayerNo + " - " + votes[playerNo];

        }
        else if (votes[playerNo] == gameModes[1])
        {
            votes[playerNo] = "-";
            playersSelected[playerNo] = false;
            playerVotesText[playerNo].text = "Player " + textPlayerNo + " - ";
        }
    }

    public void ToggleLifeSteal(int playerNo)
    {
        int textPlayerNo = playerNo + 1;
        if (votes[playerNo] == "-")
        {
            votes[playerNo] = gameModes[2];
            playersSelected[playerNo] = true;
            playerVotesText[playerNo].text = "Player " + textPlayerNo + " - " + votes[playerNo];

        }
        else if (votes[playerNo] == gameModes[2])
        {
            votes[playerNo] = "-";
            playersSelected[playerNo] = false;
            playerVotesText[playerNo].text = "Player " + textPlayerNo + " - ";
        }
    }

    public void Draw()
    {
        int randomDraw = Random.Range(0, votes.Count);

        while (validDraw == false)
        {
            if (votes[randomDraw] == "-")
            {
                randomDraw = Random.Range(0, votes.Count);
            }
            else
            {
                validDraw = true;
            }
        }
        chosenMode = votes[randomDraw];
        chosenModeTxt.text = "Loading " + chosenMode + "...";
        PlayerVotesObj.SetActive(false);
    }
}
