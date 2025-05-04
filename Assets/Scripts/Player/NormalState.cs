namespace DragonBall.Player
{
    public class NormalState : BasePlayerState
    {
        public NormalState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter() { }

        public override void HandleStateSpecificAbilities()
        {
            if (playerModel.DragonBallCount >= playerModel.DragonBallsRequiredForSuperSaiyan)
            {
                playerStateMachine.ChangeState(PlayerState.SUPER_SAIYAN);
                return;
            }

            ResetUnusedInputs();
        }

        protected override void ResetUnusedInputs()
        {
            playerView.ResetVanishInput();
            playerView.ResetDodgeInput();
            playerView.ResetKamehameha();
        }

        public override void OnStateExit() { }
    }
}