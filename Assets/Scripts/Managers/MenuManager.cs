using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    private GameObject currentMenu;

    // Menus
    public GameObject mainMenu;
    public GameObject characterMenu;
    public GameObject gamemodeMenu;

    // Hold Timers
    private const float holdDuration = 1.0f;
    private List<float> backHoldTime = new List<float>(new float[4]);
    private List<float> forwardHoldTime = new List<float>(new float[4]);

    // Hold Bar References
    public List<RectTransform> RedHoldBars;
    public List<RectTransform> GreenHoldBars;

    // Scripts
    private SceneManagement sceneManagement;
    private PlayerManager playerManager;
    private GameModeController gameModeController;

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
        sceneManagement = GameObject.Find("SceneManager").GetComponent<SceneManagement>();
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        gameModeController = GameObject.Find("Gamemode Controller").GetComponent<GameModeController>();
        gameModeController.enabled = false;
    }

    private void ResetPlayerManager()
    {
        playerManager.inputDevices.Clear();
        playerManager.p1Controller = null;
        playerManager.p2Controller = null;
        playerManager.p3Controller = null;
        playerManager.p4Controller = null;
        playerManager.playerCount = 0;
        playerManager.canPlayersJoin = false;
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
    }

    public void LoadCharacterSelectionScreen()
    {
        SwitchMenu(characterMenu);
        playerManager.canPlayersJoin = true;
        gameModeController.enabled = false;
    }

    public void LoadGamemodeMenuScreen()
    {
        SwitchMenu(gamemodeMenu);
        playerManager.canPlayersJoin = false;
        gameModeController.enabled = true;
        gameModeController.ResetGameModeMenu();
        gameModeController.SetupPlayerIcons();
    }

    private void SwitchMenu(GameObject newMenu)
    {
        currentMenu.SetActive(false);
        currentMenu = newMenu;
        currentMenu.SetActive(true);
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
        else if (currentMenu == gamemodeMenu && gameModeController.allSelected) gameModeController.Draw();
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
        if (IsBackPressed(device))
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
        else if (IsBackReleased(device))
        {
            backHoldTime[index] = 0;
            UpdateHoldBar(RedHoldBars, backHoldTime[index], 400, true);
        }

    }

    private bool IsBackPressed(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.escapeKey.isPressed) ||
               (device is Gamepad gamepad && gamepad.buttonEast.isPressed);
    }

    private bool IsBackReleased(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.escapeKey.wasReleasedThisFrame) ||
               (device is Gamepad gamepad && gamepad.buttonEast.wasReleasedThisFrame);
    }

    private void HandleReadyUp()
    {
        for (int i = 0; i < playerManager.inputDevices.Count; i++)
        {
            HandleReadyInput(playerManager.inputDevices[i], i);
        }
    }

    private void HandleReadyInput(InputDevice device, int index)
    {
        if (currentMenu == gamemodeMenu && !gameModeController.allSelected) return;

        if (IsReadyPressed(device))
        {
            forwardHoldTime[index] += Time.deltaTime;
            UpdateHoldBar(GreenHoldBars, forwardHoldTime[index], 350, false);
            if (forwardHoldTime[index] >= holdDuration)
            {
                forwardHoldTime[index] = 0;
                UpdateHoldBar(GreenHoldBars, forwardHoldTime[index], 350, false);
                GoNextMenu();
            }
        }
        else if (IsReadyReleased(device))
        {
            forwardHoldTime[index] = 0;
            UpdateHoldBar(GreenHoldBars, forwardHoldTime[index], 350, false);
        }       
    }

    private bool IsReadyPressed(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.qKey.isPressed) ||
               (device is Gamepad gamepad && gamepad.buttonNorth.isPressed);
    }

    private bool IsReadyReleased(InputDevice device)
    {
        return (device is Keyboard keyboard && keyboard.qKey.wasReleasedThisFrame) ||
               (device is Gamepad gamepad && gamepad.buttonNorth.wasReleasedThisFrame);
    }

    private void UpdateHoldBar(List<RectTransform> holdBars, float holdTime, float maxPosition, bool MoveRight)
    {
        float progress = holdTime / holdDuration;
        int menuIndex = currentMenu == mainMenu ? 0 : currentMenu == characterMenu ? 1 : currentMenu == gamemodeMenu ? 2 : -1;
        float xPosition = MoveRight ? -maxPosition + (progress * maxPosition) : maxPosition - (progress * maxPosition);
        holdBars[menuIndex].anchoredPosition = new Vector2(xPosition, 0);
    }

}
