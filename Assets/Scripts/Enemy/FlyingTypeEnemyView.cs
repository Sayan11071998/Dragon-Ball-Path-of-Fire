using System.Collections;
using DragonBall.Core;
using DragonBall.Bullet;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class FlyingTypeEnemyView : BaseEnemyView
    {
        [Header("FlyingType specific Settings")]
        [SerializeField] private AnimationClip flyAnimation;
        [SerializeField] private AnimationClip fireAnimation;
        [SerializeField] private float hitTime = 0.4f;
        [SerializeField] private float floatAmplitude = 0.5f;
        [SerializeField] private float floatSpeed = 1.0f;

        [Header("Bullet Settings")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private BulletType bulletType = BulletType.EnemyGuidedPowerBall;

        protected virtual float FloatAmplitudeValue => floatAmplitude;
        protected virtual float FloatSpeedValue => floatSpeed;
        protected virtual BulletType EnemyBulletType => bulletType;
        protected virtual Transform FirePointTransform => firePoint;

        protected Coroutine attackCoroutine;
        protected Vector3 initialPosition;
        protected float floatTimer = 0f;

        protected override void Awake()
        {
            base.Awake();
            initialPosition = transform.position;
        }

        protected virtual void Update()
        {
            if (baseEnemyController != null && !baseEnemyController.IsDead && !isMoving && !isAttacking && !isDying)
                FloatInAir();
        }

        protected virtual void FloatInAir()
        {
            floatTimer += Time.deltaTime * FloatSpeedValue;
            float yOffset = Mathf.Sin(floatTimer) * FloatAmplitudeValue;
            Vector3 newPosition = transform.position;
            newPosition.y = initialPosition.y + yOffset;
            transform.position = newPosition;
        }

        public override void MoveInDirection(Vector2 direction, float speed)
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead || isDying))
            {
                StopMovement();
                return;
            }

            rb.linearVelocity = direction * speed;
            FaceDirection(direction.x);
            initialPosition = transform.position;
        }

        public override void SetMoving(bool moving)
        {
            if (isMoving == moving) return;

            isMoving = moving;
            animator.SetBool("isMoving", isMoving);

            if (!moving)
                initialPosition = transform.position;
        }

        public override void StartAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead)) return;
            if (isAttacking) return;

            isAttacking = true;
            animator.SetBool("isAttacking", true);
            attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        protected virtual IEnumerator AttackCoroutine()
        {
            float clipLength = fireAnimation != null ? fireAnimation.length : 0.6f;
            yield return new WaitForSeconds(hitTime);

            if (baseEnemyController != null && !baseEnemyController.IsPlayerDead && !baseEnemyController.IsDead)
                PerformAttack();

            yield return new WaitForSeconds(clipLength - hitTime);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
        }

        protected override void PerformAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead)) return;

            Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (playerTransform == null) return;

            Vector2 direction = ((Vector2)playerTransform.position - (Vector2)FirePointTransform.position).normalized;

            GameService.Instance.bulletService.FireBullet(
                EnemyBulletType,
                FirePointTransform.position,
                direction,
                BulletTargetType.Player
            );
        }

        public override void StartDeathAnimation()
        {
            if (isDying) return;

            if (isAttacking)
            {
                if (attackCoroutine != null)
                    StopCoroutine(attackCoroutine);

                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }

            isDying = true;
            animator.SetBool("isDead", true);

            float xDirection = spriteRenderer.flipX ? 1f : -1f;
            Vector2 flyAwayForce = new Vector2(flyAwayForceX * xDirection, flyAwayForceY * 0.5f);
            rb.AddForce(flyAwayForce, ForceMode2D.Impulse);

            StartCoroutine(DeathCoroutine());
        }

        public override void StopMovement()
        {
            base.StopMovement();
            initialPosition = transform.position;
        }
    }
}