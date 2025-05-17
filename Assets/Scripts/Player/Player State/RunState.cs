using UnityEngine;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;

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

            float moveInput = playerView.MoveInput;
            Vector2 velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = moveInput * playerModel.MoveSpeed;
            playerView.Rigidbody.linearVelocity = velocity;

            if (playerModel.IsGrounded && Mathf.Abs(moveInput) > 0.1f && Time.time >= lastFootstepTime + footstepInterval)
                lastFootstepTime = Time.time;

            if (Mathf.Abs(moveInput) < 0.1f)
                stateMachine.ChangeState(PlayerState.Idle);
        }
    }
}