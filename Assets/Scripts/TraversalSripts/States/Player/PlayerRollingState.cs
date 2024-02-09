using UnityEngine;

public class PlayerRollingState : PlayerBaseState
{
    private readonly int RollHash = Animator.StringToHash("Roll");

    private const float CrossFadeDuration = 0.1f;
    private float previousFrameTime;
    private bool isDashing = false;
    private float dashCooldown = 2f;  // Set your desired dash cooldown time
    private float dashDuration = 1f; // Set your desired dash duration
    private float dashSpeedMultiplier = 2f; // Set the speed multiplier during dash
    private float originalMovementSpeed;

    public PlayerRollingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(RollHash, CrossFadeDuration);
        AudioManager.Instance.Play("shoot");
        OnDash();
    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Roll");

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            Vector3 dashMovement = CalculateDashMovement();
            Move(dashMovement * stateMachine.RollMovementSpeed, deltaTime);
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
            previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        stateMachine.FreeLookMovementSpeed = originalMovementSpeed;
    }

    public void OnDash()
    {
        isDashing = true;
        originalMovementSpeed = stateMachine.FreeLookMovementSpeed; // Store the original movement speed
        stateMachine.FreeLookMovementSpeed *= dashSpeedMultiplier;
    }

    private Vector3 CalculateDashMovement()
    {
        return stateMachine.gameObject.transform.forward; // Move the player forward during dash
    }


    private bool CanDash()
    {
        return true;
    }
}
