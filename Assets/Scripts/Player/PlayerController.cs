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
            float moveInput = playerView.MoveInput;

            HandleMovement(moveInput);
            UpdateAnimations(moveInput);
        }

        private void HandleMovement(float moveInput)
        {
            Vector2 velocity = playerView.Rigidbody.linearVelocity;
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

        private void UpdateAnimations(float moveInput)
        {
            playerView.Animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.1f);
        }
    }
}