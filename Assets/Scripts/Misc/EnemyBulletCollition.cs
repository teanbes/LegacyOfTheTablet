using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCollition : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    [SerializeField] private GameObject explosionEffect;
    private int bulletDamage = 20;
    private float knockback = 10;

    ParticleSystem rb;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void Start()
    {
        rb = explosionEffect.GetComponentInChildren<ParticleSystem>();
        

    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            explosionEffect.SetActive(true);
            rb.transform.parent = null;
            Destroy(this.gameObject);
            

            if (collision.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(bulletDamage);
            }

            if (collision.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (collision.transform.position - myCollider.transform.position).normalized;
                forceReceiver.AddForce(direction * knockback);
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            
            explosionEffect.SetActive(true);
            rb.transform.parent = null;
            Destroy(this.gameObject);
            
        }
    }
}

