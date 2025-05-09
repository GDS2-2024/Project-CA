using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerManager : MonoBehaviour
{
    private int maxPlayerCount = 4;
    public bool canPlayersJoin = true;

    public int playerCount = 0;
    public InputDevice p1Controller = null;
    public InputDevice p2Controller = null;
    public InputDevice p3Controller = null;
    public InputDevice p4Controller = null;
    public List<InputDevice> inputDevices = new List<InputDevice>();
    public List<GameObject> characterSelections = new List<GameObject>(new GameObject[4]);

    public static PlayerManager instance;
    void Awake()
    {
        if (instance == null)
        {
            // If no instance exists, set this one
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlayersJoin) { return; }
        //Press any button on keyboard or controller to join assign a player number to your device
        if (playerCount <= maxPlayerCount)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame && !inputDevices.Contains(Keyboard.current))
            {
                AssignController(Keyboard.current, playerCount);
            }
            else
            {
                foreach (var gamepad in Gamepad.all)
                {
                    foreach (var control in Gamepad.current.allControls)
                    {
                        if (control is ButtonControl button && button.wasPressedThisFrame && !inputDevices.Contains(Gamepad.current))
                        {
                            AssignController(Gamepad.current, playerCount);
                        }
                    }
                }
            }
        }
    }

    public void AssignController(InputDevice current, int playerNo)
    {
        //assigns player number to your device
        switch (playerNo)
        {
            case 0:
                p1Controller = current;
                print("Player 1 Controller: " + current);
                break;
            case 1:
                p2Controller = current;
                print("Player 2 Controller: " + current);
                break;
            case 2:
                p3Controller = current;
                print("Player 3 Controller: " + current);
                break;
            case 3:
                p4Controller = current;
                print("Player 4 Controller: " + current);
                break;
        }

        inputDevices.Add(current);
        playerCount++;
    }

    public void SetSelectedCharacter(GameObject character, int playerIndex)
    {
        characterSelections[playerIndex] = character;
    }

    public GameObject GetSelectedCharacter(int playerIndex)
    {
        return characterSelections[playerIndex];
    }
}
