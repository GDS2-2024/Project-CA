using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRumbleHandler : MonoBehaviour
{
    private Gamepad gamepadController;

    // Start is called before the first frame update
    void Start()
    {
        InputDevice playerController = GetComponent<PlayerController>().GetController();
        if (playerController is Gamepad gamepad) { gamepadController = gamepad; }
        else { Destroy(this); }
        // This class is removed if the player is using a keyboard
        // Check class is not null before using
    }

    public void StartRumble(float lowFrequency, float highFrequency, float duration)
    {
        StartCoroutine(HandleRumble(lowFrequency, highFrequency, duration));
    }

    private IEnumerator HandleRumble(float lowFrequency, float highFrequency, float duration)
    {
        gamepadController.SetMotorSpeeds(lowFrequency, highFrequency);
        yield return new WaitForSeconds(duration);
        gamepadController.SetMotorSpeeds(0, 0);
    }

    public void StopRumble()
    {
        gamepadController.SetMotorSpeeds(0f, 0f);
    }

}
