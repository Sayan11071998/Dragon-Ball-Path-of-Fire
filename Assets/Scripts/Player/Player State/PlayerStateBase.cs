using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Player.PlayerStates
{
    public abstract class PlayerStateBase : IState
    {
        protected readonly PlayerController playerController;
        protected readonly PlayerStateMachine stateMachine;
        protected readonly PlayerView playerView;
        protected readonly PlayerModel playerModel;

        protected PlayerStateBase(PlayerController controller, PlayerStateMachine machine)
        {
            playerController = controller;
            stateMachine = machine;
            playerView = controller.PlayerView;
            playerModel = controller.PlayerModel;
        }

        public virtual void OnStateEnter() { }

        public virtual void Update()
        {
            if (playerModel.IsDead) return;

            HandleGroundCheck();
            HandleCharacterDirection();
            playerModel.RegenerateStamina(Time.deltaTime);
        }

        public virtual void OnStateExit() { }


        protected void HandleGroundCheck()
        {
            playerModel.IsGrounded = playerView.IsTouchingGround();

            if (playerModel.IsGrounded)
                playerModel.JumpCount = 0;
        }

        protected void HandleCharacterDirection()
        {
            Vector2 moveDirection = playerView.MovementDirection;

            if (moveDirection.x > 0 && !playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = true;
                playerView.FlipCharacter(true);
            }
            else if (moveDirection.x < 0 && playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = false;
                playerView.FlipCharacter(false);
            }
        }

        protected bool ShouldTransitionToMove()
        {
            float moveInput = playerView.MoveInput;
            return Mathf.Abs(moveInput) > 0.1f;
        }

        protected bool ShouldTransitionToJump()
        {
            return playerView.JumpInput && playerModel.IsGrounded;
        }

        protected bool ShouldTransitionToDead()
        {
            return playerModel.IsDead;
        }

        protected bool CanUseKick()
        {
            return !playerModel.IsDead &&
                   playerModel.IsGrounded &&
                   playerView.KickInput &&
                   !playerModel.IsKickOnCooldown;
        }

        protected bool CanUseFire()
        {
            return !playerModel.IsDead &&
                   playerView.FireInput &&
                   !playerModel.IsFireOnCooldown;
        }

        protected bool CanUseVanish()
        {
            return !playerModel.IsDead &&
                   playerModel.IsSuperSaiyan() &&
                   playerView.VanishInput;
        }

        protected bool CanUseKamehameha()
        {
            return !playerModel.IsDead &&
                   playerModel.IsSuperSaiyan() &&
                   playerView.KamehamehaInput &&
                   playerModel.HasEnoughStaminaForKamehameha;
        }

        protected bool CanTransformSuperSaiyan()
        {
            return !playerModel.IsSuperSaiyan() &&
                   playerModel.DragonBallCount >= playerModel.DragonBallsRequiredForSuperSaiyan;
        }

        protected bool CanDodge()
        {
            return !playerModel.IsDead &&
                   playerModel.IsSuperSaiyan() &&
                   playerView.DodgeInput &&
                   playerModel.IsGrounded &&
                   Time.time > playerModel.LastDodgeTime + playerModel.DodgeCooldown;
        }

        protected bool CanToggleFlight()
        {
            return !playerModel.IsDead &&
                   playerModel.IsSuperSaiyan() &&
                   playerView.FlyInput;
        }
    }
}