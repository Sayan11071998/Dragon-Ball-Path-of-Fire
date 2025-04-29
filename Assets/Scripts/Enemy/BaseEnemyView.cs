using System.Collections;
using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Enemy
{
    public abstract class BaseEnemyView : MonoBehaviour, IDamageable
    {
        [Header("Base Attack Settings")]
        [SerializeField] protected AnimationClip enemyDeathClip;
        [SerializeField] protected float attackRadius = 0.5f;
        [SerializeField] protected Vector2 attackOffset = new Vector2(0.5f, 0f);
        [SerializeField] protected float deathClipDurationOffset = 0.5f;

        [Header("Death Fly Away Settings")]
        [SerializeField] protected float flyAwayForceX = 5f;
        [SerializeField] protected float flyAwayForceY = 2f;
        [SerializeField] protected float flyAwayDuration = 0.3f;

        protected BaseEnemyController enemyController;
        protected Rigidbody2D rb;
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;

        protected bool isMoving = false;
        protected bool isAttacking = false;
        protected bool isDying = false;

        public bool IsAttacking => isAttacking;

        public virtual void SetController(BaseEnemyController controllerToSet) => enemyController = controllerToSet;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            animator.SetBool("isDead", false);
        }

        protected virtual void FixedUpdate() => enemyController?.Update();

        public virtual void Damage(float damageValue) => enemyController?.TakeDamage(damageValue);

        public virtual void SetMoving(bool moving)
        {
            if (isMoving == moving) return;
            isMoving = moving;
            animator.SetBool("isMoving", isMoving);
        }

        public virtual void MoveInDirection(Vector2 direction, float speed)
        {
            if (enemyController != null && enemyController.isPlayerDead)
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
            FaceDirection(targetPosition.x - transform.position.x);
            rb.linearVelocity = Vector2.zero;
        }

        protected virtual void FaceDirection(float directionX)
        {
            if (directionX != 0)
                spriteRenderer.flipX = directionX < 0;
        }

        public abstract void StartAttack();

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

            isDying = true;
            animator.SetBool("isDead", true);

            float xDirection = spriteRenderer.flipX ? 1f : -1f;
            Vector2 flyAwayForce = new Vector2(flyAwayForceX * xDirection, flyAwayForceY);
            rb.AddForce(flyAwayForce, ForceMode2D.Impulse);

            StartCoroutine(DeathCoroutine());
        }

        protected virtual IEnumerator DeathCoroutine()
        {
            float length = enemyDeathClip != null ? (enemyDeathClip.length + deathClipDurationOffset) : 0.5f;
            yield return new WaitForSeconds(flyAwayDuration);
            rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(length - flyAwayDuration);
            enemyController.OnDeathAnimationComplete();
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
    }
}