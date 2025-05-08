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

        [Header("Health Regeneration Settings")]
        [SerializeField] private float regenerationDuration = 2.5f;
        [SerializeField] private Color regenerationColor = Color.green;
        [SerializeField] private float pulseFrequency = 2f;
        [SerializeField] private float pulseIntensity = 0.5f;

        private bool isRapidFiring = false;
        private bool isRegenerating = false;
        private Coroutine rapidFireCoroutine;
        private Coroutine regenerationCoroutine;
        private Color originalColor;

        protected override float FloatAmplitudeValue => bossFloatAmplitude;
        protected override float FloatSpeedValue => bossFloatSpeed;
        protected override BulletType EnemyBulletType => bossBulletType;

        protected override void Awake()
        {
            base.Awake();
            originalColor = spriteRenderer.color;
        }

        public override void StartAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead)) return;
            if (isAttacking || isRapidFiring || isRegenerating) return;

            if (Random.value < attackSelectionRandomness)
                StartRapidFireAttack();
            else
                base.StartAttack();
        }

        public void StartRapidFireAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead)) return;
            if (isAttacking || isRapidFiring || isRegenerating) return;

            CancelActiveCoroutines();

            isRapidFiring = true;
            animator.SetBool("isAttacking", true);
            rapidFireCoroutine = StartCoroutineTracked(RapidFireCoroutine());
        }

        public void StartRegenerationAnimation()
        {
            if (isRegenerating) return;

            CancelActiveCoroutines();

            isRegenerating = true;
            animator.SetBool("isRegenerating", true);
            // GameService.Instance.cameraShakeService.ShakeCamera(5f, 1f);

            regenerationCoroutine = StartCoroutineTracked(RegenerationCoroutine());
        }

        private IEnumerator RegenerationCoroutine()
        {
            bool wasMoving = isMoving;
            StopMovement();

            float elapsed = 0f;
            while (elapsed < regenerationDuration)
            {
                float pulseValue = Mathf.Sin(elapsed * pulseFrequency * Mathf.PI) * pulseIntensity + (1 - pulseIntensity);
                spriteRenderer.color = Color.Lerp(originalColor, regenerationColor, pulseValue);

                elapsed += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = originalColor;

            animator.SetBool("isRegenerating", false);
            isRegenerating = false;

            FinalBossTypeEnemyController finalBossController = baseEnemyController as FinalBossTypeEnemyController;
            finalBossController?.OnRegenerationAnimationComplete();

            if (wasMoving)
                SetMoving(true);
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

            Vector3 firePosition = FirePointTransform.position;

            if (spriteRenderer.flipX)
            {
                firePosition = new Vector3(
                    transform.position.x - Mathf.Abs(FirePointTransform.position.x - transform.position.x),
                    FirePointTransform.position.y,
                    FirePointTransform.position.z
                );
            }

            Vector2 baseDirection = ((Vector2)playerTransform.position - (Vector2)firePosition).normalized;
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
                    firePosition,
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

            if (isRegenerating && regenerationCoroutine != null)
                activeCoroutines.Add(regenerationCoroutine);
        }

        public override void ResetAllInputs()
        {
            base.ResetAllInputs();

            if (isRegenerating)
            {
                animator.SetBool("isRegenerating", false);
                isRegenerating = false;
                spriteRenderer.color = originalColor;
            }
        }
    }
}