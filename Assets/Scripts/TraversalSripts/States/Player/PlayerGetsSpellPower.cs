using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetsSpellPower : PlayerBaseState
{
    private readonly int PowerUpHash = Animator.StringToHash("PowerUp");

    private const float CrossFadeDuration = 0.1f;



    public PlayerGetsSpellPower(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.isSpell = true;
        AudioManager.Instance.Stop("walk");
        AudioManager.Instance.Play("Healing");
        stateMachine.Health.health = 100;
        stateMachine.Health.HealthBarHandler();
        stateMachine.Animator.CrossFadeInFixedTime(PowerUpHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        
        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("PowerUp"))
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));

    }

    public override void Exit()
    {
      
    }
    
}
