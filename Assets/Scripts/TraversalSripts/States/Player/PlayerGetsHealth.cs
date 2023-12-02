using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetsHealth : PlayerBaseState
{
    private readonly int PowerUpHash = Animator.StringToHash("PowerUp");

    private const float CrossFadeDuration = 0.1f;



    public PlayerGetsHealth(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        AudioManager.Instance.Stop("walk");
        stateMachine.Animator.CrossFadeInFixedTime(PowerUpHash, CrossFadeDuration);
        AudioManager.Instance.Play("healing");
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
