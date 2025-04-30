using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameModeMenu : MonoBehaviour
{
    private PlayerManager playerManagerScript;
    private SceneManagement sceneManagement;
    private int[] playerHoverIndices = new int[4];
    [SerializeField] private bool[] playersSelected = new bool[4];
    private bool validDraw = false;

    public List<GameObject> gameModeButtons;
    public List<string> votes;
    public List<TMP_Text> playerVotesText;
    public GameObject PlayerVotesObj;
    public TMP_Text chosenModeTxt, timerTxt;
    public float startGameTimer = 3f;
    public string chosenMode;
    public bool allSelected;
    private readonly string[] gameModes = { "Death Match", "King of the Hill", "Life Steal", "Payload" };

    void Start()
    {
        playerManagerScript = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        sceneManagement = GameObject.Find("Scene Manager").GetComponent<SceneManagement>();
        ResetGameModeMenu();
    }

    void Update()
    {
        CheckAllPlayersSelected();
        ProcessPlayerInputs();
        HandleGameStart();
    }

    public void ResetGameModeMenu()
    {
        HidePlayerHovers();
        playersSelected = new bool[4];
        playerHoverIndices = new int[4];
        ShowPlayerHovers();
        allSelected = false;
        validDraw = false;
        startGameTimer = 3;
        timerTxt.text = "";
        chosenModeTxt.text = "A random gamemode will be chosen based on votes";
        PlayerVotesObj.SetActive(true);
        votes.ForEach(v => v = "-");
        for (int i = 0; i < playerVotesText.Count; i++)
            playerVotesText[i].text = $"Player {i + 1} - ";
    }

    void ProcessPlayerInputs()
    {
        for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
        {
            InputDevice device = playerManagerScript.inputDevices[i];
            HandleInput(device, i);
        }
    }

    void HandleInput(InputDevice device, int playerIndex)
    {
        if (IsGameStarting()) { return; }

        if (device is Keyboard keyboard)
        {
            if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame) Scroll(playerIndex, 1);
            if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame) Scroll(playerIndex, -1);
            if (keyboard.spaceKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame) ToggleSelectGameMode(playerIndex);
        }
        else if (device is Gamepad gamepad)
        {
            if (gamepad.leftStick.down.wasPressedThisFrame || gamepad.dpad.down.wasPressedThisFrame) Scroll(playerIndex, 1);
            if (gamepad.leftStick.up.wasPressedThisFrame || gamepad.dpad.up.wasPressedThisFrame) Scroll(playerIndex, -1);
            if (gamepad.buttonSouth.wasPressedThisFrame) ToggleSelectGameMode(playerIndex);
        }
    }

    void ShowPlayerHovers()
    {
        if (!playerManagerScript) { return; }
        for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
        {
            GameObject newIcon = gameModeButtons[playerHoverIndices[i]].transform.Find($"P{i + 1} Txt")?.gameObject;
            newIcon?.SetActive(true);
        }
    }

    void Scroll(int playerIndex, int direction)
    {
        GameObject oldIcon = gameModeButtons[playerHoverIndices[playerIndex]].transform.Find($"P{playerIndex + 1} Txt")?.gameObject;
        oldIcon?.SetActive(false);
        playerHoverIndices[playerIndex] = Mathf.Clamp(playerHoverIndices[playerIndex] + direction, 0, gameModeButtons.Count - 1);
        GameObject newIcon = gameModeButtons[playerHoverIndices[playerIndex]].transform.Find($"P{playerIndex + 1} Txt")?.gameObject;
        newIcon?.SetActive(true);
    }

    void ToggleSelectGameMode(int playerIndex)
    {
        int buttonIndex = playerHoverIndices[playerIndex];
        if (buttonIndex >= gameModes.Length) return;

        votes[playerIndex] = votes[playerIndex] == gameModes[buttonIndex] ? "-" : gameModes[buttonIndex];
        playersSelected[playerIndex] = votes[playerIndex] != "-";
        if (playersSelected[playerIndex]) { playerVotesText[playerIndex].text = $"Player {playerIndex + 1} - {votes[playerIndex]}"; }
        else { playerVotesText[playerIndex].text = $"Player {playerIndex + 1} - "; }
    }

    void CheckAllPlayersSelected()
    {
        allSelected = playerManagerScript.playerCount > 0 && playersSelected.Take(playerManagerScript.playerCount).All(selected => selected);
    }

    public void HidePlayerHovers()
    {
        if (playerManagerScript == null) return;

        foreach (GameObject button in gameModeButtons)
        {
            for (int p = 1; p <= 4; p++)
            {
                GameObject playerText = button.transform.Find($"P{p} Txt")?.gameObject;
                if (playerText != null)
                    playerText.SetActive(false);
            }
        }
    }
    public bool IsGameStarting() { return validDraw; }

    public void Draw()
    {
        do { chosenMode = votes[UnityEngine.Random.Range(0, votes.Count)]; }
        while (chosenMode == "-");

        validDraw = true;
        chosenModeTxt.text = $"Loading {chosenMode}...";
        PlayerVotesObj.SetActive(false);
    }

    void HandleGameStart()
    {
        if (!validDraw) return;

        startGameTimer -= Time.deltaTime;
        timerTxt.text = Mathf.Round(startGameTimer).ToString();

        if (startGameTimer > 0) return;

        StartCoroutine(ShowGamemodeRules());
    }

    [SerializeField] private GameObject rulesKOTH;
    IEnumerator ShowGamemodeRules()
    {
        if (chosenMode == "King of the Hill") rulesKOTH.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        LoadGameScene();
    }

    void LoadGameScene()
    {
        if (chosenMode == "Death Match") sceneManagement.LoadDeathMatch();
        else if (chosenMode == "King of the Hill") sceneManagement.LoadKingOfTheHill();
    }
}
