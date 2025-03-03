using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Controls;


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
    }

    // Update is called once per frame
    void Update()
    {
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
        if ((Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
        {
            LoadGamemodeMenuScreen();
        }
    }

    private void HandleGamemodeMenu()
    {
        // Handled in GameModeController.cs
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

        mainMenu.SetActive(false);
        characterMenu.SetActive(false);
        gamemodeMenu.SetActive(true);
    }

}
