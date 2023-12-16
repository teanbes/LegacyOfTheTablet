using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWin : MonoBehaviour
{
    public PlayerStateMachine stateMachine;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStateMachine>())
        {
            stateMachine.SwitchState(new PlayerWinState(stateMachine));
            Invoke("DestroyTablet", 5.0f);
        }
    }

    private void DestroyTablet()
    {
        Destroy(gameObject);
    }
}
