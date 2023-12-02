using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrollingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

   


    public EnemyPatrollingState(EnemyStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
      
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {

       

        if(IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        Patrol(deltaTime);

        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);

    }

    public override void Exit()
    {
        
    }

    private void Patrol(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            if (stateMachine.Agent.remainingDistance < stateMachine.DistThreshhold) 
            {
                stateMachine.PathIndex++;
                stateMachine.PathIndex %= stateMachine.Path.Length; // loops thru array and goes back to first index (is the same as saying -> if (pathIndex >= path.Length) pathIndex = 0)

                stateMachine.PathTarget = stateMachine.Path[stateMachine.PathIndex].transform;

                stateMachine.Agent.SetDestination(stateMachine.PathTarget.position);


            }
                Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

                FacePatrolPoint();
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        Debug.DrawLine(stateMachine.transform.position, stateMachine.PathTarget.position, Color.red);
    }
    
   
}
