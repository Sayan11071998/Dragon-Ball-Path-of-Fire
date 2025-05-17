using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Player.PlayerData;

namespace DragonBall.Player.PlayerStates
{
    public class VanishState : PlayerStateBase
    {
        private bool wasFlying = false;

        public VanishState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            wasFlying = playerModel.IsFlying;
            playerView.ResetVanishInput();
            playerController.PerformVanish();

            if (wasFlying && playerModel.IsSuperSaiyan())
                stateMachine.ChangeState(PlayerState.Fly);
            else if (ShouldTransitionToMove())
                stateMachine.ChangeState(PlayerState.Run);
            else
                stateMachine.ChangeState(PlayerState.Idle);
        }

        public override void Update() => base.Update();

        public override void OnStateExit()
        {
            playerView.ResetVanishInput();
            base.OnStateExit();
        }
    }
}