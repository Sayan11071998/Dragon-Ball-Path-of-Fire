using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class FallState : BasePlayerState
    {
        public FallState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter() => playerView.UpdateJumpAnimation(true);

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

            if (IsGrounded())
            {
                stateMachine.ChangeState(Mathf.Abs(moveInput) > 0.1f ? PlayerState.Run : PlayerState.Idle);
                return;
            }

            if (playerView.JumpInput && playerModel.JumpCount < 1)
            {
                Vector2 doubleJumpVelocity = GetCurrentVelocity();
                doubleJumpVelocity.y = playerModel.JumpSpeed;
                doubleJumpVelocity.x *= playerModel.JumpHorizontalDampening;
                SetVelocity(doubleJumpVelocity);

                playerModel.JumpCount++;
                return;
            }

            if (playerView.FlyInput && stateMachine.CanFly())
            {
                stateMachine.ChangeState(PlayerState.Fly);
                return;
            }

            if (playerView.VanishInput && stateMachine.CanVanish())
            {
                stateMachine.ChangeState(PlayerState.Vanish);
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

        public override void OnStateExit() => playerView.UpdateJumpAnimation(false);

        public override PlayerState GetStateType() => PlayerState.Fall;
    }
}