using UnityEngine;

namespace DragonBall.Player
{
    public class NormalState : BasePlayerState
    {
        public NormalState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter()
        {
            Debug.Log("Entering NORMAL state");
        }

        public override void Update()
        {
            if (playerModel.DragonBallCount >= playerModel.DragonBallsRequiredForSuperSaiyan)
            {
                playerStateMachine.ChangeState(PlayerState.SUPER_SAIYAN);
                return;
            }

            HandleKick();
            HandleFire();

            base.Update();
        }

        public override void OnStateExit() => Debug.Log("Exiting NORMAL state");

        protected override void ResetUnhandledInputs()
        {
            playerView.ResetVanishInput();
            playerView.ResetDodgeInput();
            playerView.ResetKamehameha();
        }
    }
}