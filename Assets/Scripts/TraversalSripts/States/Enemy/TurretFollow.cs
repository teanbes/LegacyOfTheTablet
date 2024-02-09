using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurretFollow : MonoBehaviour
{
    // Turret movement and detection
    [SerializeField] Transform turretBase;
    [SerializeField] Transform turretElevation;
    [SerializeField] private GameObject playerRef;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask Walls;
    [SerializeField] private float traverseSpeed = 60f;
    [SerializeField] private bool canSeePlayer;

       
    // Projectile variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] Transform projectileSpawnPoint;
    private float lastShootTime = 0f;
    public float maxDistance = 20f;

    // Animation vars
    [SerializeField] private Animator animator;
    private readonly int ShootHash = Animator.StringToHash("Shoot");
    private const float CrossFadeDuration = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FOVRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = projectileSpawnPoint.position + new Vector3(0f, 0f, 0f);
        // to make raycast only affect on a layer 
        int layermask = 1 << 6;

        if (Physics.Raycast(rayCastOrigin, projectileSpawnPoint.forward, out hit, maxDistance, Walls))
        {
            Debug.DrawRay(rayCastOrigin, projectileSpawnPoint.forward * maxDistance, Color.blue);
            animator.Play("Idle");
            return;
        }

         if (Physics.Raycast(rayCastOrigin, projectileSpawnPoint.forward, out hit, maxDistance, targetMask))
        {
            Debug.DrawRay(rayCastOrigin, projectileSpawnPoint.forward * hit.distance, Color.red);
            // Check if raycast hit player
            if (hit.collider.GetComponent<PlayerStateMachine>())
            {
                float randomDelay = Random.Range(0.0f, 1f);
                Invoke("PlayShootAnimation", randomDelay);
            }

        }
        else
        {
            animator.Play("Idle");
            Debug.DrawRay(rayCastOrigin, projectileSpawnPoint.forward * maxDistance, Color.blue);
        }

        //RaycastHit hit;
        //Vector3 rayCastOrigin = projectileSpawnPoint.position + new Vector3(0f, 0f, 0f);
        //// to make raycast only affect on a layer 
        //int layermask = 1 << 6;


        //if (Physics.Raycast(rayCastOrigin, projectileSpawnPoint.forward, out hit, maxDistance, layermask))
        //{
        //    Debug.DrawRay(rayCastOrigin, projectileSpawnPoint.forward * hit.distance, Color.red);
        //    // Check if raycast hit player
        //    if (hit.collider.CompareTag("Player"))
        //    {
        //        if (Time.time - lastShootTime >= shootInterval)
        //        {
        //            //animator.CrossFadeInFixedTime(ShootHash, CrossFadeDuration);
        //            ShootPlayer(hit.point + new Vector3 (0, 0.5f, 0));
        //            lastShootTime = Time.time;
        //        }
        //    }

        //}
        //else
        //{
        //  Debug.DrawRay(rayCastOrigin, projectileSpawnPoint.forward * maxDistance, Color.blue);
        //}

    }

    // Runs FieldOfViewCheck() for performance 
    private IEnumerator FOVRoutine()
    {

        float delay = 0.01f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    // Turret field of view
    private void FieldOfViewCheck()
    {
        // Check for colitions in the radius of sphere cast
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        // If there is at least 1 collition player is in field of view
        if (rangeChecks.Length != 0)
        {
            canSeePlayer = true;
            //animator.Play(ShootHash);
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            // Calculations to manipulate only Y axxis rotation
            Quaternion LookAtRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion LookAtRotationOnly_Y = Quaternion.Euler(turretBase.transform.eulerAngles.x, LookAtRotation.eulerAngles.y, turretBase.transform.eulerAngles.z);

            // Turret base rotyation on Y axis
            turretBase.rotation = Quaternion.Slerp(turretBase.rotation, LookAtRotationOnly_Y, Time.deltaTime * traverseSpeed); ;

            // Turret canyon elevation
            turretElevation.rotation = Quaternion.RotateTowards(
                    Quaternion.LookRotation(turretElevation.forward),
                    Quaternion.LookRotation(directionToTarget),
                    traverseSpeed);

        }
        else
        {
            canSeePlayer = false;
            //animator.Play("Idle");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void ShootPlayer()
    {
        AudioManager.Instance.Play("cannon");
        // Instantiate bullet and set its position and rotation
        GameObject bullet = Instantiate(bulletPrefab, projectileSpawnPoint.position, Quaternion.identity);
        Destroy(bullet, 5.0f);
    }

    private void PlayShootAnimation()
    {
        animator.Play(ShootHash);
    }

}
