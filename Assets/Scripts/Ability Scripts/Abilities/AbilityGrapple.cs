using System.Collections;
using UnityEngine;

public class AbilityGrapple : Ability
{
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform grappleGunTip, cam, player;
    private float maxGrappleDistance = 75f;
    private float maxRaycast = 500f;

    private Rigidbody playerRb;
    private PlayerMoveBase playerMove;
    private bool isGrappling = false;
    private float grappleSpeedMultiplier = 50f;
    private float stopDistance = 1.2f;

    private float grappleStartTime;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerRb = GetComponentInParent<Rigidbody>();
        playerMove = GetComponentInParent<PlayerMoveBase>();
    }

    public override void OnPressAbility()
    {
        if (!isOnCooldown)
        {
            StartGrapple();
        }
    }

    public override void OnReleaseAbility()
    {
        if (!isOnCooldown)
        {
            StopGrapple();
            StartCooldown();
        }
    }

    void FixedUpdate()
    {
        if (isGrappling)
        {
            PullPlayerTowardsGrapple();
        }
    }

    void LateUpdate()
    {
        DrawGreenLine();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxRaycast, whatIsGrappleable))
        {
            lineRenderer.positionCount = 2;
            currentGrapplePosition = grappleGunTip.position;
            grapplePoint = hit.point;

            float distance = Vector3.Distance(transform.position, hit.point);
            if (distance < maxGrappleDistance)
            {
                grappleStartTime = Time.time;
                isGrappling = true;
                playerMove.DisableMovement();
            } else
            {
                StartCoroutine(DrawRedLine());
            }     
        }
        else
        {
            // Raycast did not hit, so simulate a far grapple point
            grapplePoint = cam.position + cam.forward * maxRaycast;
            StartCoroutine(DrawRedLine());
        }
    }

    void StopGrapple()
    {
        isGrappling = false;
        playerMove.EnableMovement();
        lineRenderer.positionCount = 0;
    }

    void PullPlayerTowardsGrapple()
    {
        if (playerRb == null) return;

        // Calculate the direction and distance
        Vector3 direction = (grapplePoint - player.position);
        float distance = Vector3.Distance(player.position, grapplePoint);

        // Stop grappling if close enough to the grapple point
        if (distance < stopDistance)
        {
            StopGrapple();
            StartCooldown();
            return;
        }

        // Exponentially increase speed over time
        float timeSinceGrappleStart = Time.time - grappleStartTime;
        float speed = Mathf.Pow(timeSinceGrappleStart, 2) * grappleSpeedMultiplier;

        // Apply force towards the grapple point
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(new Vector3(direction.x * speed, direction.y * speed, direction.z * speed), ForceMode.Acceleration);

    }

    private Vector3 currentGrapplePosition; 

    void DrawGreenLine()
    {
        if (!isGrappling) return;

        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 6f);
        lineRenderer.SetPosition(0, grappleGunTip.position);
        lineRenderer.SetPosition(1, currentGrapplePosition);
    }

    IEnumerator DrawRedLine()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        Vector3 startGrapplePosition = grappleGunTip.position;
        Vector3 endGrapplePosition = grapplePoint;
        float elapsedTime = 0f;
        float duration = 0.5f;

        // Lerp the red line over time
        while (elapsedTime < duration)
        {
            if (lineRenderer.positionCount == 0) { yield break; }

            elapsedTime += Time.deltaTime;
            Vector3 currentPosition = Vector3.Lerp(startGrapplePosition, endGrapplePosition, elapsedTime / duration);
            lineRenderer.SetPosition(0, grappleGunTip.position);
            lineRenderer.SetPosition(1, currentPosition);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        lineRenderer.positionCount = 0;
    }


    public override void OnHoldingAbility()
    {
        // This ability does not need this function
    }
}
