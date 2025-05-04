using UnityEngine;
using System.Collections;
using DragonBall.Core;

namespace DragonBall.Player
{
    public class SuperSaiyanState : BasePlayerState
    {
        public SuperSaiyanState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter()
        {
            playerController.DisablePlayerController();
            playerController.PlayerView.StopPlayerMovement();
            playerModel.ApplySuperSaiyanBuffs();
            playerController.PlayerView.PlaySuperSaiyanTransformationAnimation();
            playerController.PlayerView.StartCoroutine(WaitForSuperSaiyanTransformation());
        }

        private IEnumerator WaitForSuperSaiyanTransformation()
        {
            AnimationClip transformClip = playerController.PlayerView.SuperSaiyanAnimationClip;
            yield return new WaitForSeconds(transformClip.length);

            playerController.PlayerView.StopPlayerMovement();
            yield return new WaitForSeconds(0.1f);

            bool isNotificationHandled = false;
            GameService.Instance.uiService.ShowNotification(() => isNotificationHandled = true);
            yield return new WaitUntil(() => isNotificationHandled);

            playerController.EnablePlayerController();
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