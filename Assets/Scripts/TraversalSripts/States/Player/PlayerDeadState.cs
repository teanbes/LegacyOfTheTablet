using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadState : PlayerBaseState
{
    private float timer;
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        timer = 4;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        stateMachine.Ragdoll.ToggleRagdoll(true);
        stateMachine.Weapon.gameObject.SetActive(false);
        stateMachine.DeathParticles.SetActive(true);

          
    }

    public override void Tick(float deltaTime) 
    {
        if ((timer -= Time.deltaTime) <=0)
        {
            
            stateMachine.gameOverPanel.SetActive(true);
            stateMachine.uiManager.PauseBackgorundMusic();
            
        }
    }

    public override void Exit() { }


}
