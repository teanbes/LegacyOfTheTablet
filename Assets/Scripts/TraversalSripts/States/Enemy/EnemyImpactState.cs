using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    // Array of hit sounds
    private string[] hitSounds = { "hit2", "hit3", "hit4", "hit5" };

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Select a random hit sound from the array
        string randomHitSound = hitSounds[Random.Range(0, hitSounds.Length)];
        AudioManager.Instance.Play(randomHitSound);
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);

    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit() { }
}
