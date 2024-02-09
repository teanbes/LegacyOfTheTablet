using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    [Header("Type Of Enemy")]
    [SerializeField] private bool isMinotaur;
    [SerializeField] private bool isScavanger;

    [Header("Enemy Parameters")]
    [SerializeField] public NavMeshAgent enemy;
    [SerializeField] public Transform Player;
    [SerializeField] public bool canSeePlayer;
    [SerializeField] private Health health;
    [SerializeField] private HealthBar healthBar;

    [SerializeField] public GameObject playerRef;
    [SerializeField] public float radius;
    [Range(0, 360)]
    [SerializeField] public float angle;
    [SerializeField] private float rangeOfAttack;
    [SerializeField] private bool isOnRange;

    [SerializeField] public LayerMask targetMask;
    [SerializeField] public LayerMask obstructionMask;
    [SerializeField] public float maxDistance = 20f;

    [Header("Enemy Health")]
    [SerializeField] public Health enemyHealth;
    [SerializeField] public int damage;
    [SerializeField] public  bool enemyDead;

    [Header("Projectile Vars")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootInterval;
    [SerializeField] private Transform projectileSpawnPoint;
    private float lastShootTime = 0f;
    RaycastHit hit;
    Vector3 direction;


    [Header("Animations")]
    [SerializeField] public Animator animator;
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int Attack01Hash = Animator.StringToHash("attack3");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int GetHitHash = Animator.StringToHash("Hit");
    private readonly int DieHash = Animator.StringToHash("Die");
    private const float CrossFadeDuration = 0.3f;
    private const float AnimatorDampTime = 0.1f;

    private bool isAttacking = false;
    private string[] attackSounds = { "swoosh3", "swoosh5", "swoosh6" };

    // Start is called before the first frame update
    void Start()
    {
        animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration); 
        enemyDead = false;
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
        if (enemyDead) return;

        //RaycastHit hit;
        Vector3 rayCastOrigin = transform.position + new Vector3(0f, -0.5f, 0f);
        // to make raycast only affect on a layer 
        int layermask = 1 << 6;
        bool tempraycast = Physics.Raycast(rayCastOrigin, transform.forward, out hit, maxDistance, layermask);


        if (canSeePlayer == true && enemyDead == false)
        {
            // Player and enemy positions vars
            float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
            Vector3 directionToTarget = (Player.position - transform.position).normalized;

            // Follow player if can see and is not in range of attack
            if (distanceToPlayer >= rangeOfAttack && distanceToPlayer <= radius && !isAttacking)
            {
                enemy.SetDestination(Player.position);
                animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, Time.deltaTime);
                isOnRange = false;
            }
            // Enemy stops at range but rotates towards player to keep shooting from range
            else if (distanceToPlayer <= rangeOfAttack && distanceToPlayer <= radius)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0f, directionToTarget.z));

                transform.rotation = targetRotation;
                enemy.ResetPath();
                enemy.velocity = Vector3.zero;
                //animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, Time.deltaTime);
                isOnRange = true;

                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
            else if (distanceToPlayer > rangeOfAttack && animator.GetCurrentAnimatorStateInfo(0).length == 0 && !isAttacking)
            {
                // Player is out of range, stop attacking and follow again
                StopAttack();
                animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, Time.deltaTime);
                enemy.SetDestination(Player.position);
                isOnRange = false;
            }

        }
        else if (canSeePlayer == false && enemyDead == false)
        {
            enemy.ResetPath();
            enemy.velocity = Vector3.zero;
            animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
            animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, Time.deltaTime);
            isOnRange = false;
        }
    }

    private void OnEnable()
    {
        enemyHealth.OnTakeDamage += HandleTakeDamage;
        enemyHealth.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        enemyHealth.OnTakeDamage -= HandleTakeDamage;
        enemyHealth.OnDie -= HandleDie;
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

        if (rangeChecks.Length != 0 && enemyDead == false)
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
        int currentHealth = health.health;
        healthBar.UpdateHealthBar(currentHealth);
        //animator.CrossFadeInFixedTime(GetHitHash, CrossFadeDuration);
        //StartCoroutine(AnimationDelay());
    }

    private void HandleDie()
    {
        enemyDead = true;
        animator.SetTrigger("Dead");
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

    IEnumerator AttackPlayer()
    {

        // Set the attacking flag to true
        isAttacking = true;

        // Play attack sound
        if (isMinotaur) 
        {
            string randomAttackSound = attackSounds[Random.Range(0, attackSounds.Length)];
            AudioManager.Instance.Play(randomAttackSound);
        }
        if (isScavanger) 
        { 
            AudioManager.Instance.Play("swoosh4");
        }

        // Play attack animation
        animator.CrossFadeInFixedTime(Attack01Hash, CrossFadeDuration);

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);

        // Set the attacking flag to false after the attack is finished
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void StopAttack()
    {
        // Stop the attack animation and reset the attacking flag
        animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        isAttacking = false;
    }
}