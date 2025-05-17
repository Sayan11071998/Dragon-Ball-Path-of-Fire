using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class IdleState : PlayerStateBase
    {
        public IdleState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            playerView.StopPlayerMovement();
            playerView.UpdateRunAnimation(false);
            playerView.UpdateJumpAnimation(false);
            playerView.UpdateDodgeAnimation(false);
            playerView.UpdateFlightAnimation(false);
        }

        public override void OnStateExit() => playerView.ResetAllInputs();

        public override void Update()
        {
            base.Update();

            if (ShouldTransitionToMove())
                stateMachine.ChangeState(PlayerState.Run);
        }
    }
}