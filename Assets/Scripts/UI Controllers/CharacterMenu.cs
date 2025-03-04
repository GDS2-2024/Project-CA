using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMenu : MonoBehaviour
{
    private PlayerManager playerManagerScript;
    
    public List<GameObject> characterButtons;
    public List<GameObject> characterPrefabs;
    private int[] playerHovers = { 0, 0, 0, 0 };
    private const int gridWidth = 2;
    private const int gridHeight = 2;
    private int totalCharacters = 4;

    // Start is called before the first frame update
    void Start()
    {
        playerManagerScript = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessPlayerInputs();
    }

    void ProcessPlayerInputs()
    {
        for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
        {
            InputDevice device = playerManagerScript.inputDevices[i];

            Vector2 moveDir = Vector2.zero;

            if (device is Keyboard keyboard)
            {
                if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame) moveDir.y = 1; // Up
                if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame) moveDir.y = -1; // Down
                if (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame) moveDir.x = -1; // Left
                if (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame) moveDir.x = 1; // Right
                if (keyboard.spaceKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame) SelectCharacter(i);
            }
            else if (device is Gamepad gamepad)
            {
                if (gamepad.leftStick.up.wasPressedThisFrame || gamepad.dpad.up.wasPressedThisFrame) moveDir.y = 1; // Up
                if (gamepad.leftStick.down.wasPressedThisFrame || gamepad.dpad.down.wasPressedThisFrame) moveDir.y = -1; // Down
                if (gamepad.leftStick.left.wasPressedThisFrame || gamepad.dpad.left.wasPressedThisFrame) moveDir.x = -1; // Left
                if (gamepad.leftStick.right.wasPressedThisFrame || gamepad.dpad.right.wasPressedThisFrame) moveDir.x = 1; // Right
                if (gamepad.buttonSouth.wasPressedThisFrame) SelectCharacter(i);
            }

            if (moveDir != Vector2.zero)
                MovePlayerCursor(i, moveDir);
        }
    }

    void MovePlayerCursor(int playerIndex, Vector2 direction)
    {
        int currentPos = playerHovers[playerIndex];

        if (direction.x > 0 && (currentPos % gridWidth) < gridWidth - 1)        // Move Right
            playerHovers[playerIndex]++;
        else if (direction.x < 0 && (currentPos % gridWidth) > 0)               // Move Left
            playerHovers[playerIndex]--;
        else if (direction.y > 0 && currentPos - gridWidth >= 0)                // Move Up
            playerHovers[playerIndex] -= gridWidth;
        else if (direction.y < 0 && currentPos + gridWidth < totalCharacters)   // Move Down
            playerHovers[playerIndex] += gridWidth;

        UpdatePlayerHoverVisual(playerIndex);
    }

    void UpdatePlayerHoverVisual(int playerIndex)
    {
        foreach (GameObject character in characterButtons)
        {
            for (int p = 1; p <= 4; p++)
            {
                GameObject oldPlayerText = character.transform.Find($"P{p} Txt")?.gameObject;
                if (oldPlayerText != null)
                    oldPlayerText.SetActive(false);
            }
        }

        int hoveredIndex = playerHovers[playerIndex];
        GameObject selectedCharacter = characterButtons[hoveredIndex];

        var newPlayerText = selectedCharacter.transform.Find($"P{playerIndex + 1} Txt")?.gameObject;
        if (newPlayerText != null)
            newPlayerText.SetActive(true);
    }

    private void SelectCharacter(int playerIndex)
    {
        playerManagerScript.SetSelectedCharacter(characterPrefabs[playerHovers[playerIndex]], playerIndex);
        // TO DO:
        // Set text in player corner to Prefab name
        // Add the 3D model / image to player corner
    }

}
