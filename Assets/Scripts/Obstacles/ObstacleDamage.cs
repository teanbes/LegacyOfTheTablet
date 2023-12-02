using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    public int damage;
    private float knockback;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (other.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(damage);
            }

            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                myCollider = other.gameObject.GetComponent<Collider>();
                Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
                forceReceiver.AddForce(direction * knockback);
            }


        }
        


    }
}
