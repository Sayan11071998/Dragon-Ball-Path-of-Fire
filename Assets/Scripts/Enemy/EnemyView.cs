using System.Collections;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Transform attackTransform;
        [SerializeField] private LayerMask attackableLayer;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;

        public Transform AttackTransform => attackTransform;
        public LayerMask AttackableLayer => attackableLayer;
        public Animator Animator => animator;
        public Rigidbody2D Rigidbody => rb;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize()
        {
            gameObject.SetActive(true);
        }

        public void FlipCharacter(bool isFacingRight)
        {
            spriteRenderer.flipX = !isFacingRight;
        }

        public void UpdateMoveAnimation(bool isMoving)
        {
            animator.SetBool("isMoving", isMoving);
        }

        public void PlayAttackAnimation()
        {
            animator.SetTrigger("attackTrigger");
        }

        public void PlayDeathAnimation()
        {
            animator.SetTrigger("deathTrigger");
        }

        public void SetVelocity(Vector2 velocity)
        {
            rb.linearVelocity = velocity;
        }

        public void DeactivateEnemy()
        {
            gameObject.SetActive(false);
        }

        public IEnumerator PlayDeathSequence(float delay)
        {
            PlayDeathAnimation();
            yield return new WaitForSeconds(delay);
            DeactivateEnemy();
        }
    }
}