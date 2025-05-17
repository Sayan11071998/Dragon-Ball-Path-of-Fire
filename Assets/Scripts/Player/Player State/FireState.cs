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

        public FireState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            fireAnimationComplete = false;
            fireStartTime = Time.time;
            playerView.PlayFireAnimation();
            playerController.FireBullet();
            playerView.ResetFireInput();
        }

        public override void Update()
        {
            base.Update();

            if (!fireAnimationComplete && Time.time >= fireStartTime + fireAnimationDuration)
            {
                fireAnimationComplete = true;
                if (ShouldTransitionToMove())
                    stateMachine.ChangeState(PlayerState.Run);
                else
                    stateMachine.ChangeState(PlayerState.Idle);
            }
        }

        public override void OnStateExit() => base.OnStateExit();
    }
}