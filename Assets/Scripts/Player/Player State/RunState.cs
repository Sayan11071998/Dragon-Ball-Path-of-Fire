using UnityEngine;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class RunState : BasePlayerState
    {
        public RunState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter() => playerView.UpdateRunAnimation(true);

        public override void Update()
        {
            float moveInput = playerView.MoveInput;
            Vector2 velocity = GetCurrentVelocity();
            velocity.x = moveInput * playerModel.MoveSpeed;
            SetVelocity(velocity);

            if (moveInput > 0 && !IsFacingRight())
                FlipCharacter(true);
            else if (moveInput < 0 && IsFacingRight())
                FlipCharacter(false);

            if (Mathf.Abs(moveInput) < 0.1f)
            {
                stateMachine.ChangeState(PlayerState.Idle);
                return;
            }

            if (playerView.JumpInput && IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.Jump);
                return;
            }

            if (!IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.Fall);
                return;
            }

            if (playerView.DodgeInput && stateMachine.CanDodge())
            {
                stateMachine.ChangeState(PlayerState.Dodge);
                return;
            }

            if (playerView.FlyInput && stateMachine.CanFly())
            {
                stateMachine.ChangeState(PlayerState.Fly);
                return;
            }

            if (playerView.KickInput && !playerModel.IsKickOnCooldown)
            {
                stateMachine.ChangeState(PlayerState.Kick);
                return;
            }

            if (playerView.FireInput && !playerModel.IsFireOnCooldown)
            {
                stateMachine.ChangeState(PlayerState.Fire);
                return;
            }

            if (playerModel.IsDead)
            {
                stateMachine.ChangeState(PlayerState.Dead);
                return;
            }
        }

        public override void OnStateExit() => playerView.UpdateRunAnimation(false);

        public override PlayerState GetStateType() => PlayerState.Run;
    }
}