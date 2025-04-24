using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonBall.Player
{
    public class PlayerView : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;

        private float moveInput;
        private bool isJumping;

        public Animator Animator => animator;
        public Rigidbody2D Rigidbody => rb;

        public float MoveInput => moveInput;
        public bool JumpInput => isJumping;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void OnMove(InputValue value) => moveInput = value.Get<Vector2>().x;
        // public void OnJump(InputValue value)
        // {
        //     isJumping = value.isPressed;
        // }

        public void FlipSprite(bool isFacingRight)
        {
            spriteRenderer.flipX = !isFacingRight;
        }

        // public bool CheckGrounded()
        // {
        //     float rayLength = 0.1f;

        //     RaycastHit2D hit = Physics2D.Raycast(
        //         transform.position - new Vector3(0, GetComponent<Collider2D>().bounds.extents.y, 0),
        //         Vector2.down, rayLength, LayerMask.GetMask("Ground"));

        //     return hit.collider != null;
        // }
    }
}