using UnityEngine;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class RunState : PlayerStateBase
    {
        private float lastFootstepTime;
        private readonly float footstepInterval = 0.3f;

        public RunState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            playerView.UpdateRunAnimation(true);
            lastFootstepTime = Time.time;
        }

        public override void OnStateExit()
        {
            playerView.UpdateRunAnimation(false);
            base.OnStateExit();
        }

        public override void Update()
        {
            base.Update();

            if (playerModel.IsDead) return;

            float moveInput = playerView.MoveInput;
            Vector2 velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = moveInput * playerModel.MoveSpeed;
            playerView.Rigidbody.linearVelocity = velocity;

            if (playerModel.IsGrounded && Mathf.Abs(moveInput) > 0.1f && Time.time >= lastFootstepTime + footstepInterval)
            {
                lastFootstepTime = Time.time;
            }

            if (ShouldTransitionToIdle())
            {
                stateMachine.ChangeState(PlayerState.Idle);
                return;
            }

            if (CheckAbilityStateTransitions()) return;
        }

        private bool ShouldTransitionToIdle()
        {
            float moveInput = playerView.MoveInput;
            return Mathf.Abs(moveInput) < 0.1f;
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

            return false;
        }

        private void HandleJump()
        {
            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.y = playerModel.JumpSpeed;
            velocity.x = playerView.MoveInput * playerModel.MoveSpeed * playerModel.JumpHorizontalDampening;

            playerView.Rigidbody.linearVelocity = velocity;
            playerModel.JumpCount++;

            SoundManager.Instance.PlaySoundEffect(SoundType.GokuJump);
            stateMachine.ChangeState(PlayerState.Jump);
            playerView.ResetJumpInput();
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