using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!ammoTextComponent) { Debug.Log("PlayerHUD: There is no Ammo text component assigned."); }
        if (!healthBarTransform) { Debug.Log("PlayerHUD: There is no Health Bar Transform component assigned."); }
        if (!healthBarImage) { Debug.Log("PlayerHUD: There is no Health Bar Image component assigned."); }
        if (!UtilCooldownText) { Debug.Log("PlayerHUD: There is no Utility Cooldown text component assigned."); }
        if (!DamageCooldownText) { Debug.Log("PlayerHUD: There is no Damage Cooldown text component assigned."); }
        if (!GameTimerText) { Debug.Log("PlayerHUD: There is no Game Timer text component assigned."); }
        if (!RespawnTimer) { Debug.Log("PlayerHUD: There is no Respawn Timer text component assigned."); }
        if (!ObjectiveScore) { Debug.Log("PlayerHUD: There is no Objective Score text component assigned."); }
        if (!ObjectivePrompt) { Debug.Log("PlayerHUD: There is no Objective Prompt text component assigned."); }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Ammo
    public TMP_Text ammoTextComponent;
    public void UpateAmmoUI(int newAmount)
    {
        ammoTextComponent.text = "" + newAmount;
    }

    // Health
    public RectTransform healthBarTransform;
    public RawImage healthBarImage;
    private Color red = new Color(255f / 255f, 0f / 255f, 0f / 255f);
    private Color green = new Color(0f / 255f, 255f / 255f, 0f / 255f);
    public void UpdateHealthBar(float newHealth)
    {
        healthBarTransform.sizeDelta = new Vector2((newHealth / 100f) * 400, 50);
        Color newColor = Color.Lerp(red, green, newHealth / 100f);
        healthBarImage.color = newColor;
    }

    // Utilities
    public TMP_Text UtilCooldownText;
    public TMP_Text DamageCooldownText;
    public void UpdateUtilityCooldown(int newAmount)
    {
        UtilCooldownText.text = "" + newAmount;
    }
    public void UpdateDamageCooldown(int newAmount)
    {
        DamageCooldownText.text = "" + newAmount;
    }

    // Timers
    public TMP_Text GameTimerText;
    public TMP_Text RespawnTimer;
    public void UpdateGameTimer(string newTime)
    {
        GameTimerText.text = newTime;
    }
    public void StartRespawnTimer()
    {
        StartCoroutine(RespawnCountdownRoutine(3));
    }

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
    public TMP_Text ObjectiveScore;
    public TMP_Text ObjectivePrompt;
    public void UpdateObjectiveScore(int currentScore)
    {
        ObjectiveScore.text = "Score: " + currentScore;
    }
    public void UpdateObjectivePrompt(string currentObjective)
    {
        ObjectivePrompt.text = "" + currentObjective;
    }
    // Add a duration for how long to show the prompt
    public void UpdateObjectivePrompt(string currentObjective, float duration)
    {
        ObjectivePrompt.text = "" + currentObjective;
        Invoke("ClearObjectivePrompt", duration);
    }
    public void ClearObjectivePrompt()
    {
        ObjectivePrompt.text = "";
    }

}
