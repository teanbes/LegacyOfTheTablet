using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickingUp : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine stateMachine;
    [SerializeField] private GameObject magicWall1;

    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("Delay", 2.3f);
            magicWall1.SetActive(false);
            stateMachine.SwitchState(new PlayerPickSwordState(stateMachine));

        }


    }

    private void Delay()
    {
        Destroy(gameObject);
    }
}




