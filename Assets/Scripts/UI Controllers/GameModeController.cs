using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameModeController : MonoBehaviour
{
    private PlayerManager playerManagerScript;
    private SceneManagement sceneManagement;
    private int[] playerHoverIndices = new int[4];
    private bool[] playersSelected = new bool[4];
    private bool validDraw = false;
    private List<float> holdTime = new List<float>(new float[4]);

    public List<GameObject> gameModeButtons;
    public List<string> votes;
    public List<TMP_Text> playerVotesText;
    public GameObject PlayerVotesObj;
    public TMP_Text chosenModeTxt, timerTxt;
    public float startGameTimer = 3f;
    public string chosenMode;
    public bool allSelected;
    private readonly string[] gameModes = { "Death Match", "King of the Hill", "Life Steal", "Payload" };
    private const float requiredHoldDuration = 1.0f;

    void Start()
    {
        playerManagerScript = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        sceneManagement = GameObject.Find("SceneManager").GetComponent<SceneManagement>();
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
        playersSelected = new bool[4];
        playerHoverIndices = new int[4];
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
            var device = playerManagerScript.inputDevices[i];
            HandleInput(device, i);
        }
    }

    void HandleInput(InputDevice device, int playerIndex)
    {
        if (device is Keyboard keyboard)
        {
            if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame) Scroll(playerIndex, 1);
            if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame) Scroll(playerIndex, -1);
            if (keyboard.enterKey.wasPressedThisFrame) ToggleSelectGameMode(playerIndex);
        }
        else if (device is Gamepad gamepad)
        {
            if (gamepad.leftStick.down.wasPressedThisFrame || gamepad.dpad.down.wasPressedThisFrame) Scroll(playerIndex, 1);
            if (gamepad.leftStick.up.wasPressedThisFrame || gamepad.dpad.up.wasPressedThisFrame) Scroll(playerIndex, -1);
            if (gamepad.buttonSouth.wasPressedThisFrame) ToggleSelectGameMode(playerIndex);
        }
    }

    void Scroll(int playerIndex, int direction)
    {
        var oldIcon = gameModeButtons[playerHoverIndices[playerIndex]].transform.Find($"P{playerIndex + 1} Txt")?.gameObject;
        oldIcon?.SetActive(false);
        playerHoverIndices[playerIndex] = Mathf.Clamp(playerHoverIndices[playerIndex] + direction, 0, gameModeButtons.Count - 1);
        var newIcon = gameModeButtons[playerHoverIndices[playerIndex]].transform.Find($"P{playerIndex + 1} Txt")?.gameObject;
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
        allSelected = playerManagerScript.playerCount > 0 && Array.TrueForAll(playersSelected, selected => selected);
    }

    public void SetupPlayerIcons()
    {
        if (playerManagerScript == null) return;

        // Disable all player indicators first
        foreach (var button in gameModeButtons)
        {
            for (int p = 1; p <= 4; p++)
            {
                var playerText = button.transform.Find($"P{p} Txt")?.gameObject;
                if (playerText != null)
                    playerText.SetActive(false);
            }
        }

        // Enable only the relevant player indicators
        for (int i = 0; i < playerManagerScript.playerCount; i++)
        {
            int hoverIndex = playerHoverIndices[i];
            GameObject selectedButton = gameModeButtons[hoverIndex];

            var playerText = selectedButton.transform.Find($"P{i + 1} Txt")?.gameObject;
            if (playerText != null)
                playerText.SetActive(true);
        }
    }

    void HandleGameStart()
    {
        if (!validDraw) return;

        startGameTimer -= Time.deltaTime;
        timerTxt.text = Mathf.Round(startGameTimer).ToString();

        if (startGameTimer > 0) return;

        if (chosenMode == "Death Match") sceneManagement.LoadDeathMatch();
        else if (chosenMode == "King of the Hill") sceneManagement.LoadKingOfTheHill();
    }

    public void Draw()
    {
        do { chosenMode = votes[UnityEngine.Random.Range(0, votes.Count)]; }
        while (chosenMode == "-");

        validDraw = true;
        chosenModeTxt.text = $"Loading {chosenMode}...";
        PlayerVotesObj.SetActive(false);
    }
}
