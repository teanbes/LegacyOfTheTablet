using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickSwordState : PlayerBaseState
{
    private readonly int PickingUpHash = Animator.StringToHash("PickingUp");
    
    private const float CrossFadeDuration = 0.1f;

    public PlayerPickSwordState(PlayerStateMachine stateMachine) : base(stateMachine) {}


    public override void Enter()
    {

        stateMachine.isWeapon = true;
        AudioManager.Instance.Looping("walk", false);
        stateMachine.StartCoroutine(PlaySoundEffect());
        stateMachine.Animator.CrossFadeInFixedTime(PickingUpHash, CrossFadeDuration);
        stateMachine.Health.health = 100;
        stateMachine.Health.HealthBarHandler();
        stateMachine.uiManager.SaveGame();
    }
   
    public override void Tick(float deltaTime)
    {
        Debug.Log(stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5 && stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("PickingUp"))
            stateMachine.aura.SetActive(true);

        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("PickingUp"))
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));

    }

    public override void Exit()
    {
        stateMachine.aura.SetActive(false);
        AudioManager.Instance.Looping("walk", true);

    }

    private IEnumerator PlaySoundEffect()
    {
        yield return new WaitForSeconds(5);
        AudioManager.Instance.Play("powerup");
    }
   
}
