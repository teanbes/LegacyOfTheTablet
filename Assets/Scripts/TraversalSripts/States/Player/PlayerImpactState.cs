using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 0.5f;

    // Array of hurt sounds
    private string[] hurtSounds = { "hurt1", "hurt2", "hurt3" };

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Select a random hurt sound from the array
        string randomHurtSound = hurtSounds[Random.Range(0, hurtSounds.Length)];
        AudioManager.Instance.Play(randomHurtSound);
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit() { }
}
