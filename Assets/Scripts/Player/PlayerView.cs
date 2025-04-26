using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonBall.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Transform attackTransform;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private LayerMask attackableLayer;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private CapsuleCollider2D capsuleCollider2D;

        private float moveInput;
        private bool isJumping;
        private bool isVanishing;
        private bool isDodging;
        private bool isKicking;

        // Public accessors for Controller to use
        public Rigidbody2D Rigidbody => rb;
        public Animator Animator => animator;
        public Transform AttackTransform => attackTransform;
        public LayerMask AttackableLayer => attackableLayer;
        public float AttackRange => attackRange;

        // Input flags
        public float MoveInput => moveInput;
        public bool JumpInput => isJumping;
        public bool VanishInput => isVanishing;
        public bool DodgeInput => isDodging;
        public bool KickInput => isKicking;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        // Input system callbacks
        public void OnMove(InputValue value) => moveInput = value.Get<Vector2>().x;
        public void OnJump() => isJumping = true;
        public void OnVanish() => isVanishing = true;
        public void OnDodge() => isDodging = true;
        public void OnKick() => isKicking = true;

        // Reset input flags after processing
        public void ResetJumpInput() => isJumping = false;
        public void ResetVanishInput() => isVanishing = false;
        public void ResetDodgeInput() => isDodging = false;
        public void ResetKickInput() => isKicking = false;

        // Character state checks
        public bool IsTouchingGround() => capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        // Visual updates
        public void FlipSprite(bool isFacingRight) => spriteRenderer.flipX = !isFacingRight;

        // Animation controls
        public void UpdateRunAnimation(bool isRunning) => animator.SetBool("isRunning", isRunning);
        public void UpdateJumpAnimation(bool isJumping) => animator.SetBool("isJumping", isJumping);
        public void SetDodgeAnimation(bool isDodging) => animator.SetBool("isDodging", isDodging);
        public void PlayKickAnimation() => animator.SetTrigger("isKickingTrigger");

        private void OnDrawGizmosSelected()
        {
            if (attackTransform != null)
                Gizmos.DrawWireSphere(attackTransform.position, attackRange);
        }
    }
}