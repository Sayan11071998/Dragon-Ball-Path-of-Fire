using DragonBall.Bullet;
using DragonBall.Core;
using DragonBall.Enemy;
using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Player
{
    public abstract class BasePlayerState : IState
    {
        protected PlayerController playerController;
        protected PlayerStateMachine stateMachine;
        protected PlayerModel playerModel;
        protected PlayerView playerView;

        public BasePlayerState(PlayerController controller, PlayerStateMachine stateMachine)
        {
            this.playerController = controller;
            this.stateMachine = stateMachine;
            this.playerModel = controller.PlayerModel;
            this.playerView = controller.PlayerView;
        }

        public abstract void OnStateEnter();

        public virtual void Update()
        {
            ResetUnhandledInputs();
            UpdateAnimations(playerView.MoveInput);
        }

        public abstract void OnStateExit();

        protected virtual void ResetUnhandledInputs() { }

        protected void UpdateAnimations(float moveInput)
        {
            playerView.UpdateRunAnimation(Mathf.Abs(moveInput) > 0.1f && !playerModel.IsDodging);
            playerView.UpdateJumpAnimation(!playerModel.IsGrounded);
            playerView.SetDodgeAnimation(playerModel.IsDodging);
        }

        protected void HandleKick()
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

        protected void PerformKickAttack()
        {
            Vector2 attackDirection = playerModel.IsFacingRight ? Vector2.right : Vector2.left;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                playerView.AttackTransform.position,
                playerModel.KickAttackRange,
                attackDirection,
                0f,
                playerView.AttackableLayer);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var target))
                    target.Damage(playerModel.KickAttackPower);
            }
        }

        protected void HandleFire()
        {
            if (!playerView.FireInput)
                return;

            if (playerModel.IsFireOnCooldown)
            {
                playerView.ResetFireInput();
                return;
            }

            playerModel.LastFireTime = Time.time;
            playerView.PlayFireAnimation();
            FireBullet();
            playerView.ResetFireInput();
        }

        protected virtual void FireBullet()
        {
            Vector2 position = playerView.FireTransform.position;
            Vector2 direction = playerModel.IsFacingRight ? Vector2.right : Vector2.left;
            GameService.Instance.bulletService.FireBullet(BulletType.Regular, position, direction);
        }
    }
}