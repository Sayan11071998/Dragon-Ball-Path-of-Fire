using UnityEngine;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Utilities;
using DragonBall.Sound.SoundUtilities;
using DragonBall.Sound.SoundData;

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

            if (CheckCommonTransitions()) return;
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

        protected bool CheckCommonTransitions()
        {
            if (playerModel.IsDead)
            {
                stateMachine.ChangeState(PlayerState.Dead);
                return true;
            }

            if (CanTransformSuperSaiyan())
            {
                stateMachine.ChangeState(PlayerState.Transform);
                return true;
            }

            if (playerModel.IsSuperSaiyan())
            {
                if (CanUseKamehameha())
                {
                    stateMachine.ChangeState(PlayerState.Kamehameha);
                    playerView.ResetKamehameha();
                    return true;
                }

                if (CanUseVanish())
                {
                    stateMachine.ChangeState(PlayerState.Vanish);
                    playerView.ResetVanishInput();
                    return true;
                }

                if (CanDodge())
                {
                    stateMachine.ChangeState(PlayerState.Dodge);
                    playerView.ResetDodgeInput();
                    return true;
                }

                if (CanToggleFlight())
                {
                    ToggleFlight();
                    return true;
                }
            }

            if (ShouldTransitionToJump())
            {
                HandleJump();
                return true;
            }

            if (CanUseKick())
            {
                stateMachine.ChangeState(PlayerState.Kick);
                return true;
            }

            if (CanUseFire())
            {
                stateMachine.ChangeState(PlayerState.Fire);
                return true;
            }

            return false;
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

        protected void HandleJump()
        {
            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.y = playerModel.JumpSpeed;
            velocity.x *= playerModel.JumpHorizontalDampening;
            playerView.Rigidbody.linearVelocity = velocity;
            playerModel.JumpCount++;

            SoundManager.Instance.PlaySoundEffect(SoundType.GokuJump);
            stateMachine.ChangeState(PlayerState.Jump);
            playerView.ResetJumpInput();
        }

        protected void ToggleFlight()
        {
            playerModel.IsFlying = !playerModel.IsFlying;
            playerView.Rigidbody.linearVelocity = Vector2.zero;
            playerView.ResetMovementDirection();

            if (playerModel.IsFlying)
            {
                playerView.UpdateFlightAnimation(true);
                playerView.Rigidbody.gravityScale = 0f;
                playerView.StartFlightSound();
                stateMachine.ChangeState(PlayerState.Fly);
            }
            else
            {
                playerView.UpdateFlightAnimation(false);
                playerView.Rigidbody.gravityScale = 1f;
                playerView.StopFlightSound();
                stateMachine.ChangeState(PlayerState.Idle);
            }

            playerView.ResetFlyInput();
        }
    }
}