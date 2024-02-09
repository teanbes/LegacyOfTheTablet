using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private bool alreadyAppliedForce;

    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        
        AudioManager.Instance.Play(attack.soundName);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);


        FaceMouseCursor();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }
            if (stateMachine.isCombo)
            {
                if (stateMachine.InputReader.IsAttacking)
                    TryComboAttack(normalizedTime);
            }
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {

    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return; }

        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState(new PlayerAttackingState(stateMachine,attack.ComboStateIndex));
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyAppliedForce = true;
    }

    private void FaceMouseCursor()
    {
        // Raycast from the mouse cursor to determine the point on the ground
        Ray ray = stateMachine.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, stateMachine.GroundLayer))
        {
            Vector3 lookPos = hit.point - stateMachine.transform.position;
            lookPos.y = 0f;

            stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }
}
