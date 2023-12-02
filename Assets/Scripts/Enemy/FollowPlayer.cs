using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public NavMeshAgent enemy;
    [SerializeField] public Transform Player;
    [SerializeField] public bool canSeePlayer;

    [SerializeField] public GameObject playerRef;
    [SerializeField] public float radius;
    [Range(0, 360)]
    [SerializeField] public float angle;
    [SerializeField] private float rangeOfAttack;
    [SerializeField] private bool isOnRange;

    [SerializeField] public LayerMask targetMask;
    [SerializeField] public LayerMask obstructionMask;
    [SerializeField] public float maxDistance = 20f;

    //Health
    [SerializeField] public GolemHealth golemHealth;
    [SerializeField] public int damage;
    [SerializeField] public  bool golemDead;

    // Projectile variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootInterval;
    [SerializeField] private Transform projectileSpawnPoint;
    private float lastShootTime = 0f;
    RaycastHit hit;
    Vector3 direction;


    // Animation Vars
    [SerializeField] public Animator animator;
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int Attack01Hash = Animator.StringToHash("Attack01");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int GetHitHash = Animator.StringToHash("GetHit");
    private readonly int DieHash = Animator.StringToHash("Die");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        golemDead = false;
        canSeePlayer = false;
        isOnRange = false;

        if (damage <=0)
            damage = 20;
        if (!bulletPrefab)
            Debug.Log("Set bullet prefab in inspector");

        if (maxDistance <= 0f)
            maxDistance = 10f;

       
        if (bulletSpeed <= 0f)
            bulletSpeed = 10f;

        if (shootInterval <= 0f)
            shootInterval = 2f;

        StartCoroutine(FindPlayer());
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit hit;
        Vector3 rayCastOrigin = transform.position + new Vector3(0f, -0.5f, 0f);
        // to make raycast only affect on a layer 
        int layermask = 1 << 6;
        bool tempraycast = Physics.Raycast(rayCastOrigin, transform.forward, out hit, maxDistance, layermask);


        if (canSeePlayer == true && golemDead == false)
        {
            // Player and enemy positions vars
            float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
            Vector3 directionToTarget = (Player.position - transform.position).normalized;

            if (canSeePlayer == true && golemDead == false)
            {
                               
                // Follow player if can see and is not in range of attack
                if (distanceToPlayer >= rangeOfAttack && distanceToPlayer <= radius)
                {
                    enemy.SetDestination(Player.position);
                    animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, Time.deltaTime);
                    
                }
                // Enemy stops at range but rotates towards player to keep shooting from range
                else
                {
                    transform.rotation = Quaternion.LookRotation(directionToTarget);
                    enemy.ResetPath();
                    animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, Time.deltaTime);
                    isOnRange = true;
                }

                 // Raycast for attacking player
                if (tempraycast && isOnRange)
                {
                    Debug.DrawRay(rayCastOrigin, transform.forward * hit.distance, Color.red);

                    if (hit.collider.CompareTag("Player"))
                    {

                        if (Time.time - lastShootTime >= shootInterval) 
                        {
                            AudioManager.Instance.Play("Hit");
                            animator.CrossFadeInFixedTime(Attack01Hash, CrossFadeDuration);
                            
                            lastShootTime = Time.time;
                        }
                    }

                }
                else if (!tempraycast)
                {

                    Debug.DrawRay(rayCastOrigin, transform.forward * maxDistance, Color.blue);
                }

            }
        }
        else if (canSeePlayer == false && golemDead == false)
        {
            animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
            animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, Time.deltaTime);
            isOnRange = false;
        }



    }

    private void OnEnable()
    {
        golemHealth.OnTakeDamage += HandleTakeDamage;
        golemHealth.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        golemHealth.OnTakeDamage -= HandleTakeDamage;
        golemHealth.OnDie -= HandleDie;
    }

    // Delays player finding in case of instatntiating at begining of runtime 
    private IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(1f); // wait for 1 second

        playerRef = GameObject.FindGameObjectWithTag("Player");
        Player = playerRef.transform;

    }

    // Runs FieldOfViewCheck() for performance 
    private IEnumerator FOVRoutine()
    {

        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    // Checks if player is in field of view
    private void FieldOfViewCheck()
    {
        // Check for colitions in the radius of sphere cast
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        // If there is at least 1 collition player is in field of view

        if (rangeChecks.Length != 0 && golemDead == false)
        {
            
            Transform target = rangeChecks[0].transform;


            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distancetoTarget = Vector3.Distance(transform.position, target.position);
                

                if (distancetoTarget <= maxDistance && !Physics.Raycast(transform.position, directionToTarget, distancetoTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else
            canSeePlayer = false;
    }

    private void HandleTakeDamage()
    {
        animator.CrossFadeInFixedTime(GetHitHash, CrossFadeDuration);
        StartCoroutine(AnimationDelay());
    }

    private void HandleDie()
    {
        golemDead = true;
        animator.CrossFadeInFixedTime(DieHash, CrossFadeDuration);

    }

    private IEnumerator AnimationDelay()
    {
        float delay = 2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
        }
    }

    public void ShootPlayer()
    {
        // Calculate direction to player
        Vector3 direction = ((playerRef.transform.position + new Vector3(0,0.5f,0)) - transform.position).normalized;

        // Instantiate bullet and set its position and rotation
        GameObject bullet = Instantiate(bulletPrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));

        // Add force to bullet in direction of player
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}