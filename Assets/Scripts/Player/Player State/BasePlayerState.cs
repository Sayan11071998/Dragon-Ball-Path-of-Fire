using DragonBall.Bullet.BulletData;
using DragonBall.Core;
using DragonBall.Enemy.EnemyUtilities;
using DragonBall.Player.PlayerMVC;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;
using DragonBall.Utilities;
using DragonBall.VFX;
using UnityEngine;

namespace DragonBall.Player.PlayerStates
{
    public abstract class BasePlayerState : IState
    {
        protected PlayerController playerController;
        protected PlayerStateMachine playerStateMachine;
        protected PlayerModel playerModel;
        protected PlayerView playerView;

        public BasePlayerState(PlayerController controllerToSet, PlayerStateMachine stateMachineToSet)
        {
            playerController = controllerToSet;
            playerStateMachine = stateMachineToSet;
            playerModel = controllerToSet.PlayerModel;
            playerView = controllerToSet.PlayerView;
        }

        public abstract void OnStateEnter();

        public virtual void Update()
        {
            if (playerModel.IsDead) return;

            playerModel.RegenerateStamina(Time.deltaTime);

            HandleBasicAbilities();
            HandleStateSpecificAbilities();
            UpdateAnimations(playerView.MoveInput);
        }

        protected virtual void HandleBasicAbilities()
        {
            HandleKick();
            HandleFire();
        }

        public abstract void HandleStateSpecificAbilities();

        protected void UpdateAnimations(float moveInput)
        {
            playerView.UpdateRunAnimation(Mathf.Abs(moveInput) > 0.1f && !playerModel.IsDodging);
            playerView.UpdateJumpAnimation(!playerModel.IsGrounded);
            playerView.UpdateDodgeAnimation(playerModel.IsDodging);
        }

        protected void HandleKick()
        {
            if (playerModel.IsDead || !playerView.KickInput) return;

            if (playerModel.IsKickOnCooldown)
            {
                playerView.ResetKickInput();
                return;
            }

            playerModel.LastKickTime = Time.time;
            playerView.PlayKickAnimation();
            PerformKickAttack();
            playerView.ResetKickInput();
        }

        protected void PerformKickAttack()
        {
            Vector2 origin = playerView.AttackTransform.position;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, playerModel.KickAttackRange, Vector2.zero, 0f);

            if (!playerModel.IsFlying)
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuKick);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var target))
                    target.Damage(playerModel.KickAttackPower);
            }
        }

        protected void HandleFire()
        {
            if (playerModel.IsDead || !playerView.FireInput) return;

            if (playerModel.IsFireOnCooldown)
            {
                playerView.ResetFireInput();
                return;
            }

            playerModel.LastFireTime = Time.time;
            playerView.PlayFireAnimation();
            FireBullet();
            playerView.ResetFireInput();
        }

        protected virtual void FireBullet()
        {
            Vector2 position = playerView.FireTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            GameService.Instance.bulletService.FireBullet(GetBulletType(), position, direction);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuFire);
        }

        protected virtual BulletType GetBulletType() => BulletType.PlayerNormalPowerBall;

        protected void HandleDodge()
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
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuDodge);
                playerView.UpdateDodgeAnimation(true);
            }

            playerView.ResetDodgeInput();

            if (playerModel.IsDodging && Time.time > playerModel.DodgeEndTime)
            {
                playerModel.IsDodging = false;
                playerView.UpdateDodgeAnimation(false);
            }
        }

        protected void HandleVanish()
        {
            if (playerModel.IsDead || !playerView.VanishInput) return;

            Vector2 originalPosition = playerView.transform.position;
            Vector2 randomOffset = Random.insideUnitCircle * playerModel.VanishRange;

            if (randomOffset.y < 0)
                randomOffset.y = Mathf.Abs(randomOffset.y);

            GameService.Instance.vFXService.PlayVFXAtPosition(VFXType.VanishEffect, originalPosition);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuVanish);
            Vector2 newPosition = originalPosition + randomOffset;
            playerView.transform.position = new Vector3(newPosition.x, newPosition.y, playerView.transform.position.z);
            playerView.ResetVanishInput();
        }

        protected void HandleKamehameha()
        {
            if (playerModel.IsDead || !playerView.KamehamehaInput) return;

            if (!playerModel.HasEnoughStaminaForKamehameha)
            {
                playerView.ResetKamehameha();
                return;
            }

            if (playerModel.UseStaminaForKamehameha())
            {
                AnimationClip kamehamehaClip = playerView.KamehamehaAnimationClip;
                playerView.PlayKamehamehaAnimation();
                SoundManager.Instance.PlaySoundEffect(SoundType.Kamekameha);
                playerView.StartFireCoroutine(kamehamehaClip.length, FireKamehameha);
            }

            playerView.ResetKamehameha();
        }

        protected void FireKamehameha()
        {
            Vector2 position = playerView.KamehamehaTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            GameService.Instance.bulletService.FireBullet(BulletType.PlayerKamehamehaPowerBall, position, direction);
        }

        protected virtual void ResetUnusedInputs() { }

        public abstract void OnStateExit();
    }
}