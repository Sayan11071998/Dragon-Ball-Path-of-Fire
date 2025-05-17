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

            ApplyHorizontalMovement();
            CheckForDoubleJump();

            bool minJumpTimePassed = Time.time >= startJumpTime + minJumpDuration;

            if (playerModel.IsGrounded && minJumpTimePassed)
            {
                if (ShouldTransitionToMove())
                    stateMachine.ChangeState(PlayerState.Run);
                else
                    stateMachine.ChangeState(PlayerState.Idle);
            }
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
    }
}