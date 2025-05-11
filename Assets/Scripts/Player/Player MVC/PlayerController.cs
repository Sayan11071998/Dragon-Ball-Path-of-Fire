using DragonBall.Core;
using DragonBall.Player.PlayerData;
using DragonBall.Player.PlayerUtilities;
using DragonBall.Sound.SoundData;
using DragonBall.Sound.SoundUtilities;
using DragonBall.VFX;
using UnityEngine;

namespace DragonBall.Player.PlayerMVC
{
    public class PlayerController
    {
        private PlayerModel playerModel;
        private PlayerView playerView;
        private PlayerStateMachine stateMachine;

        public PlayerModel PlayerModel => playerModel;
        public PlayerView PlayerView => playerView;
        public PlayerStateMachine PlayerStateMachine => stateMachine;

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

            HandleGroundCheck();

            if (isInputEnabled)
                HandleMovement();

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
            Vector2 moveDirection = playerView.MovementDirection;

            if (playerModel.IsDodging) return;

            var velocity = playerView.Rigidbody.linearVelocity;

            if (playerModel.IsFlying)
            {
                if (Mathf.Abs(moveDirection.x) > 0.1f)
                    velocity.x = moveDirection.x * playerModel.MoveSpeed;
                else
                    velocity.x = 0;

                if (Mathf.Abs(moveDirection.y) > 0.1f)
                    velocity.y = moveDirection.y * playerModel.FlySpeed;
                else
                    velocity.y = 0;
            }
            else
            {
                velocity.x = moveInput * playerModel.MoveSpeed;
            }

            playerView.Rigidbody.linearVelocity = velocity;

            if (moveDirection.x > 0 && !playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = true;
                playerView.FlipCharacter(true);
            }
            else if (moveDirection.x < 0 && playerModel.IsFacingRight)
            {
                playerModel.IsFacingRight = false;
                playerView.FlipCharacter(false);
            }
        }

        public void HandleJump()
        {
            if (!playerView.JumpInput) return;

            var velocity = playerView.Rigidbody.linearVelocity;

            if (playerModel.IsGrounded)
            {
                velocity.y = playerModel.JumpSpeed;
                velocity.x *= playerModel.JumpHorizontalDampening;
                playerModel.JumpCount++;
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuJump);
            }
            else if (!playerModel.IsGrounded && playerModel.JumpCount < 1)
            {
                velocity.y = playerModel.JumpSpeed;
                velocity.x *= playerModel.JumpHorizontalDampening;
                GameService.Instance.vFXService.PlayVFXAtPosition(VFXType.JumpEffect, playerView.transform.position);
                SoundManager.Instance.PlaySoundEffect(SoundType.GokuJump);
                playerModel.JumpCount++;
            }
            playerView.Rigidbody.linearVelocity = velocity;
            playerView.ResetJumpInput();
        }

        public void HandleFlight()
        {
            if (!playerView.FlyInput) return;

            playerModel.IsFlying = !playerModel.IsFlying;
            playerView.Rigidbody.linearVelocity = Vector2.zero;
            playerView.ResetMovementDirection();

            if (playerModel.IsFlying)
            {
                playerView.UpdateFlightAnimation(true);
                playerView.Rigidbody.gravityScale = 0f;
                playerView.StartFlightSound();
            }
            else
            {
                playerView.UpdateFlightAnimation(false);
                playerView.Rigidbody.gravityScale = 1f;
                playerView.StopFlightSound();
            }

            playerView.ResetFlyInput();
        }

        public void CollectDragonBall()
        {
            playerModel.IncrementDragonBallCount();
            SoundManager.Instance.PlaySoundEffect(SoundType.DragonBallCollect);
        }

        public void TakeDamage(float damage)
        {
            if (playerModel.IsDead) return;

            playerModel.TakeDamage(damage);
            GameService.Instance.cameraShakeService.ShakeCamera(0.1f, 0.5f);
            SoundManager.Instance.PlaySoundEffect(SoundType.GokuTakeDamage);

            if (playerModel.IsDead)
                playerView.StartCoroutine(playerView.DeathSequence());
        }

        public bool DisablePlayerController()
        {
            isInputEnabled = false;
            playerView.DisableInput();

            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = 0;
            playerView.Rigidbody.linearVelocity = velocity;

            return isInputEnabled;
        }

        public bool EnablePlayerController()
        {
            var velocity = playerView.Rigidbody.linearVelocity;
            velocity.x = 0;
            playerView.Rigidbody.linearVelocity = velocity;

            isInputEnabled = true;
            playerView.EnableInput();
            return isInputEnabled;
        }

        public PlayerStateMachine GetPlayerStateMachine() => stateMachine;
    }
}