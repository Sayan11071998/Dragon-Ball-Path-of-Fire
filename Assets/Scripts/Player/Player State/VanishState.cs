using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Player.PlayerData;

namespace DragonBall.Player.PlayerStates
{
    public class VanishState : PlayerStateBase
    {
        public VanishState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            playerController.PerformVanish();
            if (ShouldTransitionToMove())
                stateMachine.ChangeState(PlayerState.Run);
            else
                stateMachine.ChangeState(PlayerState.Idle);
        }

        public override void Update() => base.Update();

        public override void OnStateExit() => base.OnStateExit();
    }
}