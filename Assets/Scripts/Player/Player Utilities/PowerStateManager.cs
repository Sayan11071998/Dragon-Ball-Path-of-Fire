using System.Collections;
using DragonBall.Core;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;
using UnityEngine;

namespace DragonBall.Player.PlayerUtilities
{
    public class PowerStateManager
    {
        private PlayerController playerController;
        private PlayerModel playerModel;
        private PlayerView playerView;
        private PlayerPowerState currentPowerState;

        private bool isTransforming = false;

        public PlayerPowerState CurrentPowerState => currentPowerState;
        public bool IsTransforming => isTransforming;

        public PowerStateManager(PlayerController controller)
        {
            playerController = controller;
            playerModel = controller.PlayerModel;
            playerView = controller.PlayerView;
            currentPowerState = PlayerPowerState.Normal;
        }

        public void Update()
        {
            if (currentPowerState == PlayerPowerState.Normal &&
                !isTransforming &&
                playerModel.DragonBallCount >= playerModel.DragonBallsRequiredForSuperSaiyan)
            {
                TransformToSuperSaiyan();
            }
        }

        public void TransformToSuperSaiyan()
        {
            if (currentPowerState == PlayerPowerState.SuperSaiyan || isTransforming) return;

            isTransforming = true;
            playerController.DisablePlayerController();
            playerView.StopPlayerMovement();
            playerView.PlaySuperSaiyanTransformationAnimation();
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuSuperSaiyanTransform);

            playerView.StartCoroutine(WaitForTransformation());
        }

        private IEnumerator WaitForTransformation()
        {
            AnimationClip transformClip = playerView.SuperSaiyanAnimationClip;
            yield return new WaitForSeconds(transformClip.length * 0.8f);

            // Apply buffs and visual changes
            playerModel.ApplySuperSaiyanBuffs();
            playerView.TransformToSuperSaiyan();
            currentPowerState = PlayerPowerState.SuperSaiyan;

            yield return new WaitForSeconds(transformClip.length * 0.2f);

            playerView.StopPlayerMovement();
            yield return new WaitForSeconds(0.1f);

            bool isNotificationHandled = false;
            GameService.Instance.uiService.ShowNotification(() => isNotificationHandled = true);
            yield return new WaitUntil(() => isNotificationHandled);

            playerController.EnablePlayerController();
            isTransforming = false;
        }

        public void RevertToNormal()
        {
            if (currentPowerState == PlayerPowerState.Normal) return;

            // Apply normal state attributes
            playerModel.RemoveSuperSaiyanBuffs();
            playerView.RevertToNormal();
            currentPowerState = PlayerPowerState.Normal;
        }

        public bool CanFly() => currentPowerState == PlayerPowerState.SuperSaiyan;

        public bool CanVanish() => currentPowerState == PlayerPowerState.SuperSaiyan;

        public bool CanUseKamehameha() => currentPowerState == PlayerPowerState.SuperSaiyan;
    }
}