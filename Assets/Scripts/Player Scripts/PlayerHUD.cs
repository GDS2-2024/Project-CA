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
    public TMP_Text ObjectiveText;

    // Start is called before the first frame update
    void Start()
    {
        if (!ammoTextComponent) { Debug.Log("PlayerHUD: There is no ammo text component assigned."); }
        if (!UtilCooldownText) { Debug.Log("PlayerHUD: There is no utility cooldown text component assigned."); }
        if (!DamageCooldownText) { Debug.Log("PlayerHUD: There is no damage cooldown text component assigned."); }
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

    public void UpdateGameTimer(int newTime)
    {
        GameTimerText.text = "" + newTime;
    }

    public void UpdateObjective(string currentObjective)
    {
        ObjectiveText.text = "" + currentObjective;
    }

}
