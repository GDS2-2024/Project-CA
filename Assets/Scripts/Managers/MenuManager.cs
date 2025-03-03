using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Controls;
using System.Security.Cryptography;


public class MenuManager : MonoBehaviour
{
    private GameObject currentMenu;

    public GameObject mainMenu;
    public GameObject characterMenu;
    public GameObject gamemodeMenu;

    private GameObject sceneManager;
    private SceneManagement sceneManagement;
    private GameObject playerManager;
    private PlayerManager playerManagerScript;
    private GameObject gameModeController;
    private GameModeController gameModeControllerScript;

    const float holdDuration = 1.0f;
    private List<float> backHoldTime = new List<float>();
    private List<float> forwardHoldTime = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        currentMenu = mainMenu;
        mainMenu.SetActive(true);
        characterMenu.SetActive(false);
        gamemodeMenu.SetActive(false);

        sceneManager = GameObject.Find("SceneManager");
        sceneManagement = sceneManager.GetComponent<SceneManagement>();

        playerManager = GameObject.Find("Player Manager");
        playerManagerScript = playerManager.GetComponent<PlayerManager>();
        playerManagerScript.inputDevices.Clear();
        playerManagerScript.p1Controller = null;
        playerManagerScript.p2Controller = null;
        playerManagerScript.p3Controller = null;
        playerManagerScript.p4Controller = null;
        playerManagerScript.playerCount = 0;
        playerManagerScript.canPlayersJoin = false;

        gameModeController = GameObject.Find("Gamemode Controller");
        gameModeControllerScript = gameModeController.GetComponent<GameModeController>();
        gameModeControllerScript.enabled = false;

        // Initialise hold times
        for (int i = 0; i < 4; i++)
        {
            backHoldTime.Add(0f);
            forwardHoldTime.Add(0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleBackButton();
        if (currentMenu == mainMenu) { HandleMainMenu(); }
        else if (currentMenu == characterMenu) { HandleCharacterMenu(); }
        else if (currentMenu == gamemodeMenu) { HandleGamemodeMenu(); }
    }

    private void HandleMainMenu()
    {
        if ((Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
        {
            LoadCharacterSelectionScreen();           
        }

    }

    private void HandleCharacterMenu()
    {
        HandleReadyUp();
    }

    private void HandleGamemodeMenu()
    {
        
    }

    public void LoadMainMenuScreen()
    {
        // Need to reset joined players & other menus
        currentMenu = mainMenu;
        
        mainMenu.SetActive(true);
        characterMenu.SetActive(false);
        gamemodeMenu.SetActive(false);
    }

    public void LoadCharacterSelectionScreen()
    {
        currentMenu = characterMenu;
        playerManagerScript.canPlayersJoin = true;
        gameModeControllerScript.enabled = false;

        mainMenu.SetActive(false);
        characterMenu.SetActive(true);
        gamemodeMenu.SetActive(false);
    }

    public void LoadGamemodeMenuScreen()
    {
        currentMenu = gamemodeMenu;
        playerManagerScript.canPlayersJoin = false;
        gameModeControllerScript.enabled = true;
        gameModeControllerScript.ResetGameModeMenu();
        gameModeControllerScript.SetupPlayerIcons();

        mainMenu.SetActive(false);
        characterMenu.SetActive(false);
        gamemodeMenu.SetActive(true);
    }

    public void GoBackMenu()
    {
        if (currentMenu == gamemodeMenu) { LoadCharacterSelectionScreen(); }
        else if (currentMenu == characterMenu) { LoadMainMenuScreen(); }
        else if (currentMenu == mainMenu) { sceneManagement.QuitGame(); }
    }

    void HandleBackButton()
    {
        for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
        {
            var device = playerManagerScript.inputDevices[i];
            if (device is Keyboard keyboard) { HandleBackKeyboard(keyboard, i); }
            else if (device is Gamepad gamepad) { HandleBackGamepad(gamepad, i); }
        }
    }

    void HandleBackKeyboard(Keyboard keyboard, int playerIndex)
    {
        if (keyboard.escapeKey.isPressed)
        {
            backHoldTime[playerIndex] += Time.deltaTime;
            if (backHoldTime[playerIndex] >= holdDuration)
            {
                GoBackMenu();
                backHoldTime[playerIndex] = 0;
            }
        }
        else
        {
            backHoldTime[playerIndex] = 0;
        }
    }

    void HandleBackGamepad(Gamepad gamepad, int playerIndex)
    {       
        if (gamepad.buttonEast.isPressed)
        {
            backHoldTime[playerIndex] += Time.deltaTime;
            if (backHoldTime[playerIndex] >= holdDuration)
            {
                GoBackMenu();
                backHoldTime[playerIndex] = 0;
            }
        }
        else
        {
            backHoldTime[playerIndex] = 0;
        }
    }

    void HandleReadyUp()
    {
        for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
        {
            var device = playerManagerScript.inputDevices[i];
            if (device is Keyboard keyboard) { HandleReadyKeyboard(keyboard, i); }
            else if (device is Gamepad gamepad) { HandleReadyGamepad(gamepad, i); }
        }
    }

    void HandleReadyKeyboard(Keyboard keyboard, int playerIndex)
    {
        if (keyboard.qKey.isPressed)
        {
            forwardHoldTime[playerIndex] += Time.deltaTime;
            if (forwardHoldTime[playerIndex] >= holdDuration)
            {
                LoadGamemodeMenuScreen();
                forwardHoldTime[playerIndex] = 0;
            }
        }
        else
        {
            forwardHoldTime[playerIndex] = 0;
        }
    }

    void HandleReadyGamepad(Gamepad gamepad, int playerIndex)
    {
        if (gamepad.buttonNorth.isPressed)
        {
            forwardHoldTime[playerIndex] += Time.deltaTime;
            if (forwardHoldTime[playerIndex] >= holdDuration)
            {
                LoadGamemodeMenuScreen();
                forwardHoldTime[playerIndex] = 0;
            }
        }
        else
        {
            forwardHoldTime[playerIndex] = 0;
        }
    }

}
