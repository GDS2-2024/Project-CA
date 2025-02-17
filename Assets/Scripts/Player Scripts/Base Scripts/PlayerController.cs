using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputDevice controller = null;

    public void SetController(InputDevice newController)
    {
        controller = newController;
    }

    public InputDevice GetController()
    {
        return controller;
    }
}
