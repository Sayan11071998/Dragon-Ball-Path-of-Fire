using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.Utilities;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerUtilities;

namespace DragonBall.Player.PlayerStates
{
    public abstract class BasePlayerState : IState
    {
        protected PlayerController playerController;
        protected PlayerModel playerModel;
        protected PlayerView playerView;
        protected PlayerStateMachine stateMachine;

        public BasePlayerState(PlayerController controller, PlayerStateMachine machine)
        {
            playerController = controller;
            playerModel = controller.PlayerModel;
            playerView = controller.PlayerView;
            stateMachine = machine;
        }

        public virtual void OnStateEnter() { }

        public virtual void Update() { }

        public virtual void OnStateExit() { }

        public abstract PlayerState GetStateType();

        protected bool IsGrounded() => playerModel.IsGrounded;

        protected bool HasEnoughStaminaForKamehameha() => playerModel.HasEnoughStaminaForKamehameha;

        protected Vector2 GetCurrentVelocity() => playerView.Rigidbody.linearVelocity;

        protected void SetVelocity(Vector2 velocity) => playerView.Rigidbody.linearVelocity = velocity;

        protected bool IsFacingRight() => playerModel.IsFacingRight;

        protected void FlipCharacter(bool facingRight)
        {
            if (playerModel.IsFacingRight != facingRight)
            {
                playerModel.IsFacingRight = facingRight;
                playerView.FlipCharacter(facingRight);
            }
        }
    }
}