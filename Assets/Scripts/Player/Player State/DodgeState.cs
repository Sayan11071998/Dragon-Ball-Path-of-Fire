using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Player.PlayerData;

namespace DragonBall.Player.PlayerStates
{
    public class DodgeState : PlayerStateBase
    {
        private float dodgeStartTime;
        private float dodgeDuration = 0.2f;
        private float dodgeSpeed = 10f;
        private Vector2 dodgeDirection;

        public DodgeState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            dodgeStartTime = Time.time;
            dodgeDirection = playerModel.IsFacingRight ? Vector2.left : Vector2.right;
            playerView.UpdateDodgeAnimation(true);
            playerModel.LastDodgeTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (Time.time < dodgeStartTime + dodgeDuration)
            {
                Vector2 velocity = playerView.Rigidbody.linearVelocity;
                velocity.x = dodgeDirection.x * dodgeSpeed;
                velocity.y = 0;
                playerView.Rigidbody.linearVelocity = velocity;
            }
            else
            {
                if (ShouldTransitionToMove())
                    stateMachine.ChangeState(PlayerState.Run);
                else
                    stateMachine.ChangeState(PlayerState.Idle);
            }
        }

        public override void OnStateExit()
        {
            playerView.UpdateDodgeAnimation(false);
            Vector2 velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = 0;
            playerView.Rigidbody.linearVelocity = velocity;
        }
    }
}