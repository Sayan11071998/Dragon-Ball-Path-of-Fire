using UnityEngine;

namespace DragonBall.Player
{
    public class SuperSaiyanState : BasePlayerState
    {
        public SuperSaiyanState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter()
        {
            Debug.Log("Entering SUPER_SAIYAN state");
            playerModel.ApplySuperSaiyanBuffs();
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting SUPER_SAIYAN state");
            playerModel.RemoveSuperSaiyanBuffs();
        }

        protected override void HandleStateSpecificAbilities()
        {
            HandleDodge();
            HandleVanish();
            HandleKamehameha();
        }

        protected override void ResetUnusedInputs() { }
    }
}