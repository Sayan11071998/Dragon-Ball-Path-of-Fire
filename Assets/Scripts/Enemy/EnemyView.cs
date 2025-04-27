using UnityEngine;

namespace DragonBall.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyView : MonoBehaviour
    {
        private Animator animator;
        private Rigidbody2D rb;
        private CapsuleCollider2D capsuleCollider2D;
        private EnemyController controller;
        
        [SerializeField] private float attackDamage = 10f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        public void SetController(EnemyController controllerToSet) => controller = controllerToSet;

        public void SetPosition(Vector3 position) => transform.position = position;

        public void MoveTo(Vector3 target, float speed)
        {
            animator.SetBool("isMoving", true);
            Vector3 direction = (target - transform.position).normalized;
            
            // Flip the sprite based on direction
            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
                
            transform.position += direction * speed * Time.deltaTime;
        }

        public void StopMoving()
        {
            animator.SetBool("isMoving", false);
        }

        public void Attack()
        {
            StopMoving();
            animator.SetTrigger("attack");
        }

        public void Die()
        {
            animator.SetTrigger("death");
            capsuleCollider2D.enabled = false;
            
            // Disable after animation completes
            Invoke(nameof(DisableAfterDeath), 1.5f);
        }
        
        private void DisableAfterDeath()
        {
            gameObject.SetActive(false);
        }
        
        // This method can be called from animation events
        public void DealDamage()
        {
            // Check for player in attack range
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
            foreach (Collider2D hit in hits)
            {
                // Check if it's the player
                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null && hit.CompareTag("Player"))
                {
                    damageable.Damage(attackDamage);
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Handle bullet collisions
            if (collision.CompareTag("Bullet") && controller != null)
            {
                // Let the BulletController handle the damage calculation
                // We don't need to manually access the damage value
                // The BulletView's OnTriggerEnter2D will detect our IDamageable interface
                // and call controller.OnCollision(target) automatically
            }
        }
    }
}