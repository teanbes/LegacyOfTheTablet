using System.Collections;
using UnityEngine;



public class TurretFollowProjectile : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float initialSpeed = 7f;
    [SerializeField] private float followSpeed = 7f;
    [SerializeField] private float stopFollowDistance = 8f;
    [SerializeField] private GameObject hitParticle;
    [SerializeField] private int damage = 8;
    [SerializeField] private LayerMask LayersMask;

    private Rigidbody rb;
    private bool isFollowing = true;

    private int bulletDamage = 20;
    private float knockback = 10;



    private void Start()
    {
        if (GameManager.Instance.playerIsDead) { return; }

        player = GameObject.FindObjectOfType<PlayerStateMachine>().transform;
    
        rb = GetComponent<Rigidbody>();

        // move projectile to position
        rb.velocity = transform.up * initialSpeed;
    }

    private void Update()
    {
        if (GameManager.Instance.playerIsDead) { return; }

        if (isFollowing && player != null)
        {
            Vector3 playerOffset = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
            Vector3 directionToPlayer = playerOffset - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer <= stopFollowDistance)
            {
                isFollowing = false;
            }
            else
            {
                // Follow the player
                directionToPlayer.Normalize();
                rb.velocity = directionToPlayer * followSpeed;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<PlayerStateMachine>())
        {
            Instantiate(hitParticle, transform.position, transform.rotation);
            rb.transform.parent = null;
            Destroy(this.gameObject);


            if (other.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(bulletDamage);
            }

            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - other.transform.position).normalized;
                forceReceiver.AddForce(direction * knockback);
            }
        }

        if ((LayersMask & (1 << other.gameObject.layer)) != 0)
        {
            Instantiate(hitParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
