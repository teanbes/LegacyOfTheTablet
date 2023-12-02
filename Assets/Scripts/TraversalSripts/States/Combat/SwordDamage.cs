using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GolemHealth golemHealth;

    private void Start()
    {
        if (damage <= 0)
            damage = 20;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("SwordCollider"))
        {
            golemHealth.DealDamage(damage);
        }
    }
}
