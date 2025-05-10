using System.Collections;
using System.Collections.Generic;
using DragonBall.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DragonBall.Enemy
{
    public abstract class BaseEnemyView : MonoBehaviour, IDamageable
    {
        [Header("Base Attack Settings")]
        [SerializeField] protected AnimationClip enemyDeathClip;
        [SerializeField] protected float attackRadius = 0.5f;
        [SerializeField] protected Vector2 attackOffset = new Vector2(0.5f, 0f);
        [SerializeField] protected float deathClipDurationOffset = 0.5f;
        [SerializeField] protected float attackHitTime = 0.3f;

        [Header("Death Fly Away Settings")]
        [SerializeField] protected float flyAwayForceX = 5f;
        [SerializeField] protected float flyAwayForceY = 2f;
        [SerializeField] protected float flyAwayDuration = 0.3f;

        [Header("Base Health Bar UI Settings")]
        [SerializeField] private Slider healthBar;

        protected BaseEnemyController baseEnemyController;
        protected Rigidbody2D rb;
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;

        protected bool isMoving = false;
        protected bool isAttacking = false;
        protected bool isDying = false;
        protected Coroutine attackCoroutine;
        protected List<Coroutine> activeCoroutines = new List<Coroutine>();

        public bool IsAttacking => isAttacking;
        public bool IsDying => isDying;

        public virtual void SetController(BaseEnemyController controllerToSet) => baseEnemyController = controllerToSet;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            animator.SetBool("isDead", false);
        }

        protected virtual void FixedUpdate() => baseEnemyController?.Update();

        public virtual void Damage(float damageValue)
        {
            GameService.Instance.vFXService.PlayVFXAtPosition(VFX.VFXType.Explosion, transform.position);
            baseEnemyController?.TakeDamage(damageValue);
        }

        public virtual void SetMoving(bool moving)
        {
            if (isMoving == moving) return;
            isMoving = moving;
            animator.SetBool("isMoving", isMoving);
        }

        public virtual void MoveInDirection(Vector2 direction, float speed)
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead || isDying))
            {
                StopMovement();
                return;
            }

            float velocityX = direction.x * speed;
            rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);
            FaceDirection(direction.x);
        }

        public virtual void FaceTarget(Vector2 targetPosition)
        {
            if (isDying) return;

            FaceDirection(targetPosition.x - transform.position.x);
            rb.linearVelocity = Vector2.zero;
        }

        protected virtual void FaceDirection(float directionX)
        {
            if (directionX != 0)
                spriteRenderer.flipX = directionX < 0;
        }

        public virtual void StartAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead || isDying)) return;
            if (isAttacking) return;

            CancelActiveCoroutines();

            isAttacking = true;
            animator.SetBool("isAttacking", true);
            attackCoroutine = StartCoroutineTracked(AttackCoroutine());
        }

        protected virtual IEnumerator AttackCoroutine()
        {
            float clipLength = GetAttackAnimationLength();
            yield return new WaitForSeconds(attackHitTime);

            if (baseEnemyController != null && !baseEnemyController.IsPlayerDead && !baseEnemyController.IsDead)
                PerformAttack();

            yield return new WaitForSeconds(clipLength - attackHitTime);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
        }

        protected virtual float GetAttackAnimationLength() => 0.5f;

        protected abstract void PerformAttack();

        public virtual void StopMovement()
        {
            if (rb != null)
                rb.linearVelocity = Vector2.zero;

            SetMoving(false);
        }

        public virtual void StartDeathAnimation()
        {
            if (isDying) return;

            CancelActiveCoroutines();

            isDying = true;
            animator.SetBool("isDead", true);

            ApplyDeathForce();
            StartCoroutineTracked(DeathCoroutine());
        }

        protected virtual void CancelActiveCoroutines()
        {
            foreach (var coroutine in activeCoroutines)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
            }

            activeCoroutines.Clear();

            if (isAttacking)
            {
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }
        }

        protected Coroutine StartCoroutineTracked(IEnumerator routine)
        {
            Coroutine coroutine = StartCoroutine(routine);
            activeCoroutines.Add(coroutine);
            return coroutine;
        }

        protected virtual void ApplyDeathForce()
        {
            float xDirection = spriteRenderer.flipX ? 1f : -1f;
            Vector2 flyAwayForce = new Vector2(flyAwayForceX * xDirection, flyAwayForceY);
            rb.AddForce(flyAwayForce, ForceMode2D.Impulse);
        }

        protected virtual IEnumerator DeathCoroutine()
        {
            float length = enemyDeathClip != null ? (enemyDeathClip.length + deathClipDurationOffset) : 0.5f;
            yield return new WaitForSeconds(flyAwayDuration);
            rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(length - flyAwayDuration);
            baseEnemyController.OnDeathAnimationComplete();
        }

        public virtual void ResetDeathState()
        {
            isDying = false;
            animator.SetBool("isDead", false);
        }

        public virtual void ResetAllInputs()
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
            isMoving = false;
        }

        public void UpdateHealthBar(float maxValue, float currentValue) => healthBar.value = currentValue / maxValue;
    }
}