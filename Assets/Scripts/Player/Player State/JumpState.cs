using UnityEngine;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;
using DragonBall.Core;
using DragonBall.VFX;

namespace DragonBall.Player.PlayerStates
{
    public class JumpState : PlayerStateBase
    {
        private float startJumpTime;
        private readonly float minJumpDuration = 0.1f;
        private bool hasDoubleJumped = false;

        public JumpState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            playerView.UpdateJumpAnimation(true);
            startJumpTime = Time.time;
            hasDoubleJumped = playerModel.JumpCount > 1;
        }

        public override void OnStateExit()
        {
            playerView.UpdateJumpAnimation(false);
            playerView.ResetJumpInput();
            base.OnStateExit();
        }

        public override void Update()
        {
            base.Update();

            if (playerModel.IsDead) return;

            ApplyHorizontalMovement();
            CheckForDoubleJump();

            if (CheckStateTransitions()) return;
        }

        private void ApplyHorizontalMovement()
        {
            float moveInput = playerView.MoveInput;
            Vector2 velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = moveInput * playerModel.MoveSpeed;
            playerView.Rigidbody.linearVelocity = velocity;
        }

        private void CheckForDoubleJump()
        {
            if (!hasDoubleJumped && !playerModel.IsGrounded && playerView.JumpInput && playerModel.JumpCount < 2)
            {
                PerformDoubleJump();
                hasDoubleJumped = true;
            }
        }

        private void PerformDoubleJump()
        {
            Vector2 velocity = playerView.Rigidbody.linearVelocity;
            velocity.y = playerModel.JumpSpeed;
            velocity.x *= playerModel.JumpHorizontalDampening;
            playerView.Rigidbody.linearVelocity = velocity;

            playerModel.JumpCount++;

            GameService.Instance.vFXService.PlayVFXAtPosition(VFXType.JumpEffect, playerView.transform.position);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuJump);

            playerView.ResetJumpInput();
        }

        private bool CheckStateTransitions()
        {
            bool minJumpTimePassed = Time.time >= startJumpTime + minJumpDuration;

            if (playerModel.IsGrounded && minJumpTimePassed)
            {
                if (ShouldTransitionToMove())
                {
                    stateMachine.ChangeState(PlayerState.Run);
                    return true;
                }
                else
                {
                    stateMachine.ChangeState(PlayerState.Idle);
                    return true;
                }
            }

            if (CheckAbilityStateTransitions()) return true;

            return false;
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

                if (CanToggleFlight())
                {
                    ToggleFlight();
                    return true;
                }
            }

            if (CanUseFire())
            {
                stateMachine.ChangeState(PlayerState.Fire);
                return true;
            }

            return false;
        }

        private void ToggleFlight()
        {
            playerModel.IsFlying = true;
            playerView.Rigidbody.linearVelocity = Vector2.zero;
            playerView.ResetMovementDirection();
            playerView.UpdateFlightAnimation(true);
            playerView.Rigidbody.gravityScale = 0f;
            playerView.StartFlightSound();
            stateMachine.ChangeState(PlayerState.Fly);
            playerView.ResetFlyInput();
        }
    }
}