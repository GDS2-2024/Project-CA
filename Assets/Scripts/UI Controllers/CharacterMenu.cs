using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMenu : MonoBehaviour
{
    private PlayerManager playerManagerScript;

    public List<TMP_Text> playerJoinLabels;
    public List<GameObject> characterButtons;
    public List<TMP_Text> chosenCharacterTexts;
    public List<GameObject> characterPrefabs;
    private int[] playerHoverIndices = { 0, 0, 0, 0 };
    private bool[] firstInput = new bool[4];
    private const int gridWidth = 2;
    private const int gridHeight = 2;

    // Start is called before the first frame update
    void Start()
    {
        playerManagerScript = GameObject.Find("Player Manager").GetComponent<PlayerManager>();

    }

    // Update is called once per frame
    void Update()
    {
        ProcessPlayerInputs();
        CheckPlayerJoined();
    }

    void ProcessPlayerInputs()
    {
        for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
        {
            if (!firstInput[i]) { return; } // Don't move in grid until after first input used to join
            InputDevice device = playerManagerScript.inputDevices[i];
            Vector2 moveDir = Vector2.zero; // Represents direction to move in grid as a 2D vector

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
        int currentPos = playerHoverIndices[playerIndex];
        HidePlayerHoverVisual(playerIndex);

        if (direction.x > 0 && (currentPos % gridWidth) < gridWidth - 1)        // Move Right
            playerHoverIndices[playerIndex]++;
        else if (direction.x < 0 && (currentPos % gridWidth) > 0)               // Move Left
            playerHoverIndices[playerIndex]--;
        else if (direction.y > 0 && currentPos - gridWidth >= 0)                // Move Up
            playerHoverIndices[playerIndex] -= gridWidth;
        else if (direction.y < 0 && currentPos + gridWidth < characterPrefabs.Count)   // Move Down
            playerHoverIndices[playerIndex] += gridWidth;

        ShowPlayerHoverVisual(playerIndex);
    }

    void HidePlayerHoverVisual(int playerIndex)
    {
        GameObject oldIcon = characterButtons[playerHoverIndices[playerIndex]].transform.Find($"P{playerIndex + 1} Txt")?.gameObject;
        oldIcon?.SetActive(false);
    }

    void ShowPlayerHoverVisual(int playerIndex)
    {
        GameObject newIcon = characterButtons[playerHoverIndices[playerIndex]].transform.Find($"P{playerIndex + 1} Txt")?.gameObject;
        newIcon?.SetActive(true);
    }

    private void SelectCharacter(int playerIndex)
    {
        playerManagerScript.SetSelectedCharacter(characterPrefabs[playerHoverIndices[playerIndex]], playerIndex);
        chosenCharacterTexts[playerIndex].text = characterButtons[playerHoverIndices[playerIndex]]
            .transform.Find("Character Name")?.GetComponent<TMP_Text>()?.text ?? "Not Found";
        // Add the 3D model / image to player corner
    }

    private void CheckPlayerJoined()
    {       
        for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
        {
            playerJoinLabels[i].text = $"Player {i+1}";
            ShowPlayerHoverVisual(i);
            firstInput[i] = true;
        }
    }
    
    public bool CheckIfAllSelected()
    {
        for (int i = 0; i < playerManagerScript.playerCount; i++)
        {
            if (!playerManagerScript.characterSelections[i]) { return false; }
        }
        return true;
    }

}
