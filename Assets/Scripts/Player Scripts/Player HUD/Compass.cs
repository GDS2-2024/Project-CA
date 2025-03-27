using UnityEngine.UI;
using UnityEngine;
public class Compass : MonoBehaviour
{
    public Transform Player;
    public RectTransform CompassElement;
    public float compassSize;

    public void Update()
    {
        //CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);

        Vector3 forwardVector = Vector3.ProjectOnPlane(Player.forward, Vector3.up).normalized;
        float forwardSignedAngle = Vector3.SignedAngle(forwardVector, Vector3.forward, Vector3.up);
        float compassOffset = (forwardSignedAngle / 180f) * compassSize;
        CompassElement.anchoredPosition = new Vector3(compassOffset, 0);
    }
}