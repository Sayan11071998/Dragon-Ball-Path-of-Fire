using UnityEngine;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Player.PlayerData;

namespace DragonBall.Player.PlayerStates
{
    public class KickState : PlayerStateBase
    {
        private bool kickAnimationComplete = false;
        private float kickStartTime;
        private readonly float kickAnimationDuration = 0.5f;

        public KickState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            kickAnimationComplete = false;
            kickStartTime = Time.time;
            playerView.PlayKickAnimation();
            playerController.PerformKickAttack();
            playerView.ResetKickInput();
        }

        public override void Update()
        {
            base.Update();

            if (!kickAnimationComplete && Time.time >= kickStartTime + kickAnimationDuration)
            {
                kickAnimationComplete = true;

                if (ShouldTransitionToMove())
                    stateMachine.ChangeState(PlayerState.Run);
                else
                    stateMachine.ChangeState(PlayerState.Idle);
            }
        }

        public override void OnStateExit() => base.OnStateExit();
    }
}