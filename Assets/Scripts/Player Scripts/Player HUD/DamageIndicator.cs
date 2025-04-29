using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Vector3 damageLocation;
    private Transform playerTransform;
    public Image damageImage;
    public Transform damageImagePivot;
    private float fadeStartTime = 1.5f, fadeTime = 1.5f, maxFadeTime;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = gameObject.transform.parent.parent.transform;
        maxFadeTime = fadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeStartTime > 0)
        {
            fadeStartTime -= Time.deltaTime;
        } else
        {
            fadeTime -= Time.deltaTime;
            damageImage.color = new Color(1, 1, 1, fadeTime / maxFadeTime);
            if (fadeTime < 0) { Destroy(this.gameObject); }
        }

        damageLocation.y = playerTransform.position.y;
        Vector3 direction = (damageLocation - playerTransform.position).normalized;
        float angle = (Vector3.SignedAngle(direction, playerTransform.forward, Vector3.up));
        damageImagePivot.localEulerAngles = new Vector3(0, 0, angle);
    }
}
