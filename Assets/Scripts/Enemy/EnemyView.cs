using System.Collections;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyView : MonoBehaviour, IDamageable
    {
        [Header("Attack Settings")]
        [SerializeField] private AnimationClip kickAnimation;
        [SerializeField] private AnimationClip deathClip;
        [SerializeField] private float attackRadius = 0.5f;
        [SerializeField] private Vector2 attackOffset = new Vector2(0.5f, 0f);
        [SerializeField] private float hitTime = 0.3f;
        [SerializeField] private float deathClipDurationOffset = 0.5f;

        private EnemyController controller;
        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private bool isMoving = false;
        private bool isAttacking = false;
        private bool isDying = false;

        public bool IsAttacking => isAttacking;

        public void SetController(EnemyController c) => controller = c;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            animator.SetBool("isDead", false);
        }

        private void FixedUpdate()
        {
            controller?.FixedUpdate();
        }

        public void Damage(float dmg) => controller?.TakeDamage(dmg);

        public void SetMoving(bool moving)
        {
            if (isMoving == moving) return;
            isMoving = moving;
            animator.SetBool("isMoving", isMoving);
        }

        public void MoveInDirection(Vector2 dir, float speed)
        {
            float vx = dir.x * speed;
            rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
            FaceDirection(dir.x);
        }

        public void FaceTarget(Vector2 targetPosition)
        {
            FaceDirection(targetPosition.x - transform.position.x);
            rb.linearVelocity = Vector2.zero;
        }

        private void FaceDirection(float dx)
        {
            if (dx != 0) spriteRenderer.flipX = dx < 0;
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
            float clipLength = kickAnimation != null ? kickAnimation.length : 0.5f;
            yield return new WaitForSeconds(hitTime);
            PerformCircleKick();
            yield return new WaitForSeconds(clipLength - hitTime);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
        }

        private void PerformCircleKick()
        {
            Vector2 dir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            Vector2 origin = (Vector2)transform.position + Vector2.Scale(dir, attackOffset);

            var hits = Physics2D.CircleCastAll(origin, attackRadius, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                    Debug.Log("Player got damaged");
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
            StartCoroutine(DeathCoroutine());
        }

        private IEnumerator DeathCoroutine()
        {
            float length = deathClip != null ? (deathClip.length + deathClipDurationOffset) : 0.5f;
            yield return new WaitForSeconds(length);
            controller.OnDeathAnimationComplete();
        }

        public void ResetDeathState()
        {
            isDying = false;
            animator.SetBool("isDead", false);
        }
    }
}