using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
            if (collision.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine stateMachine))
            {
                stateMachine.uiManager.SaveGame();
                Destroy(gameObject);
            }
    }
}
