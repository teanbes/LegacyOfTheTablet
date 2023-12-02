using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCollition : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    [SerializeField] private GameObject explosionEffect;
    private int bulletDamage = 20;
    private float knockback = 10;

    
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            explosionEffect.SetActive(true);
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
    }
}
