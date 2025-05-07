using System.Collections;
using DragonBall.Bullet;
using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class FinalBossTypeEnemyView : FlyingTypeEnemyView
    {
        [Header("Final Boss Specific Settings")]
        [SerializeField] private float bossFloatAmplitude = 0.7f;
        [SerializeField] private float bossFloatSpeed = 0.8f;
        [SerializeField] private BulletType bossBulletType = BulletType.EnemyGuidedPowerBall;

        [Header("Rapid Fire Attack Settings")]
        [SerializeField] private float rapidFireDuration = 3f;
        [SerializeField] private float bulletFireInterval = 0.2f;
        [SerializeField] private float spreadAngle = 15f;
        [SerializeField] private int bulletsPerSpread = 3;
        [SerializeField] private AnimationClip rapidFireAnimation;
        [SerializeField] private float attackSelectionRandomness = 0.3f;

        private bool isRapidFiring = false;
        private Coroutine rapidFireCoroutine;

        protected override float FloatAmplitudeValue => bossFloatAmplitude;
        protected override float FloatSpeedValue => bossFloatSpeed;
        protected override BulletType EnemyBulletType => bossBulletType;

        public override void StartAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead)) return;
            if (isAttacking || isRapidFiring) return;

            if (Random.value < attackSelectionRandomness)
                StartRapidFireAttack();
            else
                base.StartAttack();
        }

        public void StartRapidFireAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead)) return;
            if (isAttacking || isRapidFiring) return;

            CancelActiveCoroutines();

            isRapidFiring = true;
            animator.SetBool("isAttacking", true);
            rapidFireCoroutine = StartCoroutineTracked(RapidFireCoroutine());
        }

        private IEnumerator RapidFireCoroutine()
        {
            float elapsedTime = 0f;
            animator.SetBool("isAttacking", true);

            while (elapsedTime < rapidFireDuration)
            {
                if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead))
                    break;

                FireSpreadPattern();
                yield return new WaitForSeconds(bulletFireInterval);
                elapsedTime += bulletFireInterval;
            }

            animator.SetBool("isAttacking", false);
            isRapidFiring = false;

            baseEnemyController.BaseEnemyModel.lastAttackTime = Time.time;
        }

        private void FireSpreadPattern()
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (playerTransform == null) return;

            Vector2 baseDirection = ((Vector2)playerTransform.position - (Vector2)FirePointTransform.position).normalized;
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

            float angleStep = bulletsPerSpread > 1 ? spreadAngle / (bulletsPerSpread - 1) : 0;
            float startAngle = baseAngle - spreadAngle / 2;

            for (int i = 0; i < bulletsPerSpread; i++)
            {
                float currentAngle = startAngle + angleStep * i;
                float radianAngle = currentAngle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

                GameService.Instance.bulletService.FireBullet(
                    EnemyBulletType,
                    FirePointTransform.position,
                    direction,
                    BulletTargetType.Player
                );
            }
        }

        protected override void CancelActiveCoroutines()
        {
            base.CancelActiveCoroutines();

            if (isRapidFiring)
            {
                isRapidFiring = false;
                animator.SetBool("isAttacking", false);
            }
        }
    }
}