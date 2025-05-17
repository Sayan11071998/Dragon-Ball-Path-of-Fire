using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class IdleState : BasePlayerState
    {
        public IdleState(PlayerController controller, PlayerStateMachine manager) : base(controller, manager) { }

        public override void OnStateEnter()
        {
            playerView.UpdateRunAnimation(false);
            playerView.UpdateJumpAnimation(false);
            playerView.UpdateFlightAnimation(false);
            playerView.UpdateDodgeAnimation(false);
        }

        public override void Update()
        {
            float moveInput = playerView.MoveInput;
            if (Mathf.Abs(moveInput) > 0.1f)
            {
                stateMachine.ChangeState(PlayerState.Run);
                return;
            }

            // Jump input check
            if (playerView.JumpInput && IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.Jump);
                return;
            }

            // Fly input check
            if (playerView.FlyInput && stateMachine.CanFly())
            {
                stateMachine.ChangeState(PlayerState.Fly);
                return;
            }

            // Dodge input check
            if (playerView.DodgeInput && IsGrounded() && stateMachine.CanDodge())
            {
                stateMachine.ChangeState(PlayerState.Dodge);
                return;
            }

            // Kick input check
            if (playerView.KickInput && IsGrounded() && !playerModel.IsKickOnCooldown)
            {
                stateMachine.ChangeState(PlayerState.Kick);
                return;
            }

            // Fire input check
            if (playerView.FireInput && !playerModel.IsFireOnCooldown)
            {
                stateMachine.ChangeState(PlayerState.Fire);
                return;
            }

            // Kamehameha input check
            if (playerView.KamehamehaInput && HasEnoughStaminaForKamehameha() && stateMachine.CanUseKamehameha())
            {
                stateMachine.ChangeState(PlayerState.Kamehameha);
                return;
            }

            // Vanish input check
            if (playerView.VanishInput && stateMachine.CanVanish())
            {
                stateMachine.ChangeState(PlayerState.Vanish);
                return;
            }

            // Not grounded check
            if (!IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.Fall);
                return;
            }

            // Death check
            if (playerModel.IsDead)
            {
                stateMachine.ChangeState(PlayerState.Dead);
                return;
            }
        }

        public override PlayerState GetStateType() => PlayerState.Idle;
    }
}