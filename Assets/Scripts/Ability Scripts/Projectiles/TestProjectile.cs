using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{

    private Rigidbody rb;

    bool drawDebug = true;

    public float bulletSpeed, damage, gravity = 9.8f;
    private GameObject shooter; // Which player shot this bullet

    private Vector3 bullet_velocity = new Vector3 (0, 0, 0); //the vector that contains bullet's current speed
	private Vector3 last_position = new Vector3(0,0,0), current_position = new Vector3 (0,0,0);

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
                        PlayerStatManager statScript = hit.transform.gameObject.GetComponent<PlayerStatManager>();
                        statScript.TakeDamage(damage, shooter);
                        Destroy(gameObject);
                    }
                    break;
                case "Cart":
                    MineCartExplosion cartExplodeScript = hit.transform.gameObject.GetComponent<MineCartExplosion>();
                    cartExplodeScript.TakeDamage(damage);
                    Destroy(gameObject);
                    break;
                case "ExplosiveBarrel":
                    ExplosiveBarrel barrelScript = hit.transform.gameObject.GetComponent<ExplosiveBarrel>();
                    barrelScript.TakeDamage(damage);
                    Destroy(gameObject);
                    break;
                case "Projectile": //Do nothing if it hits another projectile (or itself)
                    break;  
                default: // Prefer to use tag like "Terrain" but this is ok
                    //Get the normal of the hit point to reflect the bullet
                    Vector3 normal = hit.normal;
                    Debug.Log(Vector3.Angle(bullet_velocity, normal) - 90);
        
                    // Shallow angles ricochet, otherwise destroy the bullet
                    if (Vector3.Angle(bullet_velocity, normal) - 90 < 20.0f)
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
                    else
                    {
                        if (drawDebug)
                        {
                            Debug.DrawLine(hit.point, hit.point + normal, Color.red, lifetime);
                        }
                        Destroy(gameObject);
                    }


                    break;
            }
		}
		if (drawDebug)
        {
            Debug.DrawLine (last_position, current_position, Color.red, lifetime);
        }
            
    }
}
