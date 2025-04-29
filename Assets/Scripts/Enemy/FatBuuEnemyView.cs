using System.Collections;
using DragonBall.Core;
using DragonBall.Bullet;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class FatBuuEnemyView : BaseEnemyView
    {
        [Header("FatBuu-specific Attack Settings")]
        [SerializeField] private AnimationClip buuFireAnimation;
        [SerializeField] private float hitTime = 0.4f;

        [Header("Bullet Settings")]
        [SerializeField] private BulletType bulletType = BulletType.EnemyRegularPowerBall;
        [SerializeField] private Vector2 bulletSpawnOffset = new Vector2(0.8f, 0.2f);

        public override void StartAttack()
        {
            if (baseEnemyController != null && baseEnemyController.IsPlayerDead) return;

            if (isAttacking) return;
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            float clipLength = buuFireAnimation != null ? buuFireAnimation.length : 0.6f;
            yield return new WaitForSeconds(hitTime);

            if (baseEnemyController != null && !baseEnemyController.IsPlayerDead)
                PerformAttack();

            yield return new WaitForSeconds(clipLength - hitTime);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
        }

        protected override void PerformAttack()
        {
            if (baseEnemyController != null && baseEnemyController.IsPlayerDead) return;

            Debug.Log("Fat Buu is firing a bullet!");

            Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            Vector2 bulletSpawnPoint = (Vector2)transform.position + new Vector2(spriteRenderer.flipX ? -bulletSpawnOffset.x : bulletSpawnOffset.x, bulletSpawnOffset.y);

            GameService.Instance.bulletService.FireBullet(
                bulletType,
                bulletSpawnPoint,
                direction,
                BulletTargetType.Player
            );
        }
    }
}