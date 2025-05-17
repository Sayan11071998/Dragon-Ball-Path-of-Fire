using UnityEngine;
using DragonBall.Core;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;
using DragonBall.VFX;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Player.PlayerData;

namespace DragonBall.Player.PlayerMVC
{
    public class PlayerController
    {
        private PlayerModel playerModel;
        private PlayerView playerView;
        private PlayerStateMachine stateMachine;

        public PlayerModel PlayerModel => playerModel;
        public PlayerView PlayerView => playerView;
        public PlayerStateMachine StateMachine => stateMachine;

        private bool isInputEnabled = true;

        public PlayerController(PlayerModel _playerModel, PlayerView _playerView, bool startAsSuperSaiyan = false)
        {
            playerModel = _playerModel;
            playerView = _playerView;

            playerView.SetPlayerController(this);
            stateMachine = new PlayerStateMachine(this);

            if (startAsSuperSaiyan)
                stateMachine.ChangeState(PlayerState.Transform);
        }

        public void Update()
        {
            if (playerModel.IsDead)
            {
                if (stateMachine.GetCurrentPlayerState() != PlayerState.Dead)
                    stateMachine.ChangeState(PlayerState.Dead);
                return;
            }

            stateMachine.Update();
        }

        public void CollectDragonBall()
        {
            playerModel.IncrementDragonBallCount();
            SoundManager.Instance.PlaySoundEffect(SoundType.DragonBallCollect);
        }

        public void TakeDamage(float damage)
        {
            if (playerModel.IsDead) return;

            playerModel.TakeDamage(damage);
            GameService.Instance.cameraShakeService.ShakeCamera(0.1f, 0.5f);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuTakeDamage);

            if (playerModel.IsDead)
            {
                stateMachine.ChangeState(PlayerState.Dead);
                playerView.StartCoroutine(playerView.DeathSequence());
            }
        }

        public bool DisablePlayerController()
        {
            isInputEnabled = false;
            playerView.DisableInput();

            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = 0;
            playerView.Rigidbody.linearVelocity = velocity;

            return isInputEnabled;
        }

        public bool EnablePlayerController()
        {
            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = 0;
            playerView.Rigidbody.linearVelocity = velocity;

            isInputEnabled = true;
            playerView.EnableInput();
            return isInputEnabled;
        }

        public void RevertFromSuperSaiyan()
        {
            playerModel.RemoveSuperSaiyanBuffs();
            playerView.RevertToNormal();

            if (playerModel.IsFlying)
            {
                playerModel.IsFlying = false;
                playerView.StopFlightSound();
            }

            stateMachine.ChangeState(PlayerState.Idle);
        }

        public void PerformKickAttack()
        {
            Vector2 origin = playerView.AttackTransform.position;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, playerModel.KickAttackRange, Vector2.zero, 0f);

            if (!playerModel.IsFlying)
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuKick);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<DragonBall.Enemy.EnemyUtilities.IDamageable>(out var target))
                    target.Damage(playerModel.KickAttackPower);
            }

            playerModel.LastKickTime = Time.time;
        }

        public void FireBullet()
        {
            Vector2 position = playerView.FireTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            DragonBall.Bullet.BulletData.BulletType bulletType = playerModel.IsSuperSaiyan() ?
                DragonBall.Bullet.BulletData.BulletType.PlayerSuperSaiyanPowerBall :
                DragonBall.Bullet.BulletData.BulletType.PlayerNormalPowerBall;

            GameService.Instance.bulletService.FireBullet(bulletType, position, direction);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuFire);

            playerModel.LastFireTime = Time.time;
        }

        public void FireKamehameha()
        {
            Vector2 position = playerView.KamehamehaTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            GameService.Instance.bulletService.FireBullet(
                DragonBall.Bullet.BulletData.BulletType.PlayerKamehamehaPowerBall,
                position,
                direction
            );
        }

        public void PerformVanish()
        {
            Vector2 originalPosition = playerView.transform.position;
            Vector2 randomOffset = Random.insideUnitCircle * playerModel.VanishRange;

            if (randomOffset.y < 0)
                randomOffset.y = Mathf.Abs(randomOffset.y);

            GameService.Instance.vFXService.PlayVFXAtPosition(VFXType.VanishEffect, originalPosition);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuVanish);
            Vector2 newPosition = originalPosition + randomOffset;
            playerView.transform.position = new Vector3(newPosition.x, newPosition.y, playerView.transform.position.z);
        }
    }
}