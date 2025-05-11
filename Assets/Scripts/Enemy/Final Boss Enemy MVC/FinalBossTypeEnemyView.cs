using UnityEngine;
using System.Collections;
using DragonBall.Bullet.BulletData;
using DragonBall.Core;
using DragonBall.Enemy.FlyingEnemyMVC;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;
using DragonBall.GameStrings;

namespace DragonBall.Enemy.FinalBossEnemyMVC
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
        [SerializeField] private AnimationClip healthRegenarationClip;
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

        public override void MoveInDirection(Vector2 direction, float speed)
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead || isDying || isRegenerating))
            {
                StopMovement();
                return;
            }

            rb.linearVelocity = direction * speed;
            FaceDirection(direction.x);
            initialPosition = transform.position;
        }

        protected override void FloatInAir()
        {
            if (isRegenerating) return;

            floatTimer += Time.deltaTime * FloatSpeedValue;
            float yOffset = Mathf.Sin(floatTimer) * FloatAmplitudeValue;
            Vector3 newPosition = transform.position;
            newPosition.y = initialPosition.y + yOffset;
            transform.position = newPosition;
        }

        public override void StartAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead || isRegenerating)) return;
            if (isAttacking || isRapidFiring) return;

            if (Random.value < attackSelectionRandomness)
                StartRapidFireAttack();
            else
                base.StartAttack();
        }

        public void StartRapidFireAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead || isRegenerating)) return;
            if (isAttacking || isRapidFiring) return;

            CancelActiveCoroutines();

            isRapidFiring = true;
            animator.SetBool(GameString.EnemyAnimatorAttackBool, true);
            SoundManager.Instance.PlaySoundEffect(SoundType.FinalBossTypeEnemyFire, isRapidFiring);

            rapidFireCoroutine = StartCoroutineTracked(RapidFireCoroutine());
        }

        public void StartRegenerationAnimation()
        {
            if (isRegenerating) return;

            CancelActiveCoroutines();

            isRegenerating = true;
            animator.SetBool(GameString.EnemyAnimatorHealthRegenerationBool, true);
            GameService.Instance.cameraShakeService.ShakeCamera(0.1f, healthRegenarationClip.length);

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

            animator.SetBool(GameString.EnemyAnimatorHealthRegenerationBool, false);
            isRegenerating = false;

            FinalBossTypeEnemyController finalBossController = baseEnemyController as FinalBossTypeEnemyController;
            finalBossController?.OnRegenerationAnimationComplete();

            if (wasMoving)
                SetMoving(true);
        }

        private IEnumerator RapidFireCoroutine()
        {
            float elapsedTime = 0f;
            animator.SetBool(GameString.EnemyAnimatorAttackBool, true);

            while (elapsedTime < rapidFireDuration)
            {
                if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead))
                    break;

                FireSpreadPattern();
                yield return new WaitForSeconds(bulletFireInterval);
                elapsedTime += bulletFireInterval;
            }

            animator.SetBool(GameString.EnemyAnimatorAttackBool, false);
            isRapidFiring = false;
            SoundManager.Instance.StopSoundEffect(SoundType.FinalBossTypeEnemyFire);

            baseEnemyController.BaseEnemyModel.lastAttackTime = Time.time;
        }

        private void FireSpreadPattern()
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag(GameString.PlayerTag)?.transform;

            if (playerTransform == null) return;

            Vector3 firePosition = GetAdjustedFirePosition();
            Vector2 baseDirection = ((Vector2)playerTransform.position - (Vector2)firePosition).normalized;
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
            FireBulletsInSpread(firePosition, baseAngle);
        }

        private Vector3 GetAdjustedFirePosition()
        {
            Vector3 firePosition = FirePointTransform.position;

            if (spriteRenderer.flipX)
            {
                firePosition = new Vector3(
                    transform.position.x - Mathf.Abs(FirePointTransform.position.x - transform.position.x),
                    FirePointTransform.position.y,
                    FirePointTransform.position.z
                );
            }

            return firePosition;
        }

        private void FireBulletsInSpread(Vector3 firePosition, float baseAngle)
        {
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
                animator.SetBool(GameString.EnemyAnimatorAttackBool, false);
                SoundManager.Instance.StopSoundEffect(SoundType.FinalBossTypeEnemyFire);
            }

            if (isRegenerating && regenerationCoroutine != null)
                activeCoroutines.Add(regenerationCoroutine);
        }

        public override void ResetAllInputs()
        {
            base.ResetAllInputs();

            if (isRegenerating)
            {
                animator.SetBool(GameString.EnemyAnimatorHealthRegenerationBool, false);
                isRegenerating = false;
                spriteRenderer.color = originalColor;
            }

            if (isRapidFiring)
            {
                isRapidFiring = false;
                animator.SetBool(GameString.EnemyAnimatorAttackBool, false);
                SoundManager.Instance.StopSoundEffect(SoundType.FinalBossTypeEnemyFire);
            }
        }
    }
}