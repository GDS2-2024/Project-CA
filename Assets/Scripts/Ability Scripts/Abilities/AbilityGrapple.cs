using System.Collections;
using UnityEngine;

public class AbilityGrapple : Ability
{
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform grappleGunTip, cam, player;
    private float maxDistance = 100f;

    private Rigidbody playerRb;
    private PlayerMoveBase playerMove;
    private bool isGrappling = false;
    private float grappleSpeedMultiplier = 50f; // Controls how fast speed increases
    private float stopDistance = 1.2f; // Distance from the grapple point where the player stops

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
            StartCoroutine(Cooldown());
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
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            grappleStartTime = Time.time;
            isGrappling = true;

            lineRenderer.positionCount = 2;
            currentGrapplePosition = grappleGunTip.position;
        }
    }

    void StopGrapple()
    {
        isGrappling = false;
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
            StartCoroutine(Cooldown());
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

    void DrawRope()
    {
        if (!isGrappling) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        lineRenderer.SetPosition(0, grappleGunTip.position);
        lineRenderer.SetPosition(1, currentGrapplePosition);
    }

    public override void OnHoldingAbility()
    {
        // This ability does not need this function
    }
}
