using System.Collections;
using DragonBall.Core;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class BuuEnemyView : BaseEnemyView
    {
        [Header("Buu-specific Attack Settings")]
        [SerializeField] private AnimationClip buuKickAnimation;
        [SerializeField] private float hitTime = 0.3f;

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
            float clipLength = buuKickAnimation != null ? buuKickAnimation.length : 0.5f;
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

            Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            Vector2 origin = (Vector2)transform.position + Vector2.Scale(direction, attackOffset);

            var hits = Physics2D.CircleCastAll(origin, attackRadius, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    var playerController = GameService.Instance.playerService.PlayerController;
                    if (!playerController.PlayerModel.IsDead)
                        playerController.TakeDamage(baseEnemyController.BaseEnemyModel.AttackDamage);
                }
            }
        }
    }
}