using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class TestProjectile : MonoBehaviour
{
    private Rigidbody rb;

    bool drawDebug = true;

    public float bulletSpeed,
        damage,
        gravity = 9.8f;
    private GameObject shooter; // Which player shot this bullet

    private Vector3 bullet_velocity = new Vector3(0, 0, 0); //the vector that contains bullet's current speed
    private Vector3 last_position = new Vector3(0, 0, 0),
        current_position = new Vector3(0, 0, 0);

    [SerializeField]
    private ParticleSystem hitEffect;

    public float lifetime = 3.0f;

    public float ricochetAngle = 20.0f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        current_position = transform.position;

        // Set a timeout so if we don't hit anything the bullet will be destroyed
        Destroy(this.gameObject, lifetime);
    }

    public void Shoot(Vector3 moveDir, GameObject whoShot)
    {
        bullet_velocity = moveDir * bulletSpeed;
        shooter = whoShot;
    }

    private void FixedUpdate()
    {
        rb.velocity = bullet_velocity;
        bullet_velocity.y -= gravity;

        /* linecasting */
        RaycastHit hit;
        last_position = current_position;
        current_position = transform.position;

        if (Physics.Linecast(last_position, current_position, out hit))
        {
            switch (hit.transform.tag)
            {
                case "Player":
                    if (hit.transform.gameObject != shooter) // Ignores if the player shoots themselves
                    {
                        // Apply damage to the player
                        PlayerHealth playerHealth = hit.transform.gameObject.GetComponent<PlayerHealth>();
                        playerHealth.TakeDamage(damage, shooter, hit);

                        // If bullet is slow enough, destroy it
                        if (bullet_velocity.magnitude < 10f)
                        {
                            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red, lifetime);
                            Destroy(gameObject);
                            break;
                        }

                        // otherwise penetrate the player and come out the other side
                        BulletPenetration(hit);
                        if (drawDebug)
                        {
                            Debug.DrawLine(
                                hit.point,
                                hit.point + hit.normal,
                                Color.yellow,
                                lifetime
                            );
                        }
                    }
                    break;
                case "Cart":
                    MineCartExplosion cartExplodeScript =
                        hit.transform.gameObject.GetComponent<MineCartExplosion>();
                    cartExplodeScript.TakeDamage(damage);
                    Destroy(gameObject);
                    break;
                case "ExplosiveBarrel":
                    ExplosiveBarrel barrelScript =
                        hit.transform.gameObject.GetComponent<ExplosiveBarrel>();
                    barrelScript.TakeDamage(damage);
                    Destroy(gameObject);
                    break;
                case "Projectile": //Do nothing if it hits another projectile (or itself)
                    break;
                default:
                    //Get the normal of the hit point to reflect the bullet
                    Vector3 normal = hit.normal;

                    // Spawn hit effect
                    if (hitEffect)
                    {
                        ParticleSystem effect = Instantiate<ParticleSystem>(
                            hitEffect,
                            hit.point,
                            Quaternion.LookRotation(hit.normal)
                        );
                        effect.Play();
                        Destroy(effect.gameObject, effect.main.duration);
                    }

                    //Steep angles destroy the bullet
                    if (Vector3.Angle(bullet_velocity, normal) - 90 > 20.0f)
                    {
                        if (drawDebug)
                        {
                            Debug.DrawLine(hit.point, hit.point + normal, Color.red, lifetime);
                        }
                        Destroy(gameObject);
                    }
                    // Shallow angles ricochet
                    else
                    {
                        // Reflect * energy loss factor
                        bullet_velocity = Vector3.Reflect(bullet_velocity, normal) * 0.8f;

                        // Move the bullet back outside the object and update rb velocity
                        transform.position = hit.point;
                        Physics.SyncTransforms();
                        current_position = hit.point;
                        rb.MovePosition(hit.point);
                        rb.velocity = bullet_velocity;

                        if (drawDebug)
                        {
                            Debug.DrawLine(hit.point, hit.point + normal, Color.green, lifetime);
                        }
                    }
                    break;
            }
            if (drawDebug)
            {
                Debug.DrawLine(last_position, hit.point, Color.red, lifetime);
            }
        }
        else if (drawDebug)
        {
            Debug.DrawLine(last_position, current_position, Color.white, lifetime);
        }
    }

    //TODO: Make this a broad utility function with velocity as an input
    Vector3 ApplyConeSpread(float angle)
    {
        Vector3 originalVelocity = rb.velocity;
        Vector3 newVelocity = GetRandomConeDirection(originalVelocity, angle);
        return newVelocity;
    }

    Vector3 GetRandomConeDirection(Vector3 direction, float angle)
    {
        // Convert angle to radians
        float angleRad = angle * Mathf.Deg2Rad;

        // Generate a random point inside a unit sphere, then normalize to ensure directionality
        Vector3 randomPoint = Random.insideUnitSphere;

        // Slerp from the original direction to the random direction within the defined cone
        return Vector3
                .Slerp(direction, randomPoint, Random.Range(0f, Mathf.Sin(angleRad)))
                .normalized * direction.magnitude;
    }

    ///<summary>  Like Physics.Raycast, but finds the exit point of a ray once already inside an object </summary>
    /// <returns>True if an exit point is found, false otherwise.</returns>
    /// <example> Can be used with same parameters as Physics.Raycast, returns a boolean:
    ///<code>
    ///if (RayCastExit(hit.point, hit.transform.gameObject, new_velocity, maxDepth, out exitHit))
    ///</code>
    ///</example>
    /// <param name="startPoint">The starting point inside the object.</param>
    /// <param name="direction">The direction in which to cast the ray.</param>
    /// <param name="maxDistance">The maximum distance the ray can travel.</param>
    /// <param name="hit">Outputs the RaycastHit data for the exit point.</param>
    public bool RayCastExit(
        Vector3 startPoint,
        GameObject hitObj,
        Vector3 direction,
        float maxDistance,
        out RaycastHit hit
    )
    {
        Vector3 lastPoint = startPoint;
        float stepSize = 0.01f; // Small increment to step forward
        Vector3 currentPoint = startPoint + direction * stepSize;
        float totalDistance = 0f;

        hit = new RaycastHit(); // Ensure it's initialized

        Collider[] colliders = Physics.OverlapSphere(startPoint, 0.01f);
        if (colliders.Length == 0)
        {
            return false; // No object found at the start point
        }

        // Step forward until the ray exits the object
        while (totalDistance < maxDistance)
        {
            if (!Physics.Raycast(currentPoint, direction, out RaycastHit stepHit, stepSize))
            {
                break; // First point outside the object
            }

            // Move forward
            lastPoint = currentPoint;
            currentPoint += direction.normalized * stepSize;
            totalDistance += stepSize;
        }

        if (drawDebug)
            Debug.DrawLine(currentPoint, lastPoint, Color.blue, lifetime);

        // Raycast back to the last in-object point to refine the exit point
        if (Physics.Linecast(currentPoint, lastPoint, out hit))
        {
            return true;
        }

        // No valid exit point found
        return false;
    }

    /// <summary>
    /// Finds entrance/exit points of a bullet through an object, modifies the bullet's position and velocity using spread angle
    /// </summary>
    private void BulletPenetration(RaycastHit hit)
    {
        float spreadAngle = 5.0f;
        float velocityLoss = 0.5f; // Magnitude of energy lost
        float maxDepth = 2.0f; // Maximum depth to penetrate
        Vector3 new_velocity = rb.velocity;

        //Cast a ray to find the exit point of the player
        RaycastHit exitHit;
        if (RayCastExit(hit.point, hit.transform.gameObject, new_velocity, maxDepth, out exitHit))
        {
            float penDistance = Vector3.Distance(hit.point, exitHit.point);
            // Move the bullet to the exit point and update rb velocity
            transform.position = exitHit.point;
            Physics.SyncTransforms();
            current_position = exitHit.point;
            rb.MovePosition(exitHit.point);
            if (drawDebug)
            {
                Debug.DrawLine(hit.point, exitHit.point, Color.magenta, lifetime);
                Debug.DrawLine(
                    exitHit.point,
                    exitHit.point + exitHit.normal,
                    Color.green,
                    lifetime
                );
            }
            rb.velocity *= velocityLoss * penDistance;
            new_velocity = ApplyConeSpread(spreadAngle);
            bullet_velocity = rb.velocity;
        }

        rb.velocity = new_velocity;
    }
}
