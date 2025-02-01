using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameModeController : MonoBehaviour
{
    private PlayerManager playerManagerScript;
    private int p1btn = 1;
    private int p2btn = 1;
    private int p3btn = 1;
    private int p4btn = 1;

    public List<Button> buttons = new List<Button>();
    public GameObject playerManager;


    // Start is called before the first frame update
    void Start()
    {
        playerManagerScript = playerManager.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManagerScript.inputDevices.Count > 0)
        {
            for (int i = 0; i < playerManagerScript.inputDevices.Count; i++)
            {
                if (playerManagerScript.inputDevices[i] is Keyboard keyboard)
                {
                    if (keyboard.sKey.wasPressedThisFrame)
                    {
                        ScrollDown(i);
                    }
                    else if (keyboard.wKey.wasPressedThisFrame)
                    {
                        ScrollUp(i);
                    }
                }
                else if (playerManagerScript.inputDevices[i] is Gamepad gamepad)
                {
                    if (gamepad.leftStick.down.wasPressedThisFrame)
                    {
                        ScrollDown(i);
                    }
                    else if (gamepad.leftStick.up.wasPressedThisFrame)
                    {
                        ScrollUp(i);
                    }
                }
            }
        }
    }

    void ScrollDown(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                GameObject oldP1Icon = buttons[p1btn].transform.Find("P1 Txt")?.gameObject;
                oldP1Icon.SetActive(false);
                p1btn++;
                GameObject newP1Icon = buttons[p1btn].transform.Find("P1 Txt")?.gameObject;
                newP1Icon.SetActive(true);
                break;
            case 1:
                GameObject oldP2Icon = buttons[p2btn].transform.Find("P2 Txt")?.gameObject;
                oldP2Icon.SetActive(false);
                p2btn++;
                GameObject newP2Icon = buttons[p2btn].transform.Find("P2 Txt")?.gameObject;
                newP2Icon.SetActive(true);
                break;
            case 2:
                GameObject oldP3Icon = buttons[p3btn].transform.Find("P3 Txt")?.gameObject;
                oldP3Icon.SetActive(false);
                p3btn++;
                GameObject newP3Icon = buttons[p3btn].transform.Find("P3 Txt")?.gameObject;
                newP3Icon.SetActive(true);
                break;
            case 3:
                GameObject oldP4Icon = buttons[p4btn].transform.Find("P4 Txt")?.gameObject;
                oldP4Icon.SetActive(false);
                p4btn++;
                GameObject newP4Icon = buttons[p4btn].transform.Find("P4 Txt")?.gameObject;
                newP4Icon.SetActive(true);
                break;
        }
    }

    void ScrollUp(int playerNo)
    {
        switch (playerNo)
        {
            case 0:
                GameObject oldP1Icon = buttons[p1btn].transform.Find("P1 Txt")?.gameObject;
                oldP1Icon.SetActive(false);
                p1btn--;
                GameObject newP1Icon = buttons[p1btn].transform.Find("P1 Txt")?.gameObject;
                newP1Icon.SetActive(true);
                break;
            case 1:
                GameObject oldP2Icon = buttons[p2btn].transform.Find("P2 Txt")?.gameObject;
                oldP2Icon.SetActive(false);
                p2btn--;
                GameObject newP2Icon = buttons[p2btn].transform.Find("P2 Txt")?.gameObject;
                newP2Icon.SetActive(true);
                break;
            case 2:
                GameObject oldP3Icon = buttons[p3btn].transform.Find("P3 Txt")?.gameObject;
                oldP3Icon.SetActive(false);
                p3btn--;
                GameObject newP3Icon = buttons[p3btn].transform.Find("P3 Txt")?.gameObject;
                newP3Icon.SetActive(true);
                break;
            case 3:
                GameObject oldP4Icon = buttons[p4btn].transform.Find("P4 Txt")?.gameObject;
                oldP4Icon.SetActive(false);
                p4btn--;
                GameObject newP4Icon = buttons[p4btn].transform.Find("P4 Txt")?.gameObject;
                newP4Icon.SetActive(true);
                break;
        }
    }
}
