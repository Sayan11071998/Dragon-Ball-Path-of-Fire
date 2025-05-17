using UnityEngine;
using System.Collections;
using DragonBall.Core;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;

namespace DragonBall.Player.PlayerStates
{
    public class TransformState : PlayerStateBase
    {
        private Coroutine transformationCoroutine;

        public TransformState(PlayerController controller, PlayerStateMachine machine) : base(controller, machine) { }

        public override void OnStateEnter()
        {
            playerController.DisablePlayerController();
            playerView.StopPlayerMovement();
            playerView.PlaySuperSaiyanTransformationAnimation();
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuSuperSaiyanTransform);
            playerModel.ApplySuperSaiyanBuffs();
            transformationCoroutine = playerView.StartCoroutine(WaitForSuperSaiyanTransformation());
        }

        private IEnumerator WaitForSuperSaiyanTransformation()
        {
            AnimationClip transformClip = playerView.SuperSaiyanAnimationClip;

            playerView.TransformToSuperSaiyan();
            yield return new WaitForSeconds(transformClip.length * 0.8f);

            yield return new WaitForSeconds(transformClip.length * 0.2f);

            playerView.StopPlayerMovement();
            yield return new WaitForSeconds(0.1f);

            bool isNotificationHandled = false;
            GameService.Instance.uiService.ShowNotification(() => isNotificationHandled = true);
            yield return new WaitUntil(() => isNotificationHandled);

            playerController.EnablePlayerController();
            stateMachine.ChangeState(PlayerState.Idle);
        }

        public override void Update()
        {
            base.Update();

            if (playerModel.IsDead)
                stateMachine.ChangeState(PlayerState.Dead);
        }

        public override void OnStateExit()
        {
            if (transformationCoroutine != null)
            {
                playerView.StopCoroutine(transformationCoroutine);
                transformationCoroutine = null;
            }

            playerController.EnablePlayerController();
        }
    }
}