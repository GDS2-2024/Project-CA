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
        if (!ammoTextComponent) { Debug.LogWarning("PlayerHUD: There is no Ammo text component assigned."); }
        if (!healthBarTransform) { Debug.LogWarning("PlayerHUD: There is no Health Bar Transform component assigned."); }
        if (!healthBarImage) { Debug.LogWarning("PlayerHUD: There is no Health Bar Image component assigned."); }
        if (!UtilCoolDownImage) { Debug.LogWarning("PlayerHUD: There is no Utility Cooldown Image component assigned."); }
        if (!DamageCoolDownImage) { Debug.LogWarning("PlayerHUD: There is no Damage Cooldown Image component assigned."); }
        if (!GameTimerText) { Debug.LogWarning("PlayerHUD: There is no Game Timer text component assigned."); }
        if (!RespawnTimer) { Debug.LogWarning("PlayerHUD: There is no Respawn Timer text component assigned."); }
        if (!ObjectiveScore) { Debug.LogWarning("PlayerHUD: There is no Objective Score text component assigned."); }
        if (!ObjectivePrompt) { Debug.LogWarning("PlayerHUD: There is no Objective Prompt text component assigned."); }
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
    public Image UtilCoolDownImage;
    public Image DamageCoolDownImage;
    public void UpdateUtilityCooldown(float currentPercentage)
    {
        UtilCoolDownImage.fillAmount = currentPercentage;
    }
    public void UpdateDamageCooldown(float currentPercentage)
    {
        DamageCoolDownImage.fillAmount = currentPercentage;
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

    //Reticle
    public Image Reticle;
    public void SetReticleSize(float reticleSize)
    {
        Reticle.rectTransform.sizeDelta = new Vector2(reticleSize, reticleSize);
    }

}
