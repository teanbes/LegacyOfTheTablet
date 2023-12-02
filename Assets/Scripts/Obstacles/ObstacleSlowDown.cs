using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class ObstacleSlowDown : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    
    public float speed = 2f;
    private float originalSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine stateMachine))
            {
                originalSpeed = stateMachine.FreeLookMovementSpeed;
                stateMachine.FreeLookMovementSpeed = speed;
                //health.DealDamage(damage);
            }

          

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine stateMachine))
            {

                stateMachine.FreeLookMovementSpeed = originalSpeed;
                //health.DealDamage(damage);
            }



        }

    }

}
