using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPowerUp : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine stateMachine;
    [SerializeField] private GameObject magicWall3;
    [SerializeField] private GameObject EnemiesArea2;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            magicWall3.SetActive(false);
            Invoke("Delay", 2.3f);
            EnemiesArea2.SetActive(true);
            stateMachine.SwitchState(new PlayerGetsSpellPower(stateMachine));


        }


    }

    private void Delay()
    {
        Destroy(gameObject);
    }
}
