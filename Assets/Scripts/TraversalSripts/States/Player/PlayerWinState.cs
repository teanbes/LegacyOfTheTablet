using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWinState : PlayerBaseState
{
    private readonly int KissHash = Animator.StringToHash("Kiss");
    private const float CrossFadeDuration = 0.1f;

    private float timer;
    public PlayerWinState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        timer = 7;
        stateMachine.Animator.CrossFadeInFixedTime(KissHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if ((timer -= Time.deltaTime) <= 0)
        {
            SceneManager.LoadScene("YouWin");
        }
    }

    public override void Exit()
    {

    }
}
