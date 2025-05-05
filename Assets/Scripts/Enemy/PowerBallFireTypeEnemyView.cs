using System.Collections;
using DragonBall.Core;
using DragonBall.Bullet;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class PowerBallFireTypeEnemyView : BaseEnemyView
    {
        [Header("PowerBallFire specific Attack Settings")]
        [SerializeField] private AnimationClip buuFireAnimation;
        [SerializeField] private float hitTime = 0.4f;

        [Header("Bullet Settings")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private BulletType bulletType = BulletType.EnemyRegularPowerBall;

        private Coroutine attackCoroutine;

        public override void StartAttack()
        {
            if (baseEnemyController != null && (baseEnemyController.IsPlayerDead || baseEnemyController.IsDead)) return;
            if (isAttacking) return;

            isAttacking = true;
            animator.SetBool("isAttacking", true);
            attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            float clipLength = buuFireAnimation != null ? buuFireAnimation.length : 0.6f;
            yield return new WaitForSeconds(hitTime);

            if (baseEnemyController != null && !baseEnemyController.IsPlayerDead && !baseEnemyController.IsDead)
                PerformAttack();

            yield return new WaitForSeconds(clipLength - hitTime);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
        }

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

        public override void StartDeathAnimation()
        {
            if (isDying) return;

            if (isAttacking)
            {
                if (attackCoroutine != null)
                    StopCoroutine(attackCoroutine);

                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }

            isDying = true;
            animator.SetBool("isDead", true);

            float xDirection = spriteRenderer.flipX ? 1f : -1f;
            Vector2 flyAwayForce = new Vector2(flyAwayForceX * xDirection, flyAwayForceY);
            rb.AddForce(flyAwayForce, ForceMode2D.Impulse);

            StartCoroutine(DeathCoroutine());
        }
    }
}