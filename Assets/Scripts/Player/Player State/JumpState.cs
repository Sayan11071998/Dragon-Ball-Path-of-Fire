using UnityEngine;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Sound.SoundUtilities;
using DragonBall.Sound.SoundData;

namespace DragonBall.Player.PlayerStates
{
    public class JumpState : BasePlayerState
    {
        public JumpState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            playerView.UpdateJumpAnimation(true);

            Vector2 velocity = GetCurrentVelocity();
            velocity.y = playerModel.JumpSpeed;
            velocity.x *= playerModel.JumpHorizontalDampening;
            SetVelocity(velocity);

            playerModel.JumpCount++;
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuJump);
            playerView.ResetJumpInput();
        }

        public override void Update()
        {
            float moveInput = playerView.MoveInput;
            Vector2 velocity = GetCurrentVelocity();
            velocity.x = moveInput * playerModel.MoveSpeed;
            SetVelocity(velocity);

            if (moveInput > 0 && !IsFacingRight())
                FlipCharacter(true);
            else if (moveInput < 0 && IsFacingRight())
                FlipCharacter(false);

            if (GetCurrentVelocity().y < 0)
            {
                stateMachine.ChangeState(PlayerState.Fall);
                return;
            }

            if (playerView.FlyInput && stateMachine.CanFly())
            {
                stateMachine.ChangeState(PlayerState.Fly);
                return;
            }

            if (playerView.VanishInput && stateMachine.CanVanish())
            {
                stateMachine.ChangeState(PlayerState.Vanish);
                return;
            }

            if (playerView.FireInput && !playerModel.IsFireOnCooldown)
            {
                stateMachine.ChangeState(PlayerState.Fire);
                return;
            }

            if (playerModel.IsDead)
            {
                stateMachine.ChangeState(PlayerState.Dead);
                return;
            }
        }

        public override void OnStateExit() => playerView.UpdateJumpAnimation(false);

        public override PlayerState GetStateType() => PlayerState.Jump;
    }
}