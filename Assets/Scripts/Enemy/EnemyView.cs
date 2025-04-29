using System.Collections;
using DragonBall.Core;
using DragonBall.Player;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyView : MonoBehaviour, IDamageable
    {
        [Header("Attack Settings")]
        [SerializeField] private AnimationClip enemyKickAnimation;
        [SerializeField] private AnimationClip enemyDeathClip;
        [SerializeField] private float attackRadius = 0.5f;
        [SerializeField] private Vector2 attackOffset = new Vector2(0.5f, 0f);
        [SerializeField] private float hitTime = 0.3f;
        [SerializeField] private float deathClipDurationOffset = 0.5f;

        [Header("Death Fly Away Settings")]
        [SerializeField] private float flyAwayForceX = 5f;
        [SerializeField] private float flyAwayForceY = 2f;
        [SerializeField] private float flyAwayDuration = 0.3f;

        private EnemyController enemyController;
        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private bool isMoving = false;
        private bool isAttacking = false;
        private bool isDying = false;

        public bool IsAttacking => isAttacking;

        public void SetController(EnemyController controllerToSet) => enemyController = controllerToSet;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            animator.SetBool("isDead", false);
        }

        private void FixedUpdate() => enemyController?.Update();

        public void Damage(float damageValue) => enemyController?.TakeDamage(damageValue);

        public void SetMoving(bool moving)
        {
            if (isMoving == moving) return;
            isMoving = moving;
            animator.SetBool("isMoving", isMoving);
        }

        public void MoveInDirection(Vector2 direction, float speed)
        {
            float velocityX = direction.x * speed;
            rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);
            FaceDirection(direction.x);
        }

        public void FaceTarget(Vector2 targetPosition)
        {
            FaceDirection(targetPosition.x - transform.position.x);
            rb.linearVelocity = Vector2.zero;
        }

        private void FaceDirection(float directionX)
        {
            if (directionX != 0)
                spriteRenderer.flipX = directionX < 0;
        }

        public void StartAttack()
        {
            if (isAttacking) return;
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            float clipLength = enemyKickAnimation != null ? enemyKickAnimation.length : 0.5f;
            yield return new WaitForSeconds(hitTime);
            PerformCircleKick();
            yield return new WaitForSeconds(clipLength - hitTime);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
        }

        private void PerformCircleKick()
        {
            Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            Vector2 origin = (Vector2)transform.position + Vector2.Scale(direction, attackOffset);

            var hits = Physics2D.CircleCastAll(origin, attackRadius, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                    GameService.Instance.playerService.PlayerController.TakeDamage(enemyController.EnemyModel.AttackDamage);
            }
        }

        public void StopMovement()
        {
            if (rb != null)
                rb.linearVelocity = Vector2.zero;

            SetMoving(false);
        }

        public void StartDeathAnimation()
        {
            if (isDying) return;

            isDying = true;
            animator.SetBool("isDead", true);

            float xDirection = spriteRenderer.flipX ? 1f : -1f;
            Vector2 flyAwayForce = new Vector2(flyAwayForceX * xDirection, flyAwayForceY);
            rb.AddForce(flyAwayForce, ForceMode2D.Impulse);

            StartCoroutine(DeathCoroutine());
        }

        private IEnumerator DeathCoroutine()
        {
            float length = enemyDeathClip != null ? (enemyDeathClip.length + deathClipDurationOffset) : 0.5f;
            yield return new WaitForSeconds(flyAwayDuration);
            rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(length - flyAwayDuration);
            enemyController.OnDeathAnimationComplete();
        }

        public void ResetDeathState()
        {
            isDying = false;
            animator.SetBool("isDead", false);
        }

        public void ResetAllInputs()
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
        }
    }
}