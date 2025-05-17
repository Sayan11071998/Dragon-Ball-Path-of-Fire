using UnityEngine;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class FlyState : BasePlayerState
    {
        public FlyState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            playerModel.IsFlying = true;
            playerView.UpdateFlightAnimation(true);
            playerView.Rigidbody.gravityScale = 0f;
            playerView.StartFlightSound();
            playerView.ResetFlyInput();
        }

        public override void Update()
        {
            Vector2 moveDirection = playerView.MovementDirection;
            Vector2 velocity = GetCurrentVelocity();

            if (Mathf.Abs(moveDirection.x) > 0.1f)
                velocity.x = moveDirection.x * playerModel.MoveSpeed;
            else
                velocity.x = 0;

            if (Mathf.Abs(moveDirection.y) > 0.1f)
                velocity.y = moveDirection.y * playerModel.FlySpeed;
            else
                velocity.y = 0;

            SetVelocity(velocity);

            if (moveDirection.x > 0 && !IsFacingRight())
                FlipCharacter(true);
            else if (moveDirection.x < 0 && IsFacingRight())
                FlipCharacter(false);

            if (playerView.FlyInput)
            {
                stateMachine.ChangeState(PlayerState.Fall);
                return;
            }

            if (playerView.VanishInput && stateMachine.CanVanish())
            {
                stateMachine.ChangeState(PlayerState.Vanish);
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

            if (playerView.KamehamehaInput && HasEnoughStaminaForKamehameha() && stateMachine.CanUseKamehameha())
            {
                stateMachine.ChangeState(PlayerState.Kamehameha);
                return;
            }

            if (playerModel.IsDead)
            {
                stateMachine.ChangeState(PlayerState.Dead);
                return;
            }
        }

        public override void OnStateExit()
        {
            playerModel.IsFlying = false;
            playerView.UpdateFlightAnimation(false);
            playerView.Rigidbody.gravityScale = 1f;
            playerView.StopFlightSound();
            playerView.ResetMovementDirection();
        }

        public override PlayerState GetStateType() => PlayerState.Fly;
    }
}