using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCombos : MonoBehaviour
{
    
    private PlayerStateMachine stateMachine;
    [SerializeField] private GameObject magicWall2;
    [SerializeField] private GameObject EnemiesArea1;

   
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (collision.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine stateMachine))
            {

                magicWall2.SetActive(false);
                this.stateMachine = stateMachine;
                stateMachine.isCombo = true;
                EnemiesArea1.SetActive(true);
                stateMachine.SwitchState(new PlayerGetsSpellPower(stateMachine));

            }

            Destroy(gameObject);

        }
    }
}
