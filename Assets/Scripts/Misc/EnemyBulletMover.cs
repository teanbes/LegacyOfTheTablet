using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMover : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float stopFollowDistance = 15f;
    [SerializeField] private GameObject hitParticle;

    private Rigidbody rb;
    private bool isFollowing = true;

    private int bulletDamage = 20;
    private float knockback = 10;



    private void Start()
    {
        if (GameManager.Instance.playerIsDead) { return; }

        player = FindObjectOfType<PlayerStateMachine>().transform;

        rb = GetComponent<Rigidbody>();

        // move projectile to position
        rb.velocity = transform.forward * initialSpeed;
    }

    private void Update()
    {
        //if (GameManager.Instance.playerIsDead) { return; }

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

        if (other.gameObject.CompareTag("Ground"))
        {
            Instantiate(hitParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
