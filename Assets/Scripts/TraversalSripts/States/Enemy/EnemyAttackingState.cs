using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly int AttackProjectileHash = Animator.StringToHash("AttackProjectile");

    private const float TransitionDuration = 0.1f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // If the enemy is type shooter
        if (stateMachine.IsShooter)
        {
            stateMachine.Animator.CrossFadeInFixedTime(AttackProjectileHash, TransitionDuration);
            stateMachine.Animator.CrossFadeInFixedTime(AttackProjectileHash, TransitionDuration);
        }
        else if (!stateMachine.IsShooter) 
        {
            stateMachine.Weapon.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnockback);
            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        }

       
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.IsShooter)
        {
            if (GetNormalizedTime(stateMachine.Animator, "AttackProjectile") >= 1)
            {
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            }
        }
        else if (!stateMachine.IsShooter)
        {
            if (GetNormalizedTime(stateMachine.Animator, "Attack") >= 1)
            {
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            }
        }

        FacePlayer();
    }

    public override void Exit() { }


}
