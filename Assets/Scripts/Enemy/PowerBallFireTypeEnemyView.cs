using DragonBall.Core;
using DragonBall.Bullet;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class PowerBallFireTypeEnemyView : BaseEnemyView
    {
        [Header("PowerBallFire specific Attack Settings")]
        [SerializeField] private AnimationClip buuFireAnimation;

        [Header("Bullet Settings")]
        [SerializeField] protected Transform firePoint;
        [SerializeField] protected BulletType bulletType = BulletType.EnemyRegularPowerBall;

        protected override float GetAttackAnimationLength() => buuFireAnimation != null ? buuFireAnimation.length : 0.6f;

        protected override void PerformAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead)) return;

            Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

            GameService.Instance.bulletService.FireBullet(
                bulletType,
                firePoint.position,
                direction,
                BulletTargetType.Player
            );
        }

        protected override void ApplyDeathForce()
        {
            float xDirection = spriteRenderer.flipX ? 1f : -1f;
            Vector2 flyAwayForce = new Vector2(flyAwayForceX * xDirection, flyAwayForceY);
            rb.AddForce(flyAwayForce, ForceMode2D.Impulse);
        }
    }
}