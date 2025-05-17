using UnityEngine;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Sound.SoundUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class IdleState : PlayerStateBase
    {
        public IdleState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            playerView.StopPlayerMovement();
            playerView.UpdateRunAnimation(false);
            playerView.UpdateJumpAnimation(false);
            playerView.UpdateDodgeAnimation(false);
            playerView.UpdateFlightAnimation(false);
        }

        public override void OnStateExit() => playerView.ResetAllInputs();

        public override void Update()
        {
            base.Update();

            if (playerModel.IsDead) return;

            if (ShouldTransitionToMove())
            {
                stateMachine.ChangeState(PlayerState.Run);
                return;
            }

            if (CheckAbilityStateTransitions()) return;
        }

        private bool CheckAbilityStateTransitions()
        {
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

            if (CanTransformSuperSaiyan())
            {
                stateMachine.ChangeState(PlayerState.Transform);
                return true;
            }

            return false;
        }

        private void HandleJump()
        {
            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.y = playerModel.JumpSpeed;
            velocity.x *= playerModel.JumpHorizontalDampening;
            playerView.Rigidbody.linearVelocity = velocity;
            playerModel.JumpCount++;

            SoundManager.Instance.PlaySoundEffect(Sound.SoundData.SoundType.GokuJump);
            stateMachine.ChangeState(PlayerState.Jump);
            playerView.ResetJumpInput();
        }

        private void ToggleFlight()
        {
            bool wasFlying = playerModel.IsFlying;
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
            else if (wasFlying)
            {
                playerView.UpdateFlightAnimation(false);
                playerView.Rigidbody.gravityScale = 1f;
                playerView.StopFlightSound();
            }

            playerView.ResetFlyInput();
        }
    }
}