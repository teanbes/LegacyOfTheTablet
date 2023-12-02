using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private float raycastYoffset ;
    [SerializeField] private Transform PlayerRef ;
    RaycastHit hit;

    // Projectile variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootInterval;
    [SerializeField] private Transform projectileSpawnPoint;
    private float lastShootTime = 0f;

    private readonly int Attack01Hash = Animator.StringToHash("Attack01");
    private const float CrossFadeDuration = 0.1f;

    private FollowPlayer followPlayer;

    Vector3 direction;

    private void Start()
    {
        followPlayer = GetComponent<FollowPlayer>();

        if (!bulletPrefab)
            Debug.Log("Set bullet prefab in inspector");

        if (maxDistance <=0f)
            maxDistance = 10f;

        if (raycastYoffset <= 0f)
            raycastYoffset = 0.5f;

        if (bulletSpeed <= 0f)
            bulletSpeed = 10f;

        if (shootInterval <= 0f)
            shootInterval = 2f;

    }

    private void Update()
    {
        
        //RaycastHit hit;
        Vector3 rayCastOrigin = transform.position + new Vector3 (0f, -0.5f, 0f);
        // to make raycast only affect on a layer 
        int layermask = 1 << 6;
        bool tempraycast = Physics.Raycast(rayCastOrigin, transform.forward, out hit, maxDistance, layermask);

        if (followPlayer.canSeePlayer == true && followPlayer.golemDead == false)
        {
            if (tempraycast)
            {
                Debug.DrawRay(rayCastOrigin, transform.forward * hit.distance, Color.red);

                if (hit.collider.CompareTag("Player"))
                {

                    if (Time.time - lastShootTime >= shootInterval)
                    {
                        followPlayer.animator.CrossFadeInFixedTime(Attack01Hash, CrossFadeDuration);
                        //ShootPlayer(hit.point); remove hit.point reference to ShootPlayer() from animation

                        lastShootTime = Time.time;
                    }
                }

            }
            else
            {

                Debug.DrawRay(rayCastOrigin, transform.forward * maxDistance, Color.blue);
            }

            direction = (hit.point - transform.position).normalized;
        }

    }

    public void ShootPlayer()
    {
        // Calculate direction to player
        Vector3 direction = (hit.point - transform.position).normalized;

        // Instantiate bullet and set its position and rotation
        GameObject bullet = Instantiate(bulletPrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));

        // Add force to bullet in direction of player
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
    }
}
