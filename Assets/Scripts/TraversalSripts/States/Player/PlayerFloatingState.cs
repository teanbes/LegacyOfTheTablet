using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloatingState : PlayerBaseState
{
    private readonly int FloatingHash = Animator.StringToHash("Floating");

    private const float CrossFadeDuration = 0.1f;




    public PlayerFloatingState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FloatingHash, CrossFadeDuration);

    }

    public override void Tick(float deltaTime)
    {
       
    }

    public override void Exit()
    {
       
    }



}
