using DragonBall.Core;
using DragonBall.VFX;
using UnityEngine;

namespace DragonBall.Player
{
    public class PlayerController
    {
        private PlayerModel playerModel;
        private PlayerView playerView;
        private PlayerStateMachine stateMachine;

        public PlayerModel PlayerModel => playerModel;
        public PlayerView PlayerView => playerView;

        private bool isInputEnabled = true;

        public PlayerController(PlayerModel _playerModel, PlayerView _playerView)
        {
            playerModel = _playerModel;
            playerView = _playerView;

            playerView.SetPlayerController(this);

            stateMachine = new PlayerStateMachine(this);
            stateMachine.ChangeState(PlayerState.NORMAL);
        }

        public void Update()
        {
            if (playerModel.IsDead) return;
            if (!isInputEnabled) return;

            HandleGroundCheck();
            HandleMovement();
            HandleJump();

            stateMachine.Update();
        }

        private void HandleGroundCheck()
        {
            playerModel.IsGrounded = playerView.IsTouchingGround();

            if (playerModel.IsGrounded)
                playerModel.JumpCount = 0;
        }

        private void HandleMovement()
        {
            float moveInput = playerView.MoveInput;

            if (playerModel.IsDodging) return;

            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = moveInput * playerModel.MoveSpeed;
            playerView.Rigidbody.linearVelocity = velocity;

            if (moveInput > 0 && !playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = true;
                playerView.FlipCharacter(true);
            }
            else if (moveInput < 0 && playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = false;
                playerView.FlipCharacter(false);
            }
        }

        private void HandleJump()
        {
            if (!playerView.JumpInput) return;

            var velocity = playerView.Rigidbody.linearVelocity;

            if (playerModel.IsGrounded)
            {
                velocity.y = playerModel.JumpSpeed;
                velocity.x *= playerModel.JumpHorizontalDampening;
                playerModel.JumpCount++;
            }
            else if (!playerModel.IsGrounded && playerModel.JumpCount < 1)
            {
                velocity.y = playerModel.JumpSpeed;
                velocity.x *= playerModel.JumpHorizontalDampening;
                GameService.Instance.vFXService.PlayVFXAtPosition(VFXType.JumpEffect, playerView.transform.position);
                playerModel.JumpCount++;
            }

            playerView.Rigidbody.linearVelocity = velocity;
            playerView.ResetJumpInput();
        }

        public void CollectDragonBall()
        {
            playerModel.IncrementDragonBallCount();
            Debug.Log("Dragon Ball collected! Total: " + playerModel.DragonBallCount);
        }

        public void TakeDamage(float damage)
        {
            if (playerModel.IsDead) return;

            playerModel.TakeDamage(damage);

            if (playerModel.IsDead)
                playerView.StartCoroutine(playerView.DeathSequence());
        }

        public bool DisablePlayerController() => isInputEnabled = false;
        public bool EnablePlayerController() => isInputEnabled = true;
    }
}