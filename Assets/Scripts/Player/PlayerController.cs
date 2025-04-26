using DragonBall.Core;
using DragonBall.Enemy;
using DragonBall.VFX;
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
            HandleDodge();
            HandleKick();
            UpdateAnimations(moveInput);
        }

        private void HandleMovement(float moveInput)
        {
            if (playerModel.IsDodging)
                return;

            var velocity = playerView.Rigidbody.linearVelocity;
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
            if (!playerView.JumpInput)
                return;

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

        private void HandleVanish()
        {
            if (!playerView.VanishInput)
                return;

            Vector2 originalPosition = playerView.transform.position;
            Vector2 randomOffset = Random.insideUnitCircle * playerModel.VanishRange;
            if (randomOffset.y < 0)
                randomOffset.y = Mathf.Abs(randomOffset.y);

            GameService.Instance.vFXService.PlayVFXAtPosition(VFXType.VanishEffect, originalPosition);
            Vector2 newPosition = originalPosition + randomOffset;
            playerView.transform.position = new Vector3(newPosition.x, newPosition.y, playerView.transform.position.z);
            playerView.ResetVanishInput();
        }

        private void HandleDodge()
        {
            if (playerView.DodgeInput && playerModel.IsGrounded && Time.time > playerModel.LastDodgeTime + playerModel.DodgeCooldown)
            {
                playerModel.IsDodging = true;
                playerModel.DodgeEndTime = Time.time + playerModel.DodgeDuration;
                playerModel.LastDodgeTime = Time.time;
                Vector2 dir = playerModel.IsFacingRight ? Vector2.left : Vector2.right;
                playerView.Rigidbody.linearVelocity = new Vector2(dir.x * playerModel.DodgeSpeed, playerView.Rigidbody.linearVelocity.y);
                playerView.SetDodgeAnimation(true);
            }

            playerView.ResetDodgeInput();

            if (playerModel.IsDodging && Time.time > playerModel.DodgeEndTime)
            {
                playerModel.IsDodging = false;
                playerView.SetDodgeAnimation(false);
            }
        }

        private void HandleKick()
        {
            if (!playerView.KickInput)
                return;

            if (playerModel.IsKickOnCooldown)
            {
                playerView.ResetKickInput();
                return;
            }

            playerModel.LastKickTime = Time.time;
            playerView.PlayKickAnimation();
            PerformKickAttack();
            playerView.ResetKickInput();
        }

        private void PerformKickAttack()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                playerView.AttackTransform.position,
                playerModel.KickAttackRange,
                playerView.transform.right,
                0f,
                playerView.AttackableLayer);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var target))
                    target.Damage(playerModel.KickAttackPower);
            }
        }

        private void UpdateAnimations(float moveInput)
        {
            playerView.UpdateRunAnimation(Mathf.Abs(moveInput) > 0.1f && !playerModel.IsDodging);
            playerView.UpdateJumpAnimation(!playerModel.IsGrounded);
            playerView.SetDodgeAnimation(playerModel.IsDodging);
        }
    }
}