using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private GameObject currentMenu;

    // Menus
    public GameObject mainMenu;
    public GameObject characterMenu;
    public GameObject gamemodeMenu;

    // Hold Timers
    private const float holdDuration = 0.8f;
    private List<float> backHoldTime = new List<float>(new float[4]);
    private List<float> forwardHoldTime = new List<float>(new float[4]);
    private List<bool> startedHoldingBack = new List<bool>(new bool[4]);

    // Hold Bar References
    public List<Image> RedHoldBars;
    public List<Image> GreenHoldBars;

    // Scripts
    private SceneManagement sceneManagement;
    private PlayerManager playerManager;
    private GameModeMenu gameModeMenuManager;
    private CharacterMenu characterMenuManager;

    void Start()
    {
        InitializeMenus();
        InitializeManagers();
        ResetPlayerManager();
    }

    void Update()
    {
        HandleBackButton();
        HandleCurrentMenu();
    }

    private void InitializeMenus()
    {
        currentMenu = mainMenu;
        mainMenu.SetActive(true);
        characterMenu.SetActive(false);
        gamemodeMenu.SetActive(false);
    }

    private void InitializeManagers()
    {
        sceneManagement = GameObject.Find("Scene Manager").GetComponent<SceneManagement>();
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        gameModeMenuManager = GameObject.Find("Gamemode Menu").GetComponent<GameModeMenu>();
        gameModeMenuManager.enabled = false;
        characterMenuManager = GameObject.Find("Character Menu").GetComponent<CharacterMenu>();
        characterMenuManager.enabled = false;
        
    }

    private void ResetPlayerManager()
    {
        playerManager.inputDevices.Clear();
        playerManager.p1Controller = null;
        playerManager.p2Controller = null;
        playerManager.p3Controller = null;
        playerManager.p4Controller = null;
        playerManager.playerCount = 0;
        playerManager.canPlayersJoin = true;
    }

    private void HandleCurrentMenu()
    {
        if (currentMenu == mainMenu) { HandleMainMenu(); }
        else { HandleReadyUp(); }
    }

    private void HandleMainMenu()
    {
        if (IsStartPressed()) { LoadCharacterSelectionScreen(); }
    }

    private bool IsStartPressed()
    {
        return (Keyboard.current?.enterKey.wasPressedThisFrame ?? false) || (Gamepad.current?.aButton.wasPressedThisFrame ?? false);
    }

    public void LoadMainMenuScreen()
    {
        SwitchMenu(mainMenu);
        playerManager.canPlayersJoin = true;
        characterMenuManager.enabled = false;
        gameModeMenuManager.enabled = false;
    }

    public void LoadCharacterSelectionScreen()
    {
        SwitchMenu(characterMenu);
        playerManager.canPlayersJoin = true;
        characterMenuManager.enabled = true;
        gameModeMenuManager.enabled = false;
    }

    public void LoadGamemodeMenuScreen()
    {
        SwitchMenu(gamemodeMenu);
        playerManager.canPlayersJoin = false;
        characterMenuManager.enabled = false;
        gameModeMenuManager.enabled = true;
        gameModeMenuManager.ResetGameModeMenu();
    }

    private void SwitchMenu(GameObject newMenu)
    {
        currentMenu.SetActive(false);
        currentMenu = newMenu;
        currentMenu.SetActive(true);

        // Reset any hold bars
        for (int i = 0; i < startedHoldingBack.Count; i++)
        {
            startedHoldingBack[i] = false;
        }
    }

    public void GoBackMenu()
    {
        if (currentMenu == gamemodeMenu) LoadCharacterSelectionScreen();
        else if (currentMenu == characterMenu) LoadMainMenuScreen();
        else sceneManagement.QuitGame();
    }

    public void GoNextMenu()
    {
        if (currentMenu == characterMenu) LoadGamemodeMenuScreen();
        else if (currentMenu == gamemodeMenu && gameModeMenuManager.allSelected) gameModeMenuManager.Draw();
    }

    private void HandleBackButton()
    {
        for (int i = 0; i < playerManager.inputDevices.Count; i++)
        {
            HandleBackInput(playerManager.inputDevices[i], i);
        }
    }

    private void HandleBackInput(InputDevice device, int index)
    {
        if (StartedPressingBack(device)) { startedHoldingBack[index] = true; }
        if (IsPressingBack(device) && startedHoldingBack[index] == true)
        {
            backHoldTime[index] += Time.deltaTime;
            UpdateHoldBar(RedHoldBars, backHoldTime[index], 400, true);
            if (backHoldTime[index] >= holdDuration)
            {
                backHoldTime[index] = 0;
                UpdateHoldBar(RedHoldBars, backHoldTime[index], 400, true);
                GoBackMenu();
            }
        }
        else if (ReleasedBack(device))
        {
            backHoldTime[index] = 0;
            startedHoldingBack[index] = false;
            UpdateHoldBar(RedHoldBars, backHoldTime[index], 400, true);
        }

    }

    private bool StartedPressingBack(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.escapeKey.wasPressedThisFrame) ||
               (device is Gamepad gamepad && gamepad.buttonEast.wasPressedThisFrame);
    }

    private bool IsPressingBack(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.escapeKey.isPressed) ||
               (device is Gamepad gamepad && gamepad.buttonEast.isPressed);
    }

    private bool ReleasedBack(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.escapeKey.wasReleasedThisFrame) ||
               (device is Gamepad gamepad && gamepad.buttonEast.wasReleasedThisFrame);
    }

    private void HandleReadyUp()
    {
        if (currentMenu == characterMenu && !characterMenuManager.CheckIfAllSelected()) { return; }
        if (currentMenu == gamemodeMenu && gameModeMenuManager.IsGameStarting()) { return; }
        
        for (int i = 0; i < playerManager.inputDevices.Count; i++)
        {
            HandleReadyInput(playerManager.inputDevices[i], i);
        }
    }

    private void HandleReadyInput(InputDevice device, int index)
    {
        if (currentMenu == gamemodeMenu && !gameModeMenuManager.allSelected) return;

        if (IsPressingReady(device))
        {
            forwardHoldTime[index] += Time.deltaTime;
            UpdateHoldBar(GreenHoldBars, forwardHoldTime[index], 400, false);
            if (forwardHoldTime[index] >= holdDuration)
            {
                forwardHoldTime[index] = 0;
                UpdateHoldBar(GreenHoldBars, forwardHoldTime[index], 400, false);
                GoNextMenu();
            }
        }
        else if (ReleasedReady(device))
        {
            forwardHoldTime[index] = 0;
            UpdateHoldBar(GreenHoldBars, forwardHoldTime[index], 400, false);
        }       
    }

    private bool StartedPressingReady(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.qKey.wasPressedThisFrame) ||
               (device is Gamepad gamepad && gamepad.buttonNorth.wasPressedThisFrame);
    }

    private bool IsPressingReady(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.qKey.isPressed) ||
               (device is Gamepad gamepad && gamepad.buttonNorth.isPressed);
    }

    private bool ReleasedReady(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.qKey.wasReleasedThisFrame) ||
               (device is Gamepad gamepad && gamepad.buttonNorth.wasReleasedThisFrame);
    }

    private void UpdateHoldBar(List<Image> holdBars, float holdTime, float maxPosition, bool MoveRight)
    {
        float progress = holdTime / holdDuration;
        int menuIndex = currentMenu == mainMenu ? 0 : currentMenu == characterMenu ? 1 : currentMenu == gamemodeMenu ? 2 : -1;
        holdBars[menuIndex].fillAmount = progress;
    }

}
