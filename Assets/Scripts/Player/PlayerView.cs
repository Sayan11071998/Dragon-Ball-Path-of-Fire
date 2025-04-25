using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonBall.Player
{
    public class PlayerView : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private CapsuleCollider2D capsuleCollider2D;

        private float moveInput;
        private bool isJumping;
        private bool isVanishing;

        public Animator Animator => animator;
        public Rigidbody2D Rigidbody => rb;

        public float MoveInput => moveInput;
        public bool JumpInput => isJumping;
        public bool VanishInput => isVanishing;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        public void OnMove(InputValue value) => moveInput = value.Get<Vector2>().x;

        public void OnJump() => isJumping = true;

        public void OnVanish() => isVanishing = true;

        public void ResetJumpInput() => isJumping = false;

        public void ResetVanishInput() => isVanishing = false;

        public bool IsTouchingGround() => capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        public void FlipSprite(bool isFacingRight) => spriteRenderer.flipX = !isFacingRight;
    }
}