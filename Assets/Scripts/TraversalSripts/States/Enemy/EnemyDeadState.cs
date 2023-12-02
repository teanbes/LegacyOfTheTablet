using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    
    
    

    public override void Enter()
    {
        
        
        stateMachine.Ragdoll.ToggleRagdoll(true);
        stateMachine.Weapon.gameObject.SetActive(false);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.DeathParticles.SetActive(true);


    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }

    

}
