using DragonBall.Bullet;
using DragonBall.Core;
using DragonBall.VFX;
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

        public override void Update()
        {
            HandleDodge();
            HandleVanish();
            HandleKick();
            HandleFire();
            HandleKamehameha();

            base.Update();
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting SUPER_SAIYAN state");
            playerModel.RemoveSuperSaiyanBuffs();
        }

        protected override void ResetUnhandledInputs() { }

        private void HandleVanish()
        {
            if (playerModel.IsDead || !playerView.VanishInput) return;

            Vector2 originalPosition = playerView.transform.position;
            Vector2 randomOffset = Random.insideUnitCircle * playerModel.VanishRange;
            if (randomOffset.y < 0)
                randomOffset.y = Mathf.Abs(randomOffset.y);

            GameService.Instance.vFXService.PlayVFXAtPosition(VFXType.VanishEffect, originalPosition);
            Vector2 newPosition = originalPosition + randomOffset;
            playerView.transform.position = new Vector3(newPosition.x, newPosition.y, playerView.transform.position.z);
            playerView.ResetVanishInput();
        }

        private void HandleDodge()
        {
            if (playerModel.IsDead)
            {
                playerView.ResetDodgeInput();
                return;
            }

            if (playerView.DodgeInput && playerModel.IsGrounded && Time.time > playerModel.LastDodgeTime + playerModel.DodgeCooldown)
            {
                playerModel.IsDodging = true;
                playerModel.DodgeEndTime = Time.time + playerModel.DodgeDuration;
                playerModel.LastDodgeTime = Time.time;
                Vector2 dir = playerModel.IsFacingRight ? Vector2.left : Vector2.right;
                playerView.Rigidbody.linearVelocity = new Vector2(dir.x * playerModel.DodgeSpeed, playerView.Rigidbody.linearVelocity.y);
                playerView.SetDodgeAnimation(true);
            }

            playerView.ResetDodgeInput();

            if (playerModel.IsDodging && Time.time > playerModel.DodgeEndTime)
            {
                playerModel.IsDodging = false;
                playerView.SetDodgeAnimation(false);
            }
        }

        protected override void FireBullet()
        {
            Vector2 position = playerView.FireTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            GameService.Instance.bulletService.FireBullet(BulletType.PlayerRegularPowerBall, position, direction);
        }

        private void HandleKamehameha()
        {
            if (playerModel.IsDead || !playerView.KamehamehaInput) return;

            AnimationClip kamehamehaClip = playerView.GetKamehamehaAnimationClip();
            playerView.PlayKamehamehaAnimation();
            playerView.StartFireCoroutine(kamehamehaClip.length, FireKamehameha);
            playerView.ResetKamehameha();
        }

        private void FireKamehameha()
        {
            Vector2 position = playerView.KamehamehaTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            GameService.Instance.bulletService.FireBullet(BulletType.PlayerKamehamehaPowerBall, position, direction);
        }
    }
}