using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public TMP_Text ammoTextComponent;
    public TMP_Text UtilCooldownText;
    public TMP_Text DamageCooldownText;
    public TMP_Text GameTimerText;
    public TMP_Text ObjectiveScore;
    public TMP_Text ObjectivePrompt;

    // Start is called before the first frame update
    void Start()
    {
        if (!ammoTextComponent) { Debug.Log("PlayerHUD: There is no Ammo text component assigned."); }
        if (!UtilCooldownText) { Debug.Log("PlayerHUD: There is no Utility Cooldown text component assigned."); }
        if (!DamageCooldownText) { Debug.Log("PlayerHUD: There is no Damage Cooldown text component assigned."); }
        if (!GameTimerText) { Debug.Log("PlayerHUD: There is no Game Timer text component assigned."); }
        if (!ObjectiveScore) { Debug.Log("PlayerHUD: There is no Objective Score text component assigned."); }
        if (!ObjectivePrompt) { Debug.Log("PlayerHUD: There is no Objective Prompt text component assigned."); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpateAmmoUI(int newAmount)
    {
        ammoTextComponent.text = "" + newAmount;
    }

    public void UpdateUtilityCooldown(int newAmount)
    {
        UtilCooldownText.text = "" + newAmount;
    }

    public void UpdateDamageCooldown(int newAmount)
    {
        DamageCooldownText.text = "" + newAmount;
    }

    public void UpdateGameTimer(string newTime)
    {
        GameTimerText.text = newTime;
    }

    public void UpdateObjectiveScore(int currentScore)
    {
        ObjectiveScore.text = "Score: " + currentScore;
    }

    public void UpdateObjectivePrompt(string currentObjective)
    {
        ObjectivePrompt.text = "" + currentObjective;
    }

}
