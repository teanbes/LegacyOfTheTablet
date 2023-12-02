using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollectable : MonoBehaviour
{
    [SerializeField] public int healthUpValue;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(healthUpValue == 0) { healthUpValue = 20; }

            if (collision.TryGetComponent<Health>(out Health health))
            {
                health.IncreaseHealth(healthUpValue);
            }
            
            Destroy(gameObject);

        }
    }
}
