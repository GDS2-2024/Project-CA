using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AbilityXray : Ability
{
    public float duration;
    public Camera playerCamera;
    UniversalAdditionalCameraData cameraData;

    public override void OnPressAbility()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(SwitchRenderer());
        }
    }

    private IEnumerator SwitchRenderer()
    {
        cameraData.SetRenderer(1);
        yield return new WaitForSeconds(duration);
        cameraData.SetRenderer(0);
        StartCooldown();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerCamera) cameraData = playerCamera.GetComponent<UniversalAdditionalCameraData>();
        else { Debug.LogWarning("Missing player camera, please assign in XRay Ability"); }
    }

    public override void OnReleaseAbility()
    {
        // not needed
    }
    public override void OnHoldingAbility()
    {
        // Not needed
    }

}
