using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class DeadState : PlayerStateBase
    {
        public DeadState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter() { }

        public override void Update() { }

        public override void OnStateExit() => base.OnStateExit();
    }
}