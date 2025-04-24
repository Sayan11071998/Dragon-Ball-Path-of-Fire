using UnityEngine;

namespace DragonBall.Player
{
    public class PlayerController
    {
        private PlayerModel playerModel;
        private PlayerView playerView;

        public PlayerController(PlayerModel _playerModel, PlayerView _playerView)
        {
            playerModel = _playerModel;
            playerView = _playerView;
        }

        public void Update()
        {
            playerModel.IsGrounded = playerView.IsTouchingGround();
            if (playerModel.IsGrounded)
                playerModel.JumpCount = 0;

            float moveInput = playerView.MoveInput;

            HandleMovement(moveInput);
            HandleJump();
            HandleVanish();
            UpdateAnimations(moveInput);
        }

        private void HandleMovement(float moveInput)
        {
            Vector2 velocity = playerView.Rigidbody.linearVelocity;

            if (playerModel.IsGrounded)
                velocity.x = moveInput * playerModel.MoveSpeed;

            playerView.Rigidbody.linearVelocity = velocity;

            if (moveInput > 0 && !playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = true;
                playerView.FlipSprite(true);
            }
            else if (moveInput < 0 && playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = false;
                playerView.FlipSprite(false);
            }
        }

        private void HandleJump()
        {
            if (playerView.JumpInput)
            {
                if (playerModel.IsGrounded)
                {
                    Vector2 velocity = playerView.Rigidbody.linearVelocity;
                    velocity.y = playerModel.JumpSpeed;
                    velocity.x *= playerModel.JumpHorizontalDampening;
                    playerView.Rigidbody.linearVelocity = velocity;
                    playerModel.JumpCount++;
                }
                else if (!playerModel.IsGrounded && playerModel.JumpCount < 1)
                {
                    Vector2 velocity = playerView.Rigidbody.linearVelocity;
                    velocity.y = playerModel.JumpSpeed;
                    velocity.x *= playerModel.JumpHorizontalDampening;
                    playerView.Rigidbody.linearVelocity = velocity;
                    playerView.JumpEffect.Play();
                    playerModel.JumpCount++;
                }

                playerView.ResetJumpInput();
            }
        }

        private void HandleVanish()
        {
            if (playerView.VanishInput)
            {
                playerView.PlayVanishEffect();
                Vector2 originalPosition = playerView.transform.position;
                Vector2 randomOffset = Random.insideUnitCircle * playerModel.VanishRange;

                if (randomOffset.y < 0)
                    randomOffset.y = Mathf.Abs(randomOffset.y);

                Vector2 newPosition = originalPosition + randomOffset;
                playerView.transform.position = new Vector3(newPosition.x, newPosition.y, playerView.transform.position.z);
                playerView.ResetVanishInput();
            }
        }

        private void UpdateAnimations(float moveInput)
        {
            playerView.Animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.1f);
            playerView.Animator.SetBool("isJumping", !playerModel.IsGrounded);
        }
    }
}