using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetupInputIcons();
        if (!AmmoTextComponent) { Debug.LogWarning("PlayerHUD: There is no Ammo text component assigned."); }
        if (!AmmoReloadImage) { Debug.LogWarning("PlayerHUD: There is no Ammo Reload Image component assigned."); }
        if (!HealthBarImage) { Debug.LogWarning("PlayerHUD: There is no Health Bar Image component assigned."); }
        if (!HealthBarImage) { Debug.LogWarning("PlayerHUD: There is no Health Bar Image component assigned."); }
        if (!UtilCoolDownImage) { Debug.LogWarning("PlayerHUD: There is no Utility Cooldown Image component assigned."); }
        if (!DamageCoolDownImage) { Debug.LogWarning("PlayerHUD: There is no Damage Cooldown Image component assigned."); }
        if (!UltimateCooldownImage) { Debug.LogWarning("PlayerHUD: There is no Ultimate Cooldown Image component assigned."); }
        if (!GameTimerText) { Debug.LogWarning("PlayerHUD: There is no Game Timer text component assigned."); }
        if (!RespawnTimer) { Debug.LogWarning("PlayerHUD: There is no Respawn Timer text component assigned."); }
        if (!ObjectiveScore) { Debug.LogWarning("PlayerHUD: There is no Objective Score text component assigned."); }
        if (!ObjectivePrompt) { Debug.LogWarning("PlayerHUD: There is no Objective Prompt text component assigned."); }
    }

    // Ammo
    [Header("Ammo")]
    public TMP_Text AmmoTextComponent;
    public Image AmmoReloadImage;
    public void UpateAmmoUI(int newAmount) { AmmoTextComponent.text = "" + newAmount; }
    public void UpdateReloadCooldown(float currentPercentage) { AmmoReloadImage.fillAmount = currentPercentage; }

    // Health
    [Header("Health")]
    public Image HealthBarImage;
    private Color red = new Color(255f / 255f, 0f / 255f, 0f / 255f);
    private Color green = new Color(0f / 255f, 255f / 255f, 0f / 255f);
    public void UpdateHealthBar(float newHealth)
    {
        HealthBarImage.fillAmount = newHealth / 100f;
        Color newColor = Color.Lerp(red, green, newHealth / 100f);
        HealthBarImage.color = newColor;
    }

    // Utilities
    [Header("Ability Icons")]
    public Image UtilCoolDownImage;
    public Image DamageCoolDownImage;
    public Image UltimateCooldownImage;
    public void RightAbilityCooldown(float currentPercentage) { UtilCoolDownImage.fillAmount = currentPercentage; }
    public void LeftAbilityCooldown(float currentPercentage) { DamageCoolDownImage.fillAmount = currentPercentage; }
    public void UltimateAbilityCooldown(float currentPercentage) { UltimateCooldownImage.fillAmount = currentPercentage; }

    // Timers
    [Header("Timers")]
    public TMP_Text GameTimerText;
    public TMP_Text RespawnTimer;
    public void UpdateGameTimer(string newTime) { GameTimerText.text = newTime; }
    public void StartRespawnTimer() { StartCoroutine(RespawnCountdownRoutine(3)); }

    private IEnumerator RespawnCountdownRoutine(int countdownTime)
    {
        while (countdownTime > 0)
        {
            RespawnTimer.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        RespawnTimer.text = "";
    }

    // Objectives
    [Header("Objective")]
    public TMP_Text ObjectiveScore;
    public TMP_Text ObjectivePrompt;
    public void UpdateObjectiveScore(int currentScore) { ObjectiveScore.text = "Score: " + currentScore; }
    public void UpdateObjectivePrompt(string currentObjective) { ObjectivePrompt.text = "" + currentObjective; }
    public void UpdateObjectivePrompt(string currentObjective, float duration)
    {
        ObjectivePrompt.text = "" + currentObjective;
        Invoke("ClearObjectivePrompt", duration);
    }
    public void ClearObjectivePrompt() { ObjectivePrompt.text = ""; }

    // Reticle
    [Header("Reticle")]
    public Image Reticle;
    public void SetReticleSize(float reticleSize) { Reticle.rectTransform.sizeDelta = new Vector2(reticleSize, reticleSize); }

    // Compass
    [Header("Compass")]
    public Camera PlayerCamera;
    public Transform CameraTransform;
    public RectTransform CompassBarRectTransform;
    public RectTransform ObjectiveRectTransform;

    public void SetCompassObjective(Vector3 objectiveWorldPosition)
    {
        Vector3 directionToTarget = objectiveWorldPosition - CameraTransform.position;
        float angle = Vector2.SignedAngle(
            new Vector2(directionToTarget.x, directionToTarget.z),
            new Vector2(CameraTransform.transform.forward.x, CameraTransform.transform.forward.z));
        float compassPositionX = Mathf.Clamp(angle / 80, -1, 1);
        ObjectiveRectTransform.anchoredPosition = 
            new Vector2(450 * compassPositionX, ObjectiveRectTransform.anchoredPosition.y);
    }

    public void SetActiveCompassObjective(bool enabled) { ObjectiveRectTransform.gameObject.SetActive(enabled); }

    // Changes the sprites for the HUD based on which input type this player is using
    [Header("Input based Icons")]
    public List<Sprite> keyboardIcons;
    public List<Sprite> xboxGamepadIcons;
    public List<Sprite> psGamepadIcons;

    [Header("Button Icons")]
    public Image ReloadButtonIcon;
    public Image UltimateButtonIcon;
    public Image DamageButtonIcon;
    public Image UtilityButtonIcon;

    private void SetupInputIcons()
    {
        InputDevice playerController =  gameObject.GetComponent<PlayerController>().GetController();
        if (playerController is Keyboard keyboard)
        {
            ReloadButtonIcon.sprite = keyboardIcons[0];
            UltimateButtonIcon.sprite = keyboardIcons[1];
            DamageButtonIcon.sprite = keyboardIcons[2];
            UtilityButtonIcon.sprite = keyboardIcons[3];
        }
        else if (playerController is UnityEngine.InputSystem.XInput.XInputController)
        {
            ReloadButtonIcon.sprite = xboxGamepadIcons[0];
            UltimateButtonIcon.sprite = xboxGamepadIcons[1];
            DamageButtonIcon.sprite = xboxGamepadIcons[2];
            UtilityButtonIcon.sprite = xboxGamepadIcons[3];
        }
        else if (playerController is UnityEngine.InputSystem.DualShock.DualShockGamepad)
        {
            ReloadButtonIcon.sprite = psGamepadIcons[0];
            UltimateButtonIcon.sprite = psGamepadIcons[1];
            DamageButtonIcon.sprite = psGamepadIcons[2];
            UtilityButtonIcon.sprite = psGamepadIcons[3];
        }
    }
}
