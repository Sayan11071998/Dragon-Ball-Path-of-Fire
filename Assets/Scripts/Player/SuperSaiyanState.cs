using UnityEngine;
using System.Collections;
using DragonBall.Core;
using DragonBall.Bullet;
using DragonBall.Sound;

namespace DragonBall.Player
{
    public class SuperSaiyanState : BasePlayerState
    {
        public SuperSaiyanState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

        public override void OnStateEnter()
        {
            playerController.DisablePlayerController();
            playerController.PlayerView.StopPlayerMovement();
            playerController.PlayerView.PlaySuperSaiyanTransformationAnimation();
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuSuperSaiyanTransformSFX);
            playerModel.ApplySuperSaiyanBuffs();
            playerController.PlayerView.StartCoroutine(WaitForSuperSaiyanTransformation());
        }

        private IEnumerator WaitForSuperSaiyanTransformation()
        {
            AnimationClip transformClip = playerController.PlayerView.SuperSaiyanAnimationClip;
            yield return new WaitForSeconds(transformClip.length * 0.8f);

            playerController.PlayerView.TransformToSuperSaiyan();

            yield return new WaitForSeconds(transformClip.length * 0.2f);

            playerController.PlayerView.StopPlayerMovement();
            yield return new WaitForSeconds(0.1f);

            bool isNotificationHandled = false;
            GameService.Instance.uiService.ShowNotification(() => isNotificationHandled = true);
            yield return new WaitUntil(() => isNotificationHandled);

            playerController.EnablePlayerController();
        }

        public override void HandleStateSpecificAbilities()
        {
            playerController.HandleFlight();
            HandleDodge();
            HandleVanish();
            HandleKamehameha();
            playerView.ResetJumpInput();
        }

        protected override BulletType GetBulletType() => BulletType.PlayerSuperSaiyanPowerBall;

        protected override void ResetUnusedInputs() { }

        public override void OnStateExit()
        {
            playerModel.RemoveSuperSaiyanBuffs();
            playerController.PlayerView.RevertToNormal();

            if (playerModel.IsFlying)
                playerView.StopFlightSound();
        }
    }
}