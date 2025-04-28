using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyView : MonoBehaviour, IDamageable
    {
        private EnemyController controller;
        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private bool isMoving = false;

        public void SetController(EnemyController controllerToSet) => controller = controllerToSet;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate() => controller?.FixedUpdate();

        public void Damage(float damageAmount) => controller?.TakeDamage(damageAmount);

        public void SetMoving(bool moving)
        {
            if (isMoving != moving)
            {
                isMoving = moving;
                UpdateAnimator();
            }
        }

        public void MoveInDirection(Vector2 direction, float speed)
        {
            float vx = direction.x * speed;
            rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);

            FaceDirection(direction.x);
        }

        public void FaceTarget(Vector2 targetPosition)
        {
            float directionX = targetPosition.x - transform.position.x;
            FaceDirection(directionX);
            rb.linearVelocity = Vector2.zero;
        }

        private void FaceDirection(float directionX)
        {
            if (directionX != 0)
                spriteRenderer.flipX = directionX < 0;
        }

        private void UpdateAnimator()
        {
            if (animator != null)
            {
                animator.SetBool("isMoving", isMoving);
            }
        }
    }
}