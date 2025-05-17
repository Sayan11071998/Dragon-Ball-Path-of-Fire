using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Player.PlayerData;

namespace DragonBall.Player.PlayerStates
{
    public class FireState : PlayerStateBase
    {
        private bool fireAnimationComplete = false;
        private float fireStartTime;
        private readonly float fireAnimationDuration = 0.5f;
        private bool wasFlying = false;

        public FireState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            fireAnimationComplete = false;
            fireStartTime = Time.time;
            wasFlying = playerModel.IsFlying;
            playerView.PlayFireAnimation();
            playerController.FireBullet();
            playerView.ResetFireInput();
        }

        public override void Update()
        {
            if (playerModel.IsDead)
            {
                stateMachine.ChangeState(PlayerState.Dead);
                return;
            }

            HandleGroundCheck();
            HandleCharacterDirection();
            playerModel.RegenerateStamina(Time.deltaTime);

            if (!fireAnimationComplete && Time.time >= fireStartTime + fireAnimationDuration)
            {
                fireAnimationComplete = true;
                HandleStateTransition();
            }
        }

        private void HandleStateTransition()
        {
            if (wasFlying)
            {
                playerModel.IsFlying = true;
                stateMachine.ChangeState(PlayerState.Fly);
            }
            else if (ShouldTransitionToMove())
            {
                stateMachine.ChangeState(PlayerState.Run);
            }
            else
            {
                stateMachine.ChangeState(PlayerState.Idle);
            }
        }

        public override void OnStateExit() { }
    }
}